using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class MyRigidbody : MonoBehaviour
{
    [SerializeField]
    private float mass; 
    public float GetMass() {return mass;}

    [SerializeField]
    public Vector2 velocity;
    public Vector2 GetVelocity() {return velocity;}
    public void SetVelocity(Vector2 vel) {velocity = vel;}

    [SerializeField]
    public float angularVelocity;
    public float GetAngularVelocity() {return angularVelocity;}
    public void SetAngularVelocity(float angVel) {angularVelocity = angVel;}

    [SerializeField]
    //Center of mass position
    public float momentOfInertia;
    public float GetMomentOfInertia() {return momentOfInertia;}
    public void SetMomentOfInertia(float inertia) {momentOfInertia = inertia;}

    [SerializeField]
    private MyCollider myCollider;
    public MyCollider GetCollider() {return myCollider;}

    [SerializeField]
    private bool useGravity;

    //if true, always this bounciness will be taken instead of Min
    public bool overrideBounciness;

    public float bounciness;

    public float dragAmount;
    public float angularDragAmount;

    //Set true for things that don't move
    [SerializeField]
    public bool isStatic;

    void Start()
    {
        this.myCollider = GetComponent<MyCollider>();
        this.velocity = new Vector2(0,0);
    }


    public void Simulate()
    {
        //Dont move if static
        if(isStatic)
            return;

        if(useGravity)
            AddForce((Vector2)Physics.gravity, Vector2.zero);

        ApplyDrag();
        ImplicitEuler();
        
    }

    protected void ApplyDrag()
    {
        AddForce(-velocity.normalized * dragAmount, Vector2.zero);
        //angularVelocity = (1 - angularDragAmount) * angularVelocity;
    }


    //Adds velocity and torque of a force applied at position, both in local coordinates!
    //(Actually, "force" is rather an impulse)
    public void AddForce(Vector2 force, Vector2 position)
    {
        velocity += force * Time.deltaTime;
        //angularVelocity += ((position.x * force.y - position.y * force.x)) * Time.deltaTime;
    }

    //Inputs in world frame!
    public void AddImpulse(Vector2 impulse, Vector2 attackPos)
    {
        //Debug.Log("Adding impulse to " + this + ", impulse=" + impulse + ", attackPos=" + attackPos);
        Vector2 radius = new Vector3(attackPos.x, attackPos.y, 0) - this.transform.position;
        //Debug.Log("Radius: " + radius);
        velocity += impulse / mass; 
        //Debug.Log("AngVelChange: " + Cross2D(radius, impulse) / momentOfInertia);
        angularVelocity +=  Cross2D(radius, impulse) / momentOfInertia;
    }

    public void StopMovement()
    {
        velocity = new Vector2(0,0);
        angularVelocity = 0;
    }

    //Symplectic euler
    public void ImplicitEuler()
    {
        transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, angularVelocity * (180 / (float)Math.PI) * Time.deltaTime ));
    }

    //Returns the velocity of a point on this rigidbody
    public Vector2 PointVelocity(Vector2 point)
    {
        Vector2 radius = point - new Vector2(transform.position.x, transform.position.y);
        return velocity + new Vector2(-angularVelocity * radius.y, angularVelocity * radius.x);
    }

    public float Cross2D(Vector2 a, Vector2 b)
    {
        return (a.x * b.y) - (a.y * b.x); 
    }
    
}
