using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatTest : MonoBehaviour
{
    public Polygon A, B;
    private Vector2 intersection;
    void Start()
    {
        (bool collide, Vector2 collisionPoint) i = SAT.PolyPolyCollision(A.vertices, B.vertices);
        Debug.Log(i.collide);
        intersection = i.collisionPoint;

        Vector2[] points = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
        PolygonUtil.MakePhysicsPolygon(points, 1f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(intersection, 0.1f);
    }
}