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
        myRigidbody.momentOfInertia = myRigidbody.GetMass() * 0.08333f * (size.x * size.x + size.y * size.y);

    }

    public bool Collides(MyCollider other)
    {
        return false;

        //EVERYTHING BELOW IS NOT WORKING CURRENTLY

        Vector2 normal, point;
        List<Vector2> otherVertices = other.GetVertices();
        //For each normal of the box try seperating axes theorem:

        //NORMAL 1:
        normal = transform.up;
        point = transform.position + size.y * transform.up / 2;

        bool seperated = true;

        foreach(Vector2 vertex in other.GetVertices())
        {
            if(Vector2.Dot(vertex - point, normal) < 0){
                seperated = false;
                break;
            }
        }
        if(seperated) return true;

        //NORMAL 2:
        normal = -transform.up;
        point = transform.position - size.y * transform.up / 2;

        seperated = true;

        foreach(Vector2 vertex in other.GetVertices())
        {
            if(Vector2.Dot(vertex - point, normal) < 0){
                seperated = false;
                break;
            }
        }
        if(seperated) return true;

        //NORMAL 3:
        normal = transform.right;
        point = transform.position + size.x * transform.right / 2;

        seperated = true;

        foreach(Vector2 vertex in other.GetVertices())
        {
            if(Vector2.Dot(vertex - point, normal) < 0){
                seperated = false;
                break;
            }
        }
        if(seperated) return true;

        //NORMAL 4:
        normal = -transform.right;
        point = transform.position - size.x * transform.right / 2;

        seperated = true;

        foreach(Vector2 vertex in other.GetVertices())
        {
            if(Vector2.Dot(vertex - point, normal) < 0){
                seperated = false;
                break;
            }
        }
        if(seperated) return true;

        return false;
    }

    public bool Collides(MyCircleCollider other)
    {
        //Let Circles handle the collision between these two
        return other.Collides(this);
    }

    //Checks for collision with other and caches the colliding point in world coordinates for penetration
    public bool Collides(MyBoxCollider other)
    {
        //Check if any vertex is inside other
        foreach(Vector2 point in GetVertices())
        {
            Vector2 pos = other.transform.InverseTransformPoint(point);
            if(Math.Abs(pos.x) <= 0.5 && Math.Abs(pos.y) <= 0.5) 
            {
                //That this calculation is cached for Penetrate()
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
        
        if(cachedThis)
        {
            thisPoint = cachedPos;
            //World coordinates of other point
            otherPoint = other.transform.TransformPoint(other.FindClosestPointOnBorder(other.transform.InverseTransformPoint(cachedPos)));
        }
        else
        {
            otherPoint = cachedPos;
            //World coordinates of other point
            thisPoint = this.transform.TransformPoint(this.FindClosestPointOnBorder(this.transform.InverseTransformPoint(cachedPos)));
        }
        normal = (thisPoint - otherPoint).normalized;
        distance = Vector2.Distance(thisPoint, otherPoint);

        Debug.Log("This: " + this + ", ThisPoint " + thisPoint + ", otherPoint " + otherPoint + ", normal " + normal + ", distance " + distance);

        return (thisPoint, otherPoint, -normal, distance);
        
    }

    public (Vector2, Vector2, Vector2, float) Penetrate(MyCircleCollider collider)
    {
        //Let Circles handle the collision between these two
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
    //Point is in local coordinates!! (Call InverseTransformPoint() first)
    public Vector2 FindClosestPointOnBorder(Vector2 point)
    {
        Vector2 finalPoint = point;

        if(Math.Abs(finalPoint.x) > Math.Abs(finalPoint.y))
        {
            if(finalPoint.x >= 0) finalPoint.x = 0.5f;
            else finalPoint.x = -0.5f;
        } 
        else
        {
            if(finalPoint.y >= 0) finalPoint.y = 0.5f;
            else finalPoint.y = -0.5f;
        } 
        return finalPoint;

    }

    // This is taken from the tutorial
    // https://github.com/matthias-research/pages/blob/master/tenMinutePhysics/04-pinball.html
    Vector2 ClosestPointOnSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float t = ab.sqrMagnitude;
        if (t == 0)
            return a;
        t = Mathf.Max(0, Mathf.Min(1, (Vector2.Dot(p, ab) - Vector2.Dot(a, ab)) / t));
        return a + ab * t;
    }

    public float Cross2D(Vector2 a, Vector2 b)
    {
        return (a.x * b.y) - (a.y * b.x); 
    }



}
