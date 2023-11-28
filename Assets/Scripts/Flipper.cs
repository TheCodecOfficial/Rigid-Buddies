using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    float restAngle; //in degrees
    public float maxRotation; //in degrees
    public bool isPressed = false;


    public MyRigidbody myRigidbody;
    public MyCapsuleCollider myCapsuleCollider;


    // Start is called before the first frame update
    void Start()
    {

        myCapsuleCollider = this.GetComponent<MyCapsuleCollider>();
        myCapsuleCollider.SetRadius(transform.localScale.x / 2);
        myCapsuleCollider.SetLength(transform.localScale.y - myCapsuleCollider.GetRadius());
    
        maxRotation = Mathf.Abs(maxRotation);
        restAngle = myCapsuleCollider.rotation;
    }


    public void Simulate() 
    {
        float prevRotation = myCapsuleCollider.rotation;
        if (isPressed) 
        {
            if (Mathf.Abs(maxRotation - myCapsuleCollider.rotation) > Math.Abs(myCapsuleCollider.angularVelocity) * Time.deltaTime)
            {
                myCapsuleCollider.rotation = myCapsuleCollider.rotation +  myCapsuleCollider.angularVelocity * Time.deltaTime;
            }

        }
        else 
            if (Mathf.Abs(restAngle - myCapsuleCollider.rotation) > Math.Abs(myCapsuleCollider.angularVelocity) * Time.deltaTime)
            {
                myCapsuleCollider.rotation = myCapsuleCollider.rotation -  myCapsuleCollider.angularVelocity * Time.deltaTime;
            }

        
        myCapsuleCollider.angularVelocity = (myCapsuleCollider.rotation - prevRotation) / Time.deltaTime;

    }

}