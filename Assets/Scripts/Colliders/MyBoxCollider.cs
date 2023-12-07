using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBoxCollider : MyCollider
{
    public Vector2 size;
    public float rotation { get { return transform.localRotation.eulerAngles.z; } set { transform.localRotation = Quaternion.Euler(new Vector3(0,0,value)); } }

    //ONLY WORKS STATICALLY

    protected override void Start()
    {
        base.Start();

        myRigidbody.isStatic = true;

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
        return other.Collides(this);
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

        /*
        Vector2 p1, p2, p3, p4;
        p1 = new Vector2(-1, -1);
        p2 = new Vector2(1, -1);
        p3 = new Vector2(1, 1);
        p4 = new Vector2(-1, 1);

        Vector2 finalPoint = ClosestPointOnSegment(point, p1, p2);
        float minDist = Vector2.Distance(finalPoint, point);

        Vector2 candidatePoint = ClosestPointOnSegment(point, p2, p3);
        float newDist = Vector2.Distance(candidatePoint, point);
        if(minDist > newDist)
        {
            minDist = newDist;
            finalPoint = candidatePoint;
        }

        candidatePoint = ClosestPointOnSegment(point, p3, p4);
        newDist = Vector2.Distance(candidatePoint, point);
        if(minDist > newDist)
        {
            minDist = newDist;
            finalPoint = candidatePoint;
        }

        candidatePoint = ClosestPointOnSegment(point, p4, p1);
        newDist = Vector2.Distance(candidatePoint, point);
        if(minDist > newDist)
        {
            minDist = newDist;
            finalPoint = candidatePoint;
        }

        return finalPoint;*/

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



}
