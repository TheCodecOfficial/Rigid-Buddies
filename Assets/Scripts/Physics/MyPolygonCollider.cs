using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyPolygonCollider : MyCollider
{
    public Vector2 center { get { return new Vector2(transform.position.x, transform.position.y); } }

    private Polygon polygon;

    private Vector2 a, b, c, d;
    private bool updated = false;

    protected override void Start()
    {
        base.Start();
        this.myRigidbody.momentOfInertia = 0.5f * myRigidbody.GetMass(); // TODO: Approximate moment of inertia
        polygon = gameObject.GetComponent<Polygon>();
        Debug.Log(gameObject.name + " my adjusted centroid is " + PolygonUtil.GetCentroid(GetVerticesArray()));
    }

    public override List<Vector2> GetVertices()
    {
        Vector2[] vertices = polygon.GetVerticesWorld();
        return new List<Vector2>(vertices);
    }

    public Vector2[] GetVerticesArray()
    {
        return polygon.GetVerticesWorld();
    }

    public bool Collides(MyCircleCollider other)
    {
        Vector2 closestPoint = PolygonUtil.GetClosestPoint(GetVerticesArray(), other.center);
        return Vector2.Distance(closestPoint, other.center) < other.radius;
    }

    public (Vector2, Vector2, Vector2, float) Penetrate(MyCircleCollider other)
    {
        Vector2 closestPoint = PolygonUtil.GetClosestPoint(GetVerticesArray(), other.center);
        Vector2 normal = (other.center - closestPoint).normalized;
        Vector2 otherPoint = other.center - normal * other.radius;
        float depth = Vector2.Distance(closestPoint, otherPoint);
        return (closestPoint, otherPoint, normal, depth);
    }

    public bool Collides(MyPolygonCollider other)
    {
        (bool collides, Vector2 collisionPoint) sat = SAT.PolyPolyCollision(GetVerticesArray(), other.GetVerticesArray());
        //Debug.Log(sat.collides);
        return sat.collides;
    }

    //Returns the penetration distance as a vector and the attackPoint
    //Returns: collisionPoint on this, collisionPoint on other, Normal, depth
    //Normal is the normal from other outwards
    public (Vector2, Vector2, Vector2, float) Penetrate(MyPolygonCollider other)
    {
        (bool collides, Vector2 collisionPoint) sat = SAT.PolyPolyCollision(GetVerticesArray(), other.GetVerticesArray());
        if (!sat.collides) return (Vector2.zero, Vector2.zero, Vector2.zero, 0);

        //Vector2 closestSelf = PolygonUtil.GetClosestPoint(GetVerticesArray(), sat.collisionPoint);
        //Vector2 closestOther = PolygonUtil.GetClosestPoint(other.GetVerticesArray(), sat.collisionPoint);

        // Get the closest point to the collision point on each of the two polygons
        Vector2 closestSelf = PolygonUtil.GetClosestPoint(GetVerticesArray(), PolygonUtil.GetCentroid(other.GetVerticesArray()));
        Vector2 closestOther = PolygonUtil.GetClosestPoint(other.GetVerticesArray(), PolygonUtil.GetCentroid(GetVerticesArray()));

        Vector2 normal = -(closestSelf - closestOther).normalized;
        float depth = Vector2.Distance(closestSelf, closestOther);

        //normal = -PolygonUtil.GetClosestNormal(GetVerticesArray(), Vector2.zero);

        a = sat.collisionPoint;
        b = closestSelf;
        c = closestOther;
        updated = true;

        //Debug.Log(gameObject.name + ": I am colliding with: " + other.gameObject.name);

        //return (closestSelf, closestOther, normal, depth * 0.25f);
        return (closestOther, closestSelf, normal, depth * 0.25f);
    }

    void OnDrawGizmos()
    {
        if (!updated) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(a, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(b, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(c, 0.1f);
        updated = false;
    }
}