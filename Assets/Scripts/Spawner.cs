using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject pref_center;
    public GameObject pref_border;
    public GameObject pref_crossing;
    public GameObject parent;
    // Start is called before the first frame update
    public Vector3 position;
    public int board_height;
    public int board_width;

    private GameObject[] _board_list;
    private float _offset_x;
    private float _offset_z;
    private GameObject tmp;
    private bool flag = true;
    void Start()
    {
        var scale_crossing = pref_crossing.transform.localScale;   
        var scale_center = pref_center.transform.localScale;
        Debug.Log("Scales. Center:"+scale_center+"\nCrossing: "+scale_crossing);
        //Ищем не позицию углового перекрёстка, а углового куба
        float full_width = (scale_center.x+scale_crossing.x)*board_width - scale_center.x;
        float full_height = (scale_center.z+scale_crossing.z)*board_height - scale_center.z;
        Debug.Log("Full dimensions.\nFull_width:"+full_width+"\nFull_height:"+full_height);
        var parent_center = parent.transform.position;
        Debug.Log("Parent position:"+parent_center);
        position.x=parent_center.x-full_width/2f;
        position.z=parent_center.z-full_height/2f;
        position.y=parent_center.y;
        
        _offset_x = (scale_center.x/2f)+(scale_crossing.x/2f);
        _offset_z = (scale_center.z/2f)+(scale_crossing.z/2f);
        Debug.Log("Current position: "+position);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("h")&& flag){
            CreateBoard();
            flag = false;
        }
    }

    void CreateCell(Vector3 pos,int sides){
        tmp = Instantiate(pref_center,pos,Quaternion.identity);
        tmp.transform.SetParent(parent.transform);
        tmp = Instantiate(pref_border,new Vector3(pos.x-_offset_x,pos.y,pos.z),Quaternion.identity);
        tmp.transform.SetParent(parent.transform);
        tmp = Instantiate(pref_border,new Vector3(pos.x,pos.y,pos.z-_offset_z),Quaternion.identity);
        tmp.transform.SetParent(parent.transform);
        tmp.transform.Rotate(0f,90f,0f);
        tmp = Instantiate(pref_crossing,new Vector3(pos.x-_offset_x,pos.y,pos.z-_offset_z),Quaternion.identity);
        tmp.transform.SetParent(parent.transform);
        if((sides&1)>0){
            tmp = Instantiate(pref_border,new Vector3(pos.x+_offset_x,pos.y,pos.z),Quaternion.identity);
            tmp.transform.SetParent(parent.transform);
            tmp = Instantiate(pref_crossing,new Vector3(pos.x+_offset_x,pos.y,pos.z-_offset_z),Quaternion.identity);
            tmp.transform.SetParent(parent.transform);    
        }
        if((sides&2)>0){
            tmp = Instantiate(pref_border,new Vector3(pos.x,pos.y,pos.z+_offset_z),Quaternion.identity);
            tmp.transform.SetParent(parent.transform);
            tmp.transform.Rotate(0f,90f,0f);
            tmp = Instantiate(pref_crossing,new Vector3(pos.x-_offset_x,pos.y,pos.z+_offset_z),Quaternion.identity);
            tmp.transform.SetParent(parent.transform);
        }
        if((sides&4)>0){
            tmp = Instantiate(pref_crossing,new Vector3(pos.x+_offset_x,pos.y,pos.z+_offset_z),Quaternion.identity);
            tmp.transform.SetParent(parent.transform);    
        }
    }
    
    void CreateBoard(){
        Vector3 current_cell_position = position;
        for(int i=0;i<board_height;++i){
            for (int j = 0; j < board_width-1; ++j)
            {
                CreateCell(current_cell_position,(i==(board_height-1)?2:0));
                current_cell_position.x+=_offset_x*2;
                //tmp = Instantiate(pref_border,new Vector3(current_cell_position.x-_offset_x,current_cell_position.y+1,current_cell_position.z),Quaternion.identity);
                //tmp.transform.SetParent(parent.transform);
            }
            CreateCell(current_cell_position,(i==(board_height-1)?4+2+1:1));
            current_cell_position.x = position.x;
            current_cell_position.z += _offset_z*2;
        }
    }
}
