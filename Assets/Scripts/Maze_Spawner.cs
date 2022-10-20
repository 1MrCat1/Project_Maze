using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static MazeLogic;

public class Maze_Spawner : MonoBehaviour
{
    //prefabs for maze parts and parent object
    public GameObject p_center;
    public GameObject p_border_l;
    public GameObject p_crossing_l;
    public GameObject p_border_m;
    public GameObject p_crossing_m;
    public GameObject p_center_goal;
    public GameObject parent;
    public GameObject p_ball;

    public Slider sl_h;
    public Slider sl_w;

    public int board_height;
    public int board_width;
    public int goal_h;
    public int goal_w;

    private int _board_height_new;
    private int _board_width_new;
    private MazeLogic _maze;
    private bool mazeExists = false;
    private bool sizeChanged = false;
    private GameObject[,] board_lower;
    private GameObject[,] board_maze;
    private GameObject[] anchors;
    private GameObject goal;
    private GameObject ball;
    private Vector3 originalParentPosition;
    private Vector3 blc; // coordinates of bottom left crossing of the lower level of the board
    private Vector3 maze_layer_offset;
    private float _offset_x,_offset_z,_offset_y;

    // Start is called before the first frame update
    void Start()
    {  
        _maze = new MazeLogic(board_height,board_width,0,0);
        anchors = new GameObject[4];
        _board_height_new = board_height;
        _board_width_new = board_width;
        goal_h = board_height-1;
        goal_w = board_width-1;
        originalParentPosition = parent.transform.position;
        _offset_x = (p_center.transform.localScale.x+p_crossing_l.transform.localScale.x)/2f;
        _offset_z = (p_center.transform.localScale.z+p_crossing_l.transform.localScale.z)/2f;

        _offset_y = p_center.transform.localScale.y+0.0001f;
        maze_layer_offset = new Vector3(0f,_offset_y,0f);
        //Debug.Log("Full_width " + full_width + " | Full_height "+full_height);
        //Debug.Log("blc " + blc);
        //slider.OnSliderValueChanged.AddListener(SetMazeHeight);
    }

    // Update is called once per frame
    void Update()
    {
        //_board_height_new = (int)sl_h.value;
        //_board_width_new = (int)sl_w.value;
        if(_board_height_new!=board_height){
            board_height=_board_height_new;
            sizeChanged=true;
        }
        if(_board_width_new!=board_width){
            board_width=_board_width_new;
            sizeChanged=true;
        }
        parent.transform.position = originalParentPosition;
        if(Input.GetKeyDown("c")&& !mazeExists){
            CreateMaze();
        }
        if(Input.GetKeyDown("r")&& mazeExists){
            ResetBoardPosition();
        }
        if(Input.GetKeyDown("f")&& mazeExists){
            DestroyMaze();
        }
        //sizeChanged = false;
    }

    public void RegenerateMaze(){
        if(mazeExists){
            DestroyMaze();
        }
        CreateMaze();
        //mazeExists=true;
        sizeChanged = false;
    }
    void CreateMaze(){
        if(!mazeExists){
            Debug.Log("Regenerating object arrays");
            board_lower = new GameObject[board_height*2+1,board_width*2+1];
            board_maze = new GameObject[board_height*2+1,board_width*2+1];
            RecalculateBlc();
        }
        if(sizeChanged){
            Debug.Log("Regenerating maze arrays");
            _maze.ChangeSize(board_height,board_width);
            RecalculateBlc();
        }
        _maze.InitMaze();
        _maze.GenerateMaze();
        InstantiateLowerBoard();
        InstantiateByIntArray(_maze);
        InstantiateBall();
        InstantiateGoal();
        mazeExists = true;
        sizeChanged = false;
    }
    
    void DestroyMaze(){
        foreach (var obj in board_lower){
            Destroy(obj);
        }
       // Delete(board_lower);
        foreach (var obj in board_maze){
            Destroy(obj);
        }
        //Delete(board_maze);
        foreach (var obj in anchors){
            Destroy(obj);
        }
        //Destroy(anchors);
        Destroy(ball);
        Destroy(goal);
        _maze.InitMaze();
        mazeExists=false;
        parent.transform.position = originalParentPosition;
        parent.transform.rotation = Quaternion.Euler(0f,0f,0f);
        //sizeChanged = false;
    }

    public void ResetBoardPosition(){
        if(mazeExists){
            parent.transform.position = originalParentPosition;
            parent.transform.rotation = Quaternion.Euler(0f,0f,0f);//(parent.transform.rotation.x);
            ball.transform.position = blc+new Vector3(_offset_x,_offset_y*2,_offset_z);
        }
    }

    public void RecalculateBlc(){
        var full_width = p_center.transform.localScale.x*board_width+p_crossing_l.transform.localScale.x*(board_width+1);
        var full_height = p_center.transform.localScale.z*board_height+p_crossing_l.transform.localScale.z*(board_height+1);
        blc = new Vector3(
            parent.transform.position.x - full_width/2f-p_crossing_l.transform.localScale.x/2f,
            parent.transform.position.y,
            parent.transform.position.z - full_height/2f-p_crossing_l.transform.localScale.z/2f
        );
    }

    void InstantiateLowerBoard(){
        for(int y=0;y<board_height*2+1;++y){
            for(int x=0;x<board_width*2+1;++x){
                if(y%2==0){
                    if(x%2==0){
                        board_lower[y,x]=Instantiate(p_crossing_l,blc+new Vector3(_offset_x*x,0,_offset_z*y),Quaternion.identity);
                        board_lower[y,x].transform.SetParent(parent.transform);
                    }
                    else{
                        board_lower[y,x]=Instantiate(p_border_l,blc+new Vector3(_offset_x*x,0,_offset_z*y),Quaternion.identity);
                        board_lower[y,x].transform.SetParent(parent.transform);
                        board_lower[y,x].transform.Rotate(0f,90f,0f);
                    }
                }
                else{
                    if(x%2==0){
                        board_lower[y,x]=Instantiate(p_border_l,blc+new Vector3(_offset_x*x,0,_offset_z*y),Quaternion.identity);
                        board_lower[y,x].transform.SetParent(parent.transform);
                    }
                    else{
                        board_lower[y,x]=Instantiate(p_center,blc+new Vector3(_offset_x*x,0,_offset_z*y),Quaternion.identity);
                        board_lower[y,x].transform.SetParent(parent.transform);
                    }
                }
            }
        }

        anchors[0]=Instantiate(p_crossing_m,blc+new Vector3(-_offset_x-_offset_x,_offset_y,-_offset_z-_offset_z),Quaternion.identity);
        anchors[1]=Instantiate(p_crossing_m,blc+new Vector3(-_offset_x-_offset_x,_offset_y,_offset_z*board_height*2+2*_offset_z),Quaternion.identity);
        anchors[2]=Instantiate(p_crossing_m,blc+new Vector3(_offset_x*board_width*2+2*_offset_x,_offset_y,-_offset_z-_offset_z),Quaternion.identity);
        anchors[3]=Instantiate(p_crossing_m,blc+new Vector3(_offset_x*board_width*2+2*_offset_x,_offset_y,_offset_z*board_height*2+2*_offset_z),Quaternion.identity);
    }

    void InstantiateByIntArray(MazeLogic mz){
        int[,] wh = mz.m_walls_h;
        int[,] wv = mz.m_walls_v;
        for(int x=0;x<board_width*2+1;++x){
                if(x%2==0){
                    board_maze[0,x]=Instantiate(p_crossing_m,blc+new Vector3(_offset_x*x,_offset_y,0),Quaternion.identity);
                    board_maze[0,x].transform.SetParent(parent.transform);
                }
                else{
                    board_maze[0,x]=Instantiate(p_border_m,blc+new Vector3(_offset_x*x,_offset_y,0),Quaternion.identity);
                    board_maze[0,x].transform.SetParent(parent.transform);
                    board_maze[0,x].transform.Rotate(0f,90f,0f);
                }
        }
        for(int y=1;y<board_height*2;++y){
            if(y%2==0){
                board_maze[y,0]=Instantiate(p_crossing_m,blc+new Vector3(0,_offset_y,_offset_z*y),Quaternion.identity);
                board_maze[y,0].transform.SetParent(parent.transform);
            }
            else{
                board_maze[y,0]=Instantiate(p_border_m,blc+new Vector3(0,_offset_y,_offset_z*y),Quaternion.identity);
                board_maze[y,0].transform.SetParent(parent.transform);
            }
            for(int x=1;x<board_width*2;++x){
                if(y%2==0){
                    if(x%2==1){
                        if(wv[y/2-1,x/2]==1){
                            board_maze[y,x]=Instantiate(p_border_m,blc+new Vector3(_offset_x*x,_offset_y,_offset_z*y),Quaternion.identity);
                            board_maze[y,x].transform.SetParent(parent.transform);
                            board_maze[y,x].transform.Rotate(0f,90f,0f);
                        }
                        continue;
                    }
                    //Debug.Log("Crossing.["+y+","+x+"].Real idx:["+y/2+","+x/2+"].");
                    else{
                        board_maze[y,x]=Instantiate(p_crossing_m,blc+new Vector3(_offset_x*x,_offset_y,_offset_z*y),Quaternion.identity);
                        board_maze[y,x].transform.SetParent(parent.transform);
                    }
                }
                else{
                    if(x%2==0 && wh[y/2,x/2-1]==1){
                        board_maze[y,x]=Instantiate(p_border_m,blc+new Vector3(_offset_x*x,_offset_y,_offset_z*y),Quaternion.identity);
                        board_maze[y,x].transform.SetParent(parent.transform);
                    }
                    else{
                        //board_maze[y,x]=Instantiate(p_center,blc+new Vector3(_offset_x*x,_offset_y,_offset_z*y),Quaternion.identity);
                        //board_maze[y,x].transform.SetParent(parent.transform);
                    }
                }
            }
            if(y%2==0){
                board_maze[y,board_width*2]=Instantiate(p_crossing_m,blc+new Vector3(_offset_x*board_width*2,_offset_y,_offset_z*y),Quaternion.identity);
                board_maze[y,board_width*2].transform.SetParent(parent.transform);
            }
            else{
                board_maze[y,board_width*2]=Instantiate(p_border_m,blc+new Vector3(_offset_x*board_width*2,_offset_y,_offset_z*y),Quaternion.identity);
                board_maze[y,board_width*2].transform.SetParent(parent.transform);
            }
        }
        for(int x=0;x<board_width*2+1;++x){
                if(x%2==0){
                    board_maze[board_height*2,x]=Instantiate(p_crossing_m,blc+new Vector3(_offset_x*x,_offset_y,_offset_z*(board_height*2)),Quaternion.identity);
                    board_maze[board_height*2,x].transform.SetParent(parent.transform);
                }
                else{
                    board_maze[board_height*2,x]=Instantiate(p_border_m,blc+new Vector3(_offset_x*x,_offset_y,_offset_z*(board_height*2)),Quaternion.identity);
                    board_maze[board_height*2,x].transform.SetParent(parent.transform);
                    board_maze[board_height*2,x].transform.Rotate(0f,90f,0f);
                }
        }
    }

    void InstantiateGoal(){
        goal = Instantiate(p_center_goal,blc + new Vector3(_offset_x*(2*goal_w+1),_offset_y,_offset_z*(2*goal_h+1)),Quaternion.identity);
        goal.transform.SetParent(parent.transform);
    }
    void InstantiateBall(){
        ball = Instantiate(p_ball,blc+new Vector3(_offset_x,_offset_y*2,_offset_z),Quaternion.identity);
    }

    public void IncrementHeight(){
        if(_board_height_new<15){
            _board_height_new++;
        }
    }
    
    public void DecrementHeight(){
        if(_board_height_new>1){
            _board_height_new--;
        }
    }
    
    public void IncrementWidth(){
        if(_board_width_new<15){
            _board_width_new++;
        }
    }
    
    public void DecrementWidth(){
       if(_board_width_new>1){
            _board_width_new--;
       }
    }
}
