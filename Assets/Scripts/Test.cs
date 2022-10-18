using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject p1;
    public GameObject p2;
    public int list_size;
    public int w,h;
    private GameObject[] _block_list;
    private int[,] _maze_base_cells;
    private int[,] _maze_base_walls_h;
    private int[,] _maze_base_walls_v;
    private List<Vector3Int> _wall_list = new List<Vector3Int>();
    
    private int counter=0;
    void Start()
    {
        _block_list = new GameObject[list_size];
        //_maze_base_cells = new int[h,w];
        //_maze_base_walls_h = new int[h,w-1];
        //_maze_base_walls_v = new int[h-1,w];
        /*_maze_base_cells.Fill(0);
        _maze_base_walls_h.Fill(1);
        _maze_base_walls_v.Fill(1);
        
        for(int i=0;i<h-1;++i){
            for(int j=0;j<w-1;++j){
                _maze_base_cells[i,j]=0;
                _maze_base_walls_h[i,j]=1;
                _maze_base_walls_v[i,j]=1;
            }
        }
        for(int i=0;i<h-1;++i){
            _maze_base_cells[i,w-1]=0;
            _maze_base_walls_v[i,w-1]=1;
        }
        for(int i=0;i<w-1;++i){
            _maze_base_cells[h-1,i]=0;
            _maze_base_walls_h[h-1,i]=1;    
        }
        _maze_base_cells[h-1,w-1]=0;
        for(int i=0;i<h;++i){
            for(int j=0;j<w-1;++j){
                _maze_base_walls_h[i,j]=1;
            }
        }
        for(int i=0;i<h-1;++i){
            for(int j=0;j<w;++j){
                _maze_base_walls_v[i,j]=1;
            }
        }*/
        //Array.Fill<int>(_maze_base_cells,0,w*h,0);
        //Array.Fill<int>(_maze_base_walls_h,1);
        //Array.Fill<int>(_maze_base_walls_v,1);

    }

    // Update is called once per frame
    void Update()
    {
        if(counter<list_size){
            _block_list[counter]=Instantiate(p1,p1.transform.position+new Vector3(1f*counter,1f*counter,1f*counter),Quaternion.identity);
            counter+=1;
        }
    }

    void GenerateMaze(){
        InitMaze(h,w,_maze_base_cells,_maze_base_walls_h,_maze_base_walls_v);
        bool valid_wall = false;
        Vector3Int buff;
        int idx,curx=0,cury=0;
        System.Random rnd = new System.Random();
        GetCellWalls(0,0,_wall_list);
        while(_wall_list.Count>0){
            while(!valid_wall){
                idx = rnd.Next(0,_wall_list.Count);
                buff = _wall_list[idx];
                curx=buff.x;
                cury=buff.y;
                _wall_list.RemoveAt(idx);
                if(buff.z==0){
                    if(_maze_base_cells[buff.x,buff.y]==0){
                        curx=buff.x;
                        cury=buff.y;
                        valid_wall=true;
                    }
                    else {
                        if(_maze_base_cells[buff.x,buff.y+1]==0){
                            curx=buff.x;
                            cury=buff.y+1;
                            valid_wall=true;
                        }
                        else {
                            continue;
                        }
                    }
                    _maze_base_walls_h[buff.x,buff.y]=0;
                }
                else{
                    if(_maze_base_cells[buff.x,buff.y]==0){
                        curx=buff.x;
                        cury=buff.y;
                        valid_wall=true;
                    }
                    else {
                        if(_maze_base_cells[buff.x+1,buff.y]==0){
                            curx=buff.x+1;
                            cury=buff.y;
                            valid_wall=true;
                        }
                        else {
                            continue;
                        }
                    }
                    _maze_base_walls_v[buff.x,buff.y]=0;
                }
            
            }
            valid_wall=false;
            GetCellWalls(curx,cury,_wall_list);
            _maze_base_cells[curx,cury]=1;
        }
    }
    
    void InitMaze(int h,int w, int[,] m_c,int[,] m_w_h,int[,] m_w_v){ 
        m_c = new int[h,w];
        m_w_h = new int[h,w-1];
        m_w_v = new int[h-1,w];
        for(int i=0;i<h;++i){
            for(int j=0;j<w-1;++j){
                m_w_h[i,j]=1;
            }
        }
        for(int i=0;i<h-1;++i){
            for(int j=0;j<w;++j){
                m_w_v[i,j]=1;
            }
        }
    }

    //z = 0 - horizontal wall, z=1 - vertical wall
    void GetCellWalls(int x,int y,List<Vector3Int> lst){
        if(x>0 && _maze_base_cells[x-1,y]==0){
            lst.Add(new Vector3Int(x-1,y,0));
        }
        if(x<w-1 && _maze_base_cells[x+1,y]==0){
            lst.Add(new Vector3Int(x,y,0));
        }
        if(y>0 && _maze_base_cells[x,y-1]==0){
            lst.Add(new Vector3Int(x,y-1,1));
        }
        if(y<h-1 && _maze_base_cells[x,y+1]==0){
            lst.Add(new Vector3Int(x,y,1));
        }
    }
}
