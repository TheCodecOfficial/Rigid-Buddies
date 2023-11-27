using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    public float radius;
    public float length;
    public float restAngle; //in degrees
    public float maxRotation; //in degrees
    public float angularVelocity; //in degrees per second
    
     float rotationSign;
    ///[HideInInspector] public float rotationSign;

    // changing
    public float rotation { get { return transform.localRotation.eulerAngles.z; } set { transform.localRotation = Quaternion.Euler(new Vector3(0,0,value)); } }
    public Vector2 pos { get { return transform.position; } set { transform.position = value; } }
    public float currentAngularVelocity = 0f;
    //public float touchIdentifier = -1;
    public bool isPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        //print(gameObject.name + restAngle);
        maxRotation = Mathf.Abs(maxRotation);
        rotationSign = Mathf.Sign(angularVelocity);
        //rotation = restAngle;
        //restAngle = rotation;
    }


    public void Simulate() 
    {
        float prevRotation = rotation;
        //bool pressed = touchIdentifier >= 0;
        if (isPressed) 
        {//rotatate with angular velocity until max rotation
            //rotation = Mathf.MoveTowards(rotation, maxRotation, rotationSign*angularVelocity * Time.deltaTime); 
            
            if (Mathf.Abs(maxRotation - rotation) > Math.Abs(angularVelocity) * Time.deltaTime)
            {
                rotation = rotation +  angularVelocity * Time.deltaTime;
            }


            //rotation = Mathf.Min(rotation + Time.deltaTime * angularVelocity, maxRotation);
            //rotation = Mathf.Max(rotation + Time.deltaTime * angularVelocity, maxRotation);
        }
        else 
            //rotation = Mathf.MoveTowards(rotation, restAngle, angularVelocity * Time.deltaTime);
            if (Mathf.Abs(restAngle - rotation) > Math.Abs(angularVelocity) * Time.deltaTime)
            {
                rotation = rotation -  angularVelocity * Time.deltaTime;
            }
            //rotation = Mathf.Min(rotation - Time.deltaTime * angularVelocity, restAngle);
            //rotation = Mathf.Max(rotation - Time.deltaTime * angularVelocity, 0f);
        print(rotation);
        
        currentAngularVelocity = (rotation - prevRotation) / Time.deltaTime;

        //currentAngularVelocity = rotationSign * (rotation - prevRotation) / Time.deltaTime;
        //print(rotation);
    }

    // todo ?
    public bool select(Vector2 _pos) {
        Vector2 d = pos - _pos;
        return d.magnitude < length;
    }
    
    public Vector2 getTip() 
    {
        float angle = rotation;//restAngle + rotationSign * rotation;
        Vector2 dir = new Vector2(Mathf.Cos(angle* (Mathf.PI / 180)), Mathf.Sin(angle* (Mathf.PI / 180)));
        Vector2 tip = pos + dir * length;

        return tip;
    }

}