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
    private Vector2 velocity;
    public Vector2 GetVelocity() {return velocity;}
    public void SetVelocity(Vector2 vel) {velocity = vel;}

    [SerializeField]
    private Vector2 position;
    public Vector2 GetPosition() {return position;}
    public void SetPosition(Vector2 pos) {position = pos;}

    [SerializeField]
    private float restitution;
    public float GetRestitution() {return restitution;}

    [SerializeField]
    private MyCollider myCollider;
    public MyCollider GetCollider() {return myCollider;}

    [SerializeField]
    private bool useGravity;

    //Set true for things that don't move
    [SerializeField]
    private bool isStatic;

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
            AddForce((Vector2)Physics.gravity);
        UpdatePosition();
        
    }

    public void AddForce(Vector2 force)
    {
        velocity += force * Time.deltaTime;
    }

    public void StopMovement()
    {
        velocity = new Vector2(0,0);
    }

    //Symplectic euler
    public void UpdatePosition()
    {
        position += velocity * Time.deltaTime;
        transform.position = new Vector3(position.x, position.y, 0);
    }

    
}
