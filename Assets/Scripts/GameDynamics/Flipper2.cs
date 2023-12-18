using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Flipper2 : MonoBehaviour
{
    public MyRigidbody[] myRigidbodies;
    //public MyRigidbody myRigidbody;
    public float rotationForce = 10000f;




    float restAngle; //in degrees
    public float maxRotation; //in degrees
    public float angularVelocity; //in degrees per second
    
    public float rotation { get { return transform.localRotation.eulerAngles.z; } }
    public Vector2 pos { get { return transform.position; } set { transform.position = value; } }
    public float currentAngularVelocity = 0f;
    public bool isPressed = false;
    //public float pushVel = 1;

    // Start is called before the first frame update
    void Start()
    {
        // maxRotation to radians


        maxRotation = Mathf.Abs(maxRotation); //rotation in rad
        restAngle = rotation;
    }
    private float Cross2D(Vector2 a, Vector2 b)
    {
        return (a.x * b.y) - (a.y * b.x); 
    }
    public void Update(){
        //print("vectoup" + (Vector2) myRigidbody.transform.up);
        //print("transfor" + (Vector2)myRigidbody.transform.position);
        //print("always " +(2f*(Vector2) myRigidbody.transform.up + (Vector2)myRigidbody.transform.position) );

        //print("always " +(Vector2)myRigidbody.transform.position);

        //float prevRotation = rotation;
        //print(maxRotation + " " + rotation);
        if (isPressed) 
        {
            
            if (Mathf.Abs(maxRotation - rotation) > 15)
            {
                
                Vector2 impulse = rotationForce * (Vector2)transform.up;
                Vector2 applicationPoint = 2f*(Vector2) transform.right + (Vector2)transform.position;
                foreach (MyRigidbody myRigidbody in myRigidbodies)
                {
                    myRigidbody.AddForce(impulse, applicationPoint);
                }
                
                //Vector2 radius = new Vector3(0, 0.5f, 0) - this.transform.position;
                //float angularVelocity1 =  Cross2D(radius, new Vector2(0, rotationForce)) / 5.488557f;
                //print("clicked " + angularVelocity1 + "application point " + applicationPoint );
            }
            else{
                foreach (MyRigidbody myRigidbody in myRigidbodies)
                {
                    myRigidbody.StopMovement();
                }
            }
            
        }else
        {
            //myRigidbody.StopMovement();
            if (Mathf.Abs(restAngle - rotation) > 15)
            {
                Vector2 impulse = -rotationForce * (Vector2)transform.up;
                Vector2 applicationPoint = 2f*(Vector2) transform.right + (Vector2)transform.position;

                //Vector2 applicationPoint = myRigidbody.transform.position + myRigidbody.transform.up* -0.5f;
                foreach (MyRigidbody myRigidbody in myRigidbodies)
                {
                    myRigidbody.AddForce(impulse, applicationPoint);
                }

                
                //Vector2 radius = new Vector3(0, 0.5f, 0) - this.transform.position;
                //float angularVelocity2 =  Cross2D(radius, new Vector2(0, rotationForce)) / 5.488557f;
                //print("released " + angularVelocity2 +"application point " + myRigidbody.transform.up);
            }
            else{
                
                foreach (MyRigidbody myRigidbody in myRigidbodies){
                    myRigidbody.StopMovement();
                }
            }
            //todo force in the opposite direction
        }
    }
    /*
    public void Update() 
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
    */

}