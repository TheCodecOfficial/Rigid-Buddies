using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class MyCollider : MonoBehaviour
{
    public MyRigidbody myRigidbody;
    //OnCollisionEnter event handler
    //public delegate void CollisionHandler(MyCollider collider);
    //public event CollisionHandler OnCollisionEnterEvent;
    public UnityEvent<(MyCollider, Vector2)> OnCollisionEnterEvent;
    protected virtual void Start()
    {
        myRigidbody = this.GetComponent<MyRigidbody>();
        if(myRigidbody == null)
            Debug.LogError("ERROR: A Collider needs a Rigidbody!!!");  
          
    }

    public MyRigidbody GetRigidbody() {return myRigidbody;}

    //Returns true if there is a collision collide, else false
    public bool Collides(MyCollider collider) {Debug.Log("Error, MyCollider.Collides was called!!!"); return false;}

    public virtual List<Vector2> GetVertices() {return null;}

    //Returns: Position of contact on this object, Position of contact on other object, Collision normal (From other to this) and distance of overlap
    public (Vector2, Vector2, Vector2, float) Penetrate(MyCollider collider) {Debug.Log("Error, MyCollider.Penetrate was called!!!"); return (Vector2.zero, Vector2.zero, Vector2.zero, 0);}
    
}
