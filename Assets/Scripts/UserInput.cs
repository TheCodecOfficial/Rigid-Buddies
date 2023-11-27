using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{


    public Flipper flipperRight;
    public Flipper flipperLeft;
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
            //print("right flipper pressed");
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            flipperRight.isPressed = false;
            //print("right flipper released");
        }

        
        // Left Flipper
        if (Input.GetKeyDown(KeyCode.X))
        {
            flipperLeft.isPressed = true;
            //print("left flipper pressed");
        }
        
        if (Input.GetKeyUp(KeyCode.X))
        {
            flipperLeft.isPressed = false;
            //print("left flipper released");
        }
        
    }


}
    

