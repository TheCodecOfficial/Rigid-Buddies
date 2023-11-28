using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCapsuleCollider : MyCollider
{
    [SerializeField]
    private float radius;
    public float GetRadius() {return radius;}
    public void SetRadius(float radius) {this.radius = radius;}

    [SerializeField]
    private float length;
    public void SetLength(float l) {length = l;}
    [SerializeField]
    public float angularVelocity; //in degrees per second
    public float GetAngularVelocity() {return angularVelocity;}

    [SerializeField]
    public float rotation { get { return transform.localRotation.eulerAngles.z; } set { transform.localRotation = Quaternion.Euler(new Vector3(0,0,value)); } }

    void Start()
    {
        base.Start();
    }

    public Vector2 GetHalfLength()
    {
        Vector2 dir = new Vector2(Mathf.Cos(rotation * (Mathf.PI / 180)), Mathf.Sin(rotation * (Mathf.PI / 180)));
        Vector2 tip = dir * length * 0.5f;

        return tip;
    }

    //Draw the capsule
    private void OnDrawGizmos()
    {
        if(this.myRigidbody == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.myRigidbody.GetPosition(), 0.3f);
        Gizmos.DrawSphere(this.myRigidbody.GetPosition() + GetHalfLength(), 0.3f);
        Gizmos.DrawSphere(this.myRigidbody.GetPosition() - GetHalfLength(), 0.3f);
        
    }
    
}
