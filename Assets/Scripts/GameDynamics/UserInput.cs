using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{


    public Flipper2 flipperRight;
    public Flipper2 flipperLeft;

    public RotateAroundAPoint rotateAroundAPointRight;
    public RotateAroundAPoint rotateAroundAPointLeft;

    public Flipper3 flipper3Right;
    public Flipper3 flipper3Left;
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
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            flipperRight.isPressed = true;
            rotateAroundAPointRight.isPressed = true;
            flipper3Right.isPressed = true;
            //print("right flipper pressed");
        }
        if (Input.GetKeyUp(KeyCode.M) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            flipperRight.isPressed = false;
            rotateAroundAPointRight.isPressed = false;
            flipper3Right.isPressed = false;
            //print("right flipper released");
        }


        // Left Flipper
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            flipperLeft.isPressed = true;
            rotateAroundAPointLeft.isPressed = true;
            flipper3Left.isPressed = true;
            //print("left flipper pressed");
        }

        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            flipperLeft.isPressed = false;
            rotateAroundAPointLeft.isPressed = false;
            flipper3Left.isPressed = false;
            //print("left flipper released");
        }

    }


}


