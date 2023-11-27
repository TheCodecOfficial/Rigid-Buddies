using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    public float radius;
    public float length;
    float restAngle; //in degrees
    public float maxRotation; //in degrees
    public float angularVelocity; //in degrees per second
    
    public float rotation { get { return transform.localRotation.eulerAngles.z; } set { transform.localRotation = Quaternion.Euler(new Vector3(0,0,value)); } }
    public Vector2 pos { get { return transform.position; } set { transform.position = value; } }
    public float currentAngularVelocity = 0f;
    public bool isPressed = false;
    public float restitution = 1;

    // Start is called before the first frame update
    void Start()
    {
        maxRotation = Mathf.Abs(maxRotation);
        restAngle = rotation;
    }


    public void Simulate() 
    {
        float prevRotation = rotation;
        if (isPressed) 
        {
            if (Mathf.Abs(maxRotation - rotation) > Math.Abs(angularVelocity) * Time.deltaTime)
            {
                rotation = rotation +  angularVelocity * Time.deltaTime;
            }

        }
        else 
            if (Mathf.Abs(restAngle - rotation) > Math.Abs(angularVelocity) * Time.deltaTime)
            {
                rotation = rotation -  angularVelocity * Time.deltaTime;
            }

        
        currentAngularVelocity = (rotation - prevRotation) / Time.deltaTime;

    }

    public Vector2 getTip() 
    {
        float angle = rotation;
        Vector2 dir = new Vector2(Mathf.Cos(angle* (Mathf.PI / 180)), Mathf.Sin(angle* (Mathf.PI / 180)));
        Vector2 tip = pos + dir * length;

        return tip;
    }

}