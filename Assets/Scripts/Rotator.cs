using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float rotationSpeedX = 0.5f;
    public float rotationSpeedY = 0.5f;
    public float rotationSpeedZ = 0.5f;
    private float rotationKeyX;
    private float rotationKeyY;
    private float rotationKeyZ;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotationKeyX = Input.GetAxis("Vertical");
        rotationKeyZ = Input.GetAxis("Horizontal");
        rotationKeyY = Input.GetAxis("Horizontal_2");
        if(Math.Abs(rotationKeyX)>0.001){
            //Debug.Log("X: " + rotationKeyX);
        }
        if(Math.Abs(rotationKeyX)>0.001){
            //Debug.Log("Y: " + rotationKeyX);
        }
        if(Math.Abs(rotationKeyZ)>0.001){
            //Debug.Log("Z: " + rotationKeyZ);
        }
        transform.Rotate(rotationSpeedX*rotationKeyX,rotationKeyY*rotationSpeedY,-rotationSpeedZ*rotationKeyZ,Space.Self);
        if (Input.GetKeyDown("w"))
        {
            Debug.Log("w key was pressed");
        }
    }
}
