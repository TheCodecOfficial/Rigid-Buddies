using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyCollider : MonoBehaviour
{
    public MyRigidbody myRigidbody;

    protected virtual void Start()
    {
        myRigidbody = this.GetComponent<MyRigidbody>();
        if(myRigidbody == null)
            Debug.LogError("ERROR: A Collider needs a Rigidbody!!!");    
    }

    public MyRigidbody GetRigidbody() {return myRigidbody;}

    public bool Collides(MyCollider collider) {Debug.Log("Error, MyCollider.Collides was called!!!"); return false;}

    public virtual List<Vector2> GetVertices() {return null;}

    public (Vector2, Vector2, Vector2, float) Penetrate(MyCollider collider) {Debug.Log("Error, MyCollider.Penetrate was called!!!"); return (Vector2.zero, Vector2.zero, Vector2.zero, 0);}
 
}
