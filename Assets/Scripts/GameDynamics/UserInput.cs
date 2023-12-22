using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{



    public Flipper flipperRight;
    public Flipper flipperLeft;

    // Update is called once per frame
    void Update()
    {
        // Right Flipper
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            flipperRight.isPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.M) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            flipperRight.isPressed = false;
        }

        // Left Flipper
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            flipperLeft.isPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            flipperLeft.isPressed = false;
        }
    }
}


