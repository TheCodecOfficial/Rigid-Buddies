using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{


    public Flipper2 flipperRight;
    public Flipper2 flipperLeft;

    public RotateAroundAPoint rotateAroundAPointRight;
    public RotateAroundAPoint rotateAroundAPointLeft;
    //public delegate void MKeyPressedHandler();
    //public event MKeyPressedHandler MKeyPressed;
    //public event MKeyPressedHandler XKeyPressed;
    //dictionary of events

    // Start is called before the first frame update
    void Start()
    {
        //MKeyPressed += onMpressed;
        //XKeyPressed += onXpressed;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey(KeyCode.M)) //Returns true while the user holds down the key M.
        {
            MKeyPressed?.Invoke();
        }*/

        // Right Flipper
        if (Input.GetKeyDown(KeyCode.M))
        {
            flipperRight.isPressed = true;
            rotateAroundAPointRight.isPressed = true;
            //print("right flipper pressed");
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            flipperRight.isPressed = false;
            rotateAroundAPointRight.isPressed = false;  
            //print("right flipper released");
        }

        
        // Left Flipper
        if (Input.GetKeyDown(KeyCode.X))
        {
            flipperLeft.isPressed = true;
            rotateAroundAPointLeft.isPressed = true;
            //print("left flipper pressed");
        }
        
        if (Input.GetKeyUp(KeyCode.X))
        {
            flipperLeft.isPressed = false;
            rotateAroundAPointLeft.isPressed = false;
            //print("left flipper released");
        }
        
    }


}
    

