using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyCircleCollider : MyCollider
{
    [SerializeField]
    public Vector2 center { get { return new Vector2(transform.position.x, transform.position.y); } }
    [SerializeField]
    public float radius;
    public float GetRadius() { return radius; }

    protected override void Start()
    {
        base.Start();
        this.myRigidbody.momentOfInertia = 0.5f * myRigidbody.GetMass() * radius * radius;
    }

    public bool Collides(MyPolygonCollider other)
    {
        Vector2 closestPoint = PolygonUtil.GetClosestPoint(other.GetVerticesArray(), center);
        return Vector2.Distance(closestPoint, center) < radius;
    }

    public (Vector2, Vector2, Vector2, float) Penetrate(MyPolygonCollider other)
    {
        Vector2 closestPoint = PolygonUtil.GetClosestPoint(other.GetVerticesArray(), center);
        Vector2 normal = (center - closestPoint).normalized;
        Vector2 otherPoint = center - normal * radius;
        float depth = Vector2.Distance(closestPoint, otherPoint);
        return (closestPoint, otherPoint, normal, depth);
    }

    public bool Collides(MyCircleCollider other)
    {
        return Vector2.Distance(this.center, other.center) < this.radius + other.radius;
    }

    public bool Collides(MyBoxCollider other)
    {
        //In the box collider local transform, SCALE AFFECTED!
        Vector2 pos = other.transform.InverseTransformPoint(center);
        float radiusX = Math.Abs(radius / other.transform.localScale.x);
        float radiusY = Math.Abs(radius / other.transform.localScale.y);

        if (Math.Abs(pos.x) - radiusX > 0.5) return false;
        if (Math.Abs(pos.y) - radiusY > 0.5) return false;

        if (Math.Abs(pos.x) <= 0.5) return true;
        if (Math.Abs(pos.y) <= 0.5) return true;

        Vector2 closest = other.FindClosestPoint(pos);
        closest = other.transform.TransformPoint(closest);

        return (closest - center).sqrMagnitude <= radius * radius;

    }

    //Returns the penetration distance as a vector and the attackPoint
    //Returns: collisionPoint on this, collisionPoint on other, Normal, depth
    //Normal is the normal from other outwards
    public (Vector2, Vector2, Vector2, float) Penetrate(MyBoxCollider other)
    {

        Vector2 pos = other.transform.InverseTransformPoint(center);

        //Find closest point on box
        Vector2 closestPoint = other.FindClosestPoint(pos);
        closestPoint = other.transform.TransformPoint(closestPoint);

        Vector2 attackPoint = center + (closestPoint - center).normalized * radius;
        Vector2 direction = (center - attackPoint).normalized;

        //Debug.Log("This: " + this + ", ThisPoint " + attackPoint + ", otherPoint " + closestPoint + ", normal " + direction + ", distance " + Vector2.Distance(attackPoint, closestPoint));

        return (attackPoint, closestPoint, direction, Vector2.Distance(attackPoint, closestPoint));

    }

    //Returns the penetration distance and position of penetrating point as a vector
    //Normal is from other to this
    public (Vector2, Vector2, Vector2, float) Penetrate(MyCircleCollider other)
    {
        Vector2 normal = (other.center - center); //From this to other
        float dist = (radius + other.radius) - normal.magnitude; //Pene Distance
        normal.Normalize();

        Vector2 pos1 = center + (radius - dist) * normal;
        Vector2 pos2 = other.center - (other.radius - dist) * normal;

        return (pos1, pos2, -normal, dist);
    }


}
