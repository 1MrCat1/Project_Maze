using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Maze;

public class Maze_Spawner : MonoBehaviour
{
    //prefabs for maze parts and parent object
    public GameObject p_center;
    public GameObject p_border;
    public GameObject p_crossing;
    public GameObject parent;

    public int board_height;
    public int board_width;
    private Maze _maze;
    private bool flag = true;
    private GameObject[,] board_lower;
    private GameObject[,] board_maze;
    private Vector3 blc; // coordinates of bottom left crossing of the lower level of the board
    private Vector3 maze_layer_offset;
    private float _offset_x,_offset_z,_offset_y;
    // Start is called before the first frame update
    void Start()
    {
        _maze = new Maze(board_height,board_width,0,0);
        board_lower = new GameObject[board_height*2+1,board_width*2+1];
        board_maze = new GameObject[board_height*2+1,board_width*2+1];
        _offset_x = (p_center.transform.localScale.x+p_crossing.transform.localScale.x)/2f;
        _offset_z = (p_center.transform.localScale.z+p_crossing.transform.localScale.z)/2f;
        var full_width = p_center.transform.localScale.x*board_width+p_crossing.transform.localScale.x*(board_width+1);
        var full_height = p_center.transform.localScale.z*board_height+p_crossing.transform.localScale.z*(board_height+1);
        blc = new Vector3(
            parent.transform.position.x - full_width/2f-p_crossing.transform.localScale.x/2f,
            parent.transform.position.y,
            parent.transform.position.z - full_height/2f-p_crossing.transform.localScale.z/2f
        );
        _offset_y = p_center.transform.localScale.y;
        maze_layer_offset = new Vector3(0f,p_center.transform.localScale.y,0f);
        Debug.Log("Full_width " + full_width + " | Full_height "+full_height);
        Debug.Log("blc " + blc);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("h")&& flag){
            InstantiateLowerBoard();
            InstantiateByIntArray(_maze.m_walls_h,_maze.m_walls_v);
            flag = false;
        }
    }


    void InstantiateLowerBoard(){
        for(int y=0;y<board_height*2+1;++y){
            for(int x=0;x<board_width*2+1;++x){
                if(y%2==0){
                    if(x%2==0){
                        board_lower[y,x]=Instantiate(p_crossing,blc+new Vector3(_offset_x*x,0,_offset_z*y),Quaternion.identity);
                        board_lower[y,x].transform.SetParent(parent.transform);
                    }
                    else{
                        board_lower[y,x]=Instantiate(p_border,blc+new Vector3(_offset_x*x,0,_offset_z*y),Quaternion.identity);
                        board_lower[y,x].transform.SetParent(parent.transform);
                        board_lower[y,x].transform.Rotate(0f,90f,0f);
                    }
                }
                else{
                    if(x%2==0){
                        board_lower[y,x]=Instantiate(p_border,blc+new Vector3(_offset_x*x,0,_offset_z*y),Quaternion.identity);
                        board_lower[y,x].transform.SetParent(parent.transform);
                    }
                    else{
                        board_lower[y,x]=Instantiate(p_center,blc+new Vector3(_offset_x*x,0,_offset_z*y),Quaternion.identity);
                        board_lower[y,x].transform.SetParent(parent.transform);
                    }
                }
            }
        }
    }

    void InstantiateByIntArray(int[,] wh,int[,] wv){
        for(int x=0;x<board_width*2+1;++x){
                if(x%2==0){
                    board_maze[0,x]=Instantiate(p_crossing,blc+new Vector3(_offset_x*x,_offset_y,0),Quaternion.identity);
                    board_maze[0,x].transform.SetParent(parent.transform);
                }
                else{
                    board_maze[0,x]=Instantiate(p_border,blc+new Vector3(_offset_x*x,_offset_y,0),Quaternion.identity);
                    board_maze[0,x].transform.SetParent(parent.transform);
                    board_maze[0,x].transform.Rotate(0f,90f,0f);
                }
        }
        for(int y=1;y<board_height*2;++y){
            if(y%2==0){
                board_maze[y,0]=Instantiate(p_crossing,blc+new Vector3(0,_offset_y,_offset_z*y),Quaternion.identity);
                board_maze[y,0].transform.SetParent(parent.transform);
            }
            else{
                board_maze[y,0]=Instantiate(p_border,blc+new Vector3(0,_offset_y,_offset_z*y),Quaternion.identity);
                board_maze[y,0].transform.SetParent(parent.transform);
            }
            /*for(int x=1;x<board_width*2;++x){
                if(y%2==0){
                    if(x%2==0){
                        board_maze[y,x]=Instantiate(p_crossing,blc+new Vector3(_offset_x*x,_offset_y,_offset_z*y),Quaternion.identity);
                        board_maze[y,x].transform.SetParent(parent.transform);
                    }
                    else{
                        board_maze[y,x]=Instantiate(p_border,blc+new Vector3(_offset_x*x,_offset_y,_offset_z*y),Quaternion.identity);
                        board_maze[y,x].transform.SetParent(parent.transform);
                        board_maze[y,x].transform.Rotate(0f,90f,0f);
                    }
                }
                else{
                    if(x%2==0 && wh[y/2,x/2]){
                        board_maze[y,x]=Instantiate(p_border,blc+new Vector3(_offset_x*x,_offset_y,_offset_z*y),Quaternion.identity);
                        board_maze[y,x].transform.SetParent(parent.transform);
                    }
                    else{
                        board_maze[y,x]=Instantiate(p_center,blc+new Vector3(_offset_x*x,_offset_y,_offset_z*y),Quaternion.identity);
                        board_maze[y,x].transform.SetParent(parent.transform);
                    }
                }
            }*/
            if(y%2==0){
                board_maze[y,0]=Instantiate(p_crossing,blc+new Vector3(_offset_x*board_width*2,_offset_y,_offset_z*y),Quaternion.identity);
                board_maze[y,0].transform.SetParent(parent.transform);
            }
            else{
                board_maze[y,0]=Instantiate(p_border,blc+new Vector3(_offset_x*board_width*2,_offset_y,_offset_z*y),Quaternion.identity);
                board_maze[y,0].transform.SetParent(parent.transform);
            }
        }
        for(int x=0;x<board_width*2+1;++x){
                if(x%2==0){
                    board_maze[board_height*2,x]=Instantiate(p_crossing,blc+new Vector3(_offset_x*x,_offset_y,_offset_z*(board_height*2)),Quaternion.identity);
                    board_maze[board_height*2,x].transform.SetParent(parent.transform);
                }
                else{
                    board_maze[board_height*2,x]=Instantiate(p_border,blc+new Vector3(_offset_x*x,_offset_y,_offset_z*(board_height*2)),Quaternion.identity);
                    board_maze[board_height*2,x].transform.SetParent(parent.transform);
                    board_maze[board_height*2,x].transform.Rotate(0f,90f,0f);
                }
        }
    }
}
