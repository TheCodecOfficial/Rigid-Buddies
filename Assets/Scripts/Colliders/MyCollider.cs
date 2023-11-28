using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCollider : MonoBehaviour
{
    protected MyRigidbody myRigidbody;

    public void Start()
    {
        myRigidbody = this.GetComponent<MyRigidbody>();
        if(myRigidbody == null)
            Debug.LogError("ERROR: A Collider needs a Rigidbody!!!");    
    }

    public MyRigidbody GetRigidbody() {return myRigidbody;}

}
