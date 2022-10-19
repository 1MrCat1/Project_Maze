using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToRotate;
    public float rotationSpeedX = 0.5f;
    public float rotationSpeedY = 0.5f;
    public float rotationSpeedZ = 0.5f;
    public float rotationAccelerationMax = 5;
    public float rotationAccelerationMin = 1;
    public float rotationAccelerationFrames = 60;
    private float rotationAccelerationCurrentX = 1;
    private float rotationAccelerationCurrentY = 1;
    private float rotationAccelerationCurrentZ = 1;
    private float rotationKeyXPrev=0;
    private float rotationKeyYPrev=0;
    private float rotationKeyZPrev=0;
    private float rotationKeyX;
    private float rotationKeyY;
    private float rotationKeyZ;
    void Start()
    {
        rotationAccelerationCurrentX = rotationAccelerationMin;
        rotationAccelerationCurrentY = rotationAccelerationMin;
        rotationAccelerationCurrentZ = rotationAccelerationMin;
    }

    // Update is called once per frame
    void Update()
    {
        rotationKeyX = Input.GetAxis("Vertical");
        rotationKeyZ = Input.GetAxis("Horizontal");
        rotationKeyY = Input.GetAxis("Horizontal_2");


        rotationAccelerationCurrentX = UpdateAcceleration(rotationAccelerationCurrentX,rotationKeyX,rotationKeyXPrev);
        rotationAccelerationCurrentY = UpdateAcceleration(rotationAccelerationCurrentY,rotationKeyY,rotationKeyYPrev);
        rotationAccelerationCurrentZ = UpdateAcceleration(rotationAccelerationCurrentZ,rotationKeyZ,rotationKeyZPrev);
        /*if(Math.Abs(rotationKeyX)>=Math.Abs(rotationKeyXPrev)){
            rotationAccelerationCurrentX=Math.Min(
                rotationAccelerationCurrentX+(rotationAccelerationMax-rotationAccelerationMin)/rotationAccelerationFrames,
                rotationAccelerationMax);
        }
        else{
            rotationAccelerationCurrentX=rotationAccelerationMin;
        }
        if(Math.Abs(rotationKeyY)>=Math.Abs(rotationKeyYPrev)){
            rotationAccelerationCurrentY=Math.Min(
                rotationAccelerationCurrentY+(rotationAccelerationMax-rotationAccelerationMin)/rotationAccelerationFrames,
                rotationAccelerationMax);
        }
        else{
            rotationAccelerationCurrentY=rotationAccelerationMin;
        }
        if(Math.Abs(rotationKeyZ)>=Math.Abs(rotationKeyZPrev)){
            rotationAccelerationCurrentZ=Math.Min(
                rotationAccelerationCurrentZ+(rotationAccelerationMax-rotationAccelerationMin)/rotationAccelerationFrames,
                rotationAccelerationMax);
        }
        else{
            rotationAccelerationCurrentZ=rotationAccelerationMin;
        }*/
        objectToRotate.transform.Rotate(
            rotationSpeedX*rotationKeyX*rotationAccelerationCurrentX,
            rotationKeyY*rotationSpeedY*rotationAccelerationCurrentY,
            -rotationSpeedZ*rotationKeyZ*rotationAccelerationCurrentZ,
            Space.Self);
        /*if (Input.GetKeyDown("w"))
        {
            Debug.Log("w key was pressed");
        }*/
        rotationKeyXPrev=rotationKeyX;
        rotationKeyYPrev=rotationKeyY;
        rotationKeyZPrev=rotationKeyZ;
    }

    float UpdateAcceleration(float accCurrent,float keyCurrent,float keyPrev){
    if(Math.Abs(keyCurrent)>=Math.Abs(keyPrev)){
        return Math.Min(
            accCurrent+(rotationAccelerationMax-rotationAccelerationMin)/rotationAccelerationFrames,
            rotationAccelerationMax
            );
    }
    else{
        return rotationAccelerationMin;
    }
}
}

