using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyBoxCollider : MyCollider
{
    public Vector2 size;
    public float rotation { get { return transform.localRotation.eulerAngles.z; } set { transform.localRotation = Quaternion.Euler(new Vector3(0,0,value)); } }

    protected Vector2 cachedPos;
    protected bool cachedThis;
    //ONLY WORKS STATICALLY

    protected override void Start()
    {
        base.Start();
        
        this.size = new Vector2(transform.localScale.x, transform.localScale.y);

        //Formula taken from Wikipedia
        myRigidbody.momentOfInertia = myRigidbody.GetMass() * 0.08333f * (size.x * size.x + size.y * size.y);

    }

    public bool Collides(MyCircleCollider other)
    {
        //Let Circles handle the collision between these two
        return other.Collides(this);
    }

    //Checks for collision with other and caches the colliding point in world coordinates for penetration.
    //(In the variable cachedPos)
    public bool Collides(MyBoxCollider other)
    {
        
        //Check if any vertex is inside other
        foreach(Vector2 point in GetVertices())
        {
            Vector2 pos = other.transform.InverseTransformPoint(point);
            if(Math.Abs(pos.x) <= 0.5 && Math.Abs(pos.y) <= 0.5) 
            {
                cachedPos = point;
                cachedThis = true;
                return true;
            }
        }
        //Check if any vertex of other is in this
        foreach(Vector2 point in other.GetVertices())
        {
            Vector2 pos = this.transform.InverseTransformPoint(point);
            if(Math.Abs(pos.x) <= 0.5 && Math.Abs(pos.y) <= 0.5)
            {
                //That this calculation is cached for Penetrate()
                other.cachedPos = point;
                other.cachedThis = true;
                cachedThis = false;
                return true;
            }
        }
        return false;
    }

    public (Vector2, Vector2, Vector2, float) Penetrate(MyBoxCollider other)
    {
        Vector2 thisPoint; Vector2 otherPoint; Vector2 normal;
        float distance;
    
        //Get collision point that was cached in the collision check
        if(cachedThis)
        {
            thisPoint = cachedPos;
            //World coordinates of other point
            otherPoint = other.FindClosestPointOnBorder(cachedPos);
        }
        else
        {
            otherPoint = other.cachedPos;
            //World coordinates of other point
            thisPoint = this.FindClosestPointOnBorder(otherPoint);
        }
        normal = (thisPoint - otherPoint).normalized;
        distance = Vector2.Distance(thisPoint, otherPoint);

        //Debug.Log("This: " + this + ", ThisPoint " + thisPoint + ", otherPoint " + otherPoint + ", normal " + normal + ", distance " + distance);

        return (thisPoint, otherPoint, -normal, distance);
        
    }

    public (Vector2, Vector2, Vector2, float) Penetrate(MyCircleCollider collider)
    {
        //Let Circles handle the collision between these two, but swap return values
        var penetration = collider.Penetrate(this);
        return (penetration.Item2, penetration.Item1, -penetration.Item3, penetration.Item4);
    }

    public override List<Vector2> GetVertices()
    {
        List<Vector2> vertices = new();
        Vector3 right = transform.right * size.x / 2;
        Vector3 up = transform.up * size.y / 2;
        vertices.Add(transform.position + (right + up));
        vertices.Add(transform.position + (-right - up));
        vertices.Add(transform.position + (-right + up));
        vertices.Add(transform.position + (right - up));
        return vertices;
    }

    //Visualization of getVe3rtices for debugging
    void OnDrawGizmos()
    {
        var vertices = GetVertices();

        foreach(Vector2 vec in vertices)
        {
            Gizmos.DrawSphere(vec, 0.3f);
        }
        
    }

    //Returns the closest point on the collider to another point OUTSIDE of the collider
    //Point is in local coordinates!! (Call InverseTransformPoint() first)
    public Vector2 FindClosestPoint(Vector2 point)
    {
        Vector2 finalPoint = point;

        if(finalPoint.x > 0.5f) finalPoint.x = 0.5f;
        if(finalPoint.x < -0.5f) finalPoint.x = -0.5f;
        
        if(finalPoint.y < -0.5f) finalPoint.y = -0.5f;
        if(finalPoint.y > 0.5f) finalPoint.y = 0.5f;
        
        return finalPoint;

    }

    //Returns the closest point on the collider to another point INSIDE of the collider
    //Point is in world coordinates!!
    public Vector2 FindClosestPointOnBorder(Vector2 point)
    {
        Debug.Log("This: " + transform.position + ", other: " + point);

        List<Vector2> points = GetVertices();
        float distance = 100000;
        Vector2 closestPoint = new Vector2(0,0);

        foreach(Vector2 p in points)
        {
            float d = Vector2.Distance(p, point);
            if(d < distance)
            {
                closestPoint = p;
                distance = d;
            }
        }

        Vector2 relDist = transform.InverseTransformDirection(closestPoint - point);

        Vector2 returnPoint = this.transform.InverseTransformPoint(point);

        if(Math.Abs(relDist.x) < Math.Abs(relDist.y))
        {
            if(relDist.x < 0) 
            {
                returnPoint.x = -0.5f;
            }
            else returnPoint.x = 0.5f;
        }
        else{
            if(relDist.y < 0) 
            {
                returnPoint.y = -0.5f;
            }
            else returnPoint.y = 0.5f;
        }
        returnPoint = this.transform.TransformPoint(returnPoint);
        return returnPoint;

    }

    public float Cross2D(Vector2 a, Vector2 b)
    {
        return (a.x * b.y) - (a.y * b.x); 
    }



}
