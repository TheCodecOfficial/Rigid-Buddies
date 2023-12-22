using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class MyRigidbody : MonoBehaviour
{
    [SerializeField]
    private float mass;
    public float GetMass() { return mass; }
    public void SetMass(float mass) { this.mass = mass; }

    [SerializeField]
    public Vector2 velocity;
    public Vector2 GetVelocity() { return velocity; }
    public void SetVelocity(Vector2 vel) { velocity = vel; }

    [SerializeField]
    public float angularVelocity;
    public float GetAngularVelocity() { return angularVelocity; }
    public void SetAngularVelocity(float angVel) { angularVelocity = angVel; }

    [SerializeField]
    public float momentOfInertia;
    public float GetMomentOfInertia() { return momentOfInertia; }
    public void SetMomentOfInertia(float inertia) { momentOfInertia = inertia; }

    [SerializeField]
    private MyCollider myCollider;
    public MyCollider GetCollider() { return myCollider; }

    [SerializeField]
    private bool useGravity;

    //if true, always this bounciness will be taken instead of Min
    public bool overrideBounciness;

    //Aka restitution
    public float bounciness;

    //in percent per seconds (eg. 0.5 -> velocity halves every second)
    public float dragAmount;
    public float angularDragAmount;

    //Set true for things that don't move
    [SerializeField]
    public bool isStatic;
    public bool isKinematic;
    public bool hasFixedPosition = false;

    void Start()
    {
        this.myCollider = GetComponent<MyCollider>();
        this.velocity = new Vector2(0, 0);
    }


    public void Simulate()
    {
        //Dont move if static
        if (isStatic)
            return;

        //Apply gravity
        if(useGravity)
            AddForce((Vector2)Physics.gravity*mass, Vector2.zero);

        ApplyDrag();
        SymplecticEuler();

    }

    
    protected void ApplyDrag()
    {
        velocity = velocity - dragAmount * velocity * Time.deltaTime;
        angularVelocity = angularVelocity - angularDragAmount * angularVelocity * Time.deltaTime;
    }


    //Adds velocity (without torque) of a force applied at position, both in local coordinates!
    public void AddForce(Vector2 force, Vector2 position)
    {
        velocity += force / mass * Time.deltaTime;
        //angularVelocity += ((position.x * force.y - position.y * force.x)) * Time.deltaTime;
    }

    //Inputs in world frame
    //Applies an instantanious change of velocity at the attackPos point
    public void AddImpulse(Vector2 impulse, Vector2 attackPos)
    {
        //Velocity part
        velocity += impulse / mass;

        //Rotation part
        Vector2 radius = new Vector3(attackPos.x, attackPos.y, 0) - this.transform.position;
        angularVelocity += Cross2D(radius, impulse) / momentOfInertia;
    }

    public void StopMovement()
    {
        velocity = new Vector2(0, 0);
        angularVelocity = 0;
    }

    //Symplectic euler (Actually only updates position, because all velocity changes are applied in other methods)
    public void SymplecticEuler()
    {
        if(!hasFixedPosition)
            transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, angularVelocity * (180 / (float)Math.PI) * Time.deltaTime ));
    }

    //Returns the velocity of a point on this rigidbody (input and output are in world coordinates)
    public Vector2 PointVelocity(Vector2 point)
    {
        Vector2 radius = point - new Vector2(transform.position.x, transform.position.y);
        return velocity + new Vector2(-angularVelocity * radius.y, angularVelocity * radius.x);
    }

    //2D cross product
    public float Cross2D(Vector2 a, Vector2 b)
    {
        return (a.x * b.y) - (a.y * b.x);
    }

}
