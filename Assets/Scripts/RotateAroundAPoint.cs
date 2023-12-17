using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;

public class RotateAroundAPoint : MonoBehaviour
{
    //float rotationForce = 0.5f;
    public float rotationSpeed = 0.5f;
    //public MyRigidbody myRigidbody;
    public MyRigidbody[] myRigidbodies;
    bool[] firstUpdateAfterClick;
    bool[] firstUpdateAfterRelease;
    public bool isPressed = false;
    public float maxRotation; //in degrees
    float restAngle; //in degrees



    void Start(){
        firstUpdateAfterClick = new bool[myRigidbodies.Length];
        firstUpdateAfterRelease = new bool[myRigidbodies.Length];
        for(int i = 0; i < myRigidbodies.Length; i++){
            firstUpdateAfterClick[i] = true;
            firstUpdateAfterRelease[i] = true;
        }
        maxRotation = Mathf.Abs(maxRotation); //rotation in rad
        restAngle = myRigidbodies[0].transform.localRotation.eulerAngles.z;
    }
    
    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < myRigidbodies.Length; i++)
        {
            MyRigidbody myRigidbody = myRigidbodies[i];

        
            Vector2 centerPoint = transform.position;
            Vector2 direction = centerPoint - (Vector2)myRigidbody.transform.position;
            if(isPressed){
                if (Mathf.Abs(maxRotation - myRigidbodies[0].transform.localRotation.eulerAngles.z) > 15)
                {
                    if (firstUpdateAfterClick[i])
                    {
                        
                        myRigidbody.StopMovement();
                        //set initial velocity
                        firstUpdateAfterClick[i] = false;
                        firstUpdateAfterRelease[i] = true;
                        Vector2 perpendicular = Vector2.Perpendicular(direction);
                        Vector2 impulse = perpendicular.normalized * rotationSpeed * direction.magnitude;
                        myRigidbody.AddImpulse(impulse, (Vector2)myRigidbody.transform.position);

                        //Set angular velocity
                        SetAngularVelocity(myRigidbody, rotationSpeed, -1);
                    }
                    else{
                        //translate
                        Vector2 force = direction.normalized * rotationSpeed*rotationSpeed  * direction.magnitude * myRigidbody.GetMass() ;
                        myRigidbody.AddForce(force, (Vector2)myRigidbody.transform.position);

                        

                    }
                }else{
                    
                    myRigidbody.StopMovement();
                }
            }
            else{
                if (Mathf.Abs(restAngle - myRigidbodies[0].transform.localRotation.eulerAngles.z) > 15)
                {
                    //myRigidbody.StopMovement();
                    firstUpdateAfterClick[i] = true;
                    if (firstUpdateAfterRelease[i])
                    {
                        myRigidbody.StopMovement();
                        firstUpdateAfterRelease[i] = false;
                        Vector2 perpendicular = Vector2.Perpendicular(direction);
                        Vector2 impulse = -perpendicular.normalized * rotationSpeed * direction.magnitude;
                        myRigidbody.AddImpulse(impulse, (Vector2)myRigidbody.transform.position);

                        //rotate
                        SetAngularVelocity(myRigidbody, rotationSpeed, 1);
                    }
                    else{
                        Vector2 force = direction.normalized * rotationSpeed*rotationSpeed  * direction.magnitude * myRigidbody.GetMass() ;
                        myRigidbody.AddForce(force, (Vector2)myRigidbody.transform.position);
                    }
                }
                else{
                    myRigidbody.StopMovement();
                }


            }
        }
    }
    private void SetAngularVelocity(MyRigidbody myRigidbody, float angularVelocity, int rotationDirection = -1){
        float distanceToCenter = 2f;
        Vector2 impulseforRotation = -myRigidbody.transform.up * rotationSpeed * myRigidbody.GetMomentOfInertia()/ distanceToCenter/2;
        Vector2 applicationPointRight = distanceToCenter*(Vector2) myRigidbody.transform.right + (Vector2)myRigidbody.transform.position;
        Vector2 applicationPointLeft = -distanceToCenter*(Vector2) myRigidbody.transform.right + (Vector2)myRigidbody.transform.position;
        myRigidbody.AddImpulse(rotationDirection*-impulseforRotation, applicationPointRight);
        myRigidbody.AddImpulse(rotationDirection*impulseforRotation, applicationPointLeft);
    }

}
