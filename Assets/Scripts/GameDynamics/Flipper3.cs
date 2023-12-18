using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;

public class Flipper3 : MonoBehaviour
{
    //float rotationForce = 0.5f;
    public float rotationSpeed = 0.5f;
    //public MyRigidbody myRigidbody;
    public MyRigidbody centerRigidbody;
    public MyRigidbody[] additionalFlipperComponents;

    bool firstUpdateAfterClick = true;
    bool firstUpdateAfterRelease = true;
    public bool isPressed = false;
    public float maxRotation; //in degrees
    float restAngle; //in degrees



    void Start(){

        maxRotation = Mathf.Abs(maxRotation); //rotation in rad
        restAngle = transform.localRotation.eulerAngles.z;

        
    }
    
    // Update is called once per frame
    void Update()
    {
 


        if(isPressed){
            if (Mathf.Abs(maxRotation - centerRigidbody.transform.localRotation.eulerAngles.z) > 10)
            {
                if (firstUpdateAfterClick)
                {
                    centerRigidbody.StopMovement();
                    firstUpdateAfterClick = false;
                    firstUpdateAfterRelease = true;
                    //Set angular velocity
                    Debug.Log("rotation speed: " + -rotationSpeed);
                    centerRigidbody.SetAngularVelocity(-rotationSpeed);

                    //SetAngularVelocity(centerRigidbody, rotationSpeed, -1);
                }

            }else{
                
                centerRigidbody.StopMovement();
            }
            
        }
        else{
            if (Mathf.Abs(restAngle - centerRigidbody.transform.localRotation.eulerAngles.z) > 10)
            {
                if (firstUpdateAfterRelease)
                {
                    centerRigidbody.StopMovement();
                    firstUpdateAfterRelease = false;
                    firstUpdateAfterClick = true;
                    //Set angular velocity
                    //SetAngularVelocity(centerRigidbody, rotationSpeed, 1);
                    centerRigidbody.SetAngularVelocity(rotationSpeed);
                }
            }
            else{
                centerRigidbody.StopMovement();
            }
            
        }

        foreach (MyRigidbody myRigidbodyadditional in additionalFlipperComponents)
        {
            myRigidbodyadditional.SetAngularVelocity(centerRigidbody.angularVelocity);
            Vector2 PosAddObj = new Vector2(myRigidbodyadditional.transform.position.x, myRigidbodyadditional.transform.position.y);
            myRigidbodyadditional.SetVelocity(centerRigidbody.PointVelocity(PosAddObj));

        }
    }
    
    private void SetAngularVelocity(MyRigidbody myRigidbody, float angularVelocity, int rotationDirection = -1){
        float distanceToCenter = 2f;
        Vector2 impulseforRotation = -myRigidbody.transform.up * angularVelocity * myRigidbody.GetMomentOfInertia()/ distanceToCenter/2;
        Vector2 applicationPointRight = distanceToCenter*(Vector2) myRigidbody.transform.right + (Vector2)myRigidbody.transform.position;
        Vector2 applicationPointLeft = -distanceToCenter*(Vector2) myRigidbody.transform.right + (Vector2)myRigidbody.transform.position;
        myRigidbody.AddImpulse(rotationDirection*-impulseforRotation, applicationPointRight);
        myRigidbody.AddImpulse(rotationDirection*impulseforRotation, applicationPointLeft);
    }

}
