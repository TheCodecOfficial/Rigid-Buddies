using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    public Vector2 position;
    public Vector2 GetPosition() {return position;}
    public void SetPosition(Vector2 pos) {position = pos;}

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

    public float bounciness;

    //Set true for things that don't move
    [SerializeField]
    public bool isStatic;

    void Start()
    {
        this.myCollider = GetComponent<MyCollider>();
        this.position = new Vector2(transform.position.x, transform.position.y);
    }


    public void Simulate()
    {
        //Dont move if static
        if(isStatic)
            return;

        if(useGravity)
            AddForce((Vector2)Physics.gravity, Vector2.zero);

        ImplicitEuler();
        
    }


    //Adds velocity and torque of a force applied at position, both in local coordinates!
    //(Actually, "force" is rather an impulse)
    public void AddForce(Vector2 force, Vector2 position)
    {
        velocity += force / mass * Time.deltaTime;
        angularVelocity += (position.x * force.y - position.y * force.x) / momentOfInertia * Time.deltaTime;
    }

    public void AddImpulse(Vector2 impulse, Vector2 position)
    {
        velocity += impulse / mass;
        angularVelocity += (position.x * impulse.y - position.y * impulse.x) / momentOfInertia;
    }

    public void AddVelocity(Vector2 velocity, Vector2 position)
    {
        this.velocity += velocity;
        //angularVelocity += (position.x * force.y - position.y * force.x) / momentOfInertia * Time.deltaTime;
    }

    public void StopMovement()
    {
        velocity = new Vector2(0,0);
    }

    //Symplectic euler
    public void ImplicitEuler()
    {
        position += velocity * Time.deltaTime;
        transform.position = new Vector3(position.x, position.y, 0);
    }

    public Vector2 PointVelocity(Vector2 point)
    {
        return velocity + angularVelocity * Vector2.Distance(position, point) * new Vector2(-(point - position).y, (point - position).x).normalized;
    }

    
}
