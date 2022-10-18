using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Maze{
    public int[,] m_cells;
    public int[,] m_walls_h;
    public int[,] m_walls_v;
    
    public int w,h;
    public int entrance_w,entrance_h;

    public Maze(int h,int w,int eh,int ew){
        this.h = h;
        this.w = w;
        this.entrance_h=eh;
        this.entrance_w=ew;
        m_cells = new int[h,w];
        m_walls_h = new int[h,w-1];
        m_walls_v = new int[h-1,w];
    }

    public void GenerateMaze(){
        InitMaze();
        bool valid_wall = false;
        List<Vector3Int> _wall_list = new List<Vector3Int>();
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
                    if(m_cells[buff.x,buff.y]==0){
                        curx=buff.x;
                        cury=buff.y;
                        valid_wall=true;
                    }
                    else {
                        if(m_cells[buff.x,buff.y+1]==0){
                            curx=buff.x;
                            cury=buff.y+1;
                            valid_wall=true;
                        }
                        else {
                            continue;
                        }
                    }
                    m_walls_h[buff.x,buff.y]=0;
                }
                else{
                    if(m_cells[buff.x,buff.y]==0){
                        curx=buff.x;
                        cury=buff.y;
                        valid_wall=true;
                    }
                    else {
                        if(m_cells[buff.x+1,buff.y]==0){
                            curx=buff.x+1;
                            cury=buff.y;
                            valid_wall=true;
                        }
                        else {
                            continue;
                        }
                    }
                    m_walls_v[buff.x,buff.y]=0;
                }
            
            }
            valid_wall=false;
            GetCellWalls(curx,cury,_wall_list);
            m_cells[curx,cury]=1;
        }
    }

    private void InitMaze(){
        for(int i=0;i<h;++i){
            for(int j=0;j<w;++j){
                m_cells[i,j]=0;
            }
        }
        for(int i=0;i<h;++i){
            for(int j=0;j<w-1;++j){
                m_walls_h[i,j]=1;
            }
        }
        for(int i=0;i<h-1;++i){
            for(int j=0;j<w;++j){
                m_walls_v[i,j]=1;
            }
        }
    }

    private void GetCellWalls(int x,int y,List<Vector3Int> lst){
        if(x>0 && m_cells[x-1,y]==0){
            lst.Add(new Vector3Int(x-1,y,0));
        }
        if(x<w-1 && m_cells[x+1,y]==0){
            lst.Add(new Vector3Int(x,y,0));
        }
        if(y>0 && m_cells[x,y-1]==0){
            lst.Add(new Vector3Int(x,y-1,1));
        }
        if(y<h-1 && m_cells[x,y+1]==0){
            lst.Add(new Vector3Int(x,y,1));
        }
    }
}