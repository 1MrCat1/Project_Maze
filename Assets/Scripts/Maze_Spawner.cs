using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Maze;

public class Maze_Spawner : MonoBehaviour
{
    //prefabs for maze parts and parent object
    public GameObject p_center;
    public GameObject p_border_l;
    public GameObject p_crossing_l;
    public GameObject p_border_m;
    public GameObject p_crossing_m;
    public GameObject parent;
    public GameObject p_ball;

    public int board_height;
    public int board_width;
    private Maze _maze;
    private bool mazeExists = false;
    private GameObject[,] board_lower;
    private GameObject[,] board_maze;
    private GameObject[] anchors;
    private GameObject ball;
    private Vector3 originalParentPosition;
    private Vector3 blc; // coordinates of bottom left crossing of the lower level of the board
    private Vector3 maze_layer_offset;
    private float _offset_x,_offset_z,_offset_y;

    // Start is called before the first frame update
    void Start()
    {  
        _maze = new Maze(board_height,board_width,0,0);
        originalParentPosition = parent.transform.position;
        board_lower = new GameObject[board_height*2+1,board_width*2+1];
        board_maze = new GameObject[board_height*2+1,board_width*2+1];
        anchors = new GameObject[4];
        _offset_x = (p_center.transform.localScale.x+p_crossing_l.transform.localScale.x)/2f;
        _offset_z = (p_center.transform.localScale.z+p_crossing_l.transform.localScale.z)/2f;
        var full_width = p_center.transform.localScale.x*board_width+p_crossing_l.transform.localScale.x*(board_width+1);
        var full_height = p_center.transform.localScale.z*board_height+p_crossing_l.transform.localScale.z*(board_height+1);
        blc = new Vector3(
            parent.transform.position.x - full_width/2f-p_crossing_l.transform.localScale.x/2f,
            parent.transform.position.y,
            parent.transform.position.z - full_height/2f-p_crossing_l.transform.localScale.z/2f
        );
        _offset_y = p_center.transform.localScale.y;
        maze_layer_offset = new Vector3(0f,p_center.transform.localScale.y,0f);
        Debug.Log("Full_width " + full_width + " | Full_height "+full_height);
        Debug.Log("blc " + blc);

    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void RegenerateMaze(){
        if(mazeExists){
            DestroyMaze();
        }
        CreateMaze();
        mazeExists=true;
    }
    void CreateMaze(){
        _maze.InitMaze();
        _maze.GenerateMaze();
        InstantiateLowerBoard();
        InstantiateByIntArray(_maze);
        InstantiateBall();
        mazeExists = true;
    }
    
    void DestroyMaze(){
        foreach (var obj in board_lower){
            Destroy(obj);
        }
        foreach (var obj in board_maze){
            Destroy(obj);
        }
        foreach (var obj in anchors){
            Destroy(obj);
        }
        Destroy(ball);
        _maze.InitMaze();
        mazeExists=false;
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

    void InstantiateByIntArray(Maze mz){
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

    void InstantiateBall(){
        ball = Instantiate(p_ball,blc+new Vector3(_offset_x,_offset_y*2,_offset_z),Quaternion.identity);
    }

    public void ResetBoardPosition(){
        if(mazeExists){
            parent.transform.position = originalParentPosition;
            parent.transform.rotation = Quaternion.Euler(0f,0f,0f);//(parent.transform.rotation.x);
            ball.transform.position = blc+new Vector3(_offset_x,_offset_y*2,_offset_z);
        }
    }

    public void SetMazeHeight(float val){
        Debug.Log(val);
        //board_height = int.Parse(val);
    }
}
