using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

// This class handles the movement of the flippers.
// The flippers are composed of a center rigidbody (sphere collider) and two additional
// components: A square box collider and a sphere collider.
public class Flipper : MonoBehaviour
{
    public float rotationSpeed; // in rad/s
    public MyRigidbody centerRigidbody; // the rigidbody of the center of the flipper
    public MyRigidbody[] additionalFlipperComponents; // the rigidbodies of the additional components of the flipper
    public bool isPressed = false; // indicates if the flipper is pressed (from the user input)
    public float maxRotation; //in degrees
    float restAngle; //in degrees

    bool firstUpdateAfterClick = true;
    bool firstUpdateAfterRelease = true;
    
    void Start(){
        maxRotation = Mathf.Abs(maxRotation);
        restAngle = transform.localRotation.eulerAngles.z;
    }
    
    void Update()
    {
        if(isPressed){
            // If the flipper is pressed, rotate it until it reaches the max rotation.
            if (Mathf.Abs(maxRotation - centerRigidbody.transform.localRotation.eulerAngles.z) > 10)
            {
                // If this is the first update after the flipper was pressed, stop the movement of the flipper
                // and set the corresponding angular velocity.
                if (firstUpdateAfterClick)
                {
                    centerRigidbody.StopMovement();
                    firstUpdateAfterClick = false;
                    firstUpdateAfterRelease = true;
                    //Set angular velocity
                    Debug.Log("rotation speed: " + -rotationSpeed);
                    centerRigidbody.SetAngularVelocity(-rotationSpeed);
                }
            }else{
                centerRigidbody.StopMovement();
            }
            
        }
        else{
            // If the flipper is not pressed, rotate it until it reaches the rest angle.
            if (Mathf.Abs(restAngle - centerRigidbody.transform.localRotation.eulerAngles.z) > 10)
            {
                // If this is the first update after the flipper was released, stop the movement of the flipper
                // and set the corresponding angular velocity.
                if (firstUpdateAfterRelease)
                {
                    centerRigidbody.StopMovement();
                    firstUpdateAfterRelease = false;
                    firstUpdateAfterClick = true;
                    centerRigidbody.SetAngularVelocity(rotationSpeed);
                }
            }
            else{
                centerRigidbody.StopMovement();
            }
        }
        // Set the angular velocity and linear velocity of the additional components of the flipper.
        foreach (MyRigidbody myRigidbodyadditional in additionalFlipperComponents)
        {
            myRigidbodyadditional.SetAngularVelocity(centerRigidbody.angularVelocity);
            Vector2 PosAddObj = new Vector2(myRigidbodyadditional.transform.position.x, myRigidbodyadditional.transform.position.y);
            myRigidbodyadditional.SetVelocity(centerRigidbody.PointVelocity(PosAddObj));
        }
    }
}
