using System.Collections.Generic;
using UnityEngine;


public class PolygonUtil
{
    // Gets the closest point on a line segment (a, b) to a point p
    public static Vector2 ClosestPointOnSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float t = ab.sqrMagnitude;
        if (t == 0)
            return a;
        t = Mathf.Max(0, Mathf.Min(1, (Vector2.Dot(p, ab) - Vector2.Dot(a, ab)) / t));
        return a + ab * t;
    }

    // Gets the closest point on a polygon defined by vertices to a point p
    public static Vector2 GetClosestPoint(Vector2[] vertices, Vector2 p)
    {
        Vector2 closestPoint = vertices[0];
        float closestDistance = (p - closestPoint).sqrMagnitude;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 point = ClosestPointOnSegment(p, vertices[i], vertices[(i + 1) % vertices.Length]);
            float distance = (p - point).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestPoint = point;
                closestDistance = distance;
            }
        }
        return closestPoint;
    }

    // Gets the index of the closes segment on a polygon defined by vertices to a point p
    public static int GetClosestSegmentIndex(Vector2[] vertices, Vector2 p, bool reverse = false)
    {
        float closestDistance = 10000000;
        int closestIndex = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            int k = reverse ? i : vertices.Length - i - 1;
            Vector2 point = ClosestPointOnSegment(p, vertices[k], vertices[(k + 1) % vertices.Length]);
            float distance = (p - point).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = k;
            }
        }
        return closestIndex;
    }

    // Checks if a polygon defined by vertices is convex
    public static bool IsConvex(Vector2[] vertices)
    {
        bool sign = false;
        int n = vertices.Length;
        for (int i = 0; i < n; i++)
        {
            double dx1 = vertices[(i + 2) % n].x - vertices[(i + 1) % n].x;
            double dy1 = vertices[(i + 2) % n].y - vertices[(i + 1) % n].y;
            double dx2 = vertices[i].x - vertices[(i + 1) % n].x;
            double dy2 = vertices[i].y - vertices[(i + 1) % n].y;

            double zcrossproduct = dx1 * dy2 - dy1 * dx2;
            if (i == 0)
                sign = zcrossproduct > 0;
            else if (sign != (zcrossproduct > 0))
            {
                return false;
            }
        }
        return true;
    }

    // Gets the centroid of a triangle defined by three vertices
    public static Vector2 GetCentroid(Vector2 a, Vector2 b, Vector2 c)
    {
        return (a + b + c) / 3;
    }

    // Gets the centroid of a polygon defined by vertices
    public static Vector2 GetCentroid(Vector2[] vertices)
    {
        Vector2 centroid = Vector2.zero;
        for (int i = 0; i < vertices.Length; i++)
        {
            centroid += vertices[i];
        }
        return centroid / vertices.Length;
    }

    // Sorts the vertices of a polygon defined by vertices in clockwise order
    public static Vector2[] SortVertices(Vector2[] vertices)
    {
        if (vertices.Length < 3)
        {
            Debug.LogWarning("Cannot sort vertices with less than 3 vertices");
            return null;
        }
        Vector2 centroid = GetCentroid(vertices[0], vertices[1], vertices[2]);
        System.Array.Sort(vertices, (a, b) =>
        {
            float angleA = Mathf.Atan2(a.y - centroid.y, a.x - centroid.x);
            float angleB = Mathf.Atan2(b.y - centroid.y, b.x - centroid.x);
            return angleB.CompareTo(angleA);
        });
        return vertices;
    }

    // Merges neighboring vertices that are too close together into one
    public static Vector2[] MergeVertices(Vector2[] vertices)
    {
        List<Vector2> merged = new();
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 p = vertices[i];
            Vector2 next = vertices[(i + 1) % vertices.Length];
            if (Vector2.Distance(p, next) < 0.01f) continue;
            merged.Add(p);
        }
        return merged.ToArray();
    }

    // Sorts an array of vertices and merges neighboring vertices that are too close together into one
    public static Vector2[] Polygonize(Vector2[] vertices)
    {
        vertices = SortVertices(vertices);
        vertices = MergeVertices(vertices);
        return vertices;
    }

    // Cuts a polygon defined by vertices along a line segment (a, b)
    // Returns an array of two polygons
    public static Vector2[][] Cut(Vector2[] vertices, Vector2 a, Vector2 b)
    {
        Vector2 closestA = GetClosestPoint(vertices, a);
        Vector2 closestB = GetClosestPoint(vertices, b);
        float eps = 0.1f;
        if (Vector2.Distance(a, closestA) > eps)
        {
            Debug.Log("A is not on the polygon");
            Debug.Log("A: " + a + ", closestA: " + closestA);
            return null;
        }
        if (Vector2.Distance(b, closestB) > eps)
        {
            Debug.Log("B is not on the polygon");
            Debug.Log("B: " + b + ", closestB: " + closestB);
            return null;
        }

        a = closestA;
        b = closestB;

        List<Vector2> verticesLeft = new();
        List<Vector2> verticesRight = new();

        verticesLeft.Add(a);
        verticesLeft.Add(b);
        verticesRight.Add(a);
        verticesRight.Add(b);

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 p = vertices[i];
            if (IsLeftOfSegment(p, a, b))
            {
                verticesLeft.Add(p);
            }
            else
            {
                verticesRight.Add(p);
            }
        }

        Vector2[][] result = new Vector2[2][];
        result[0] = Polygonize(verticesLeft.ToArray());
        result[1] = Polygonize(verticesRight.ToArray());
        return result;

    }

    // Gets the area of a triangle defined by three vertices
    public static float GetTriangleArea(Vector2 a, Vector2 b, Vector2 c)
    {
        return Mathf.Abs((a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y)) / 2);
    }

    // Checks if a point p is left of a line segment pointing from a to b
    public static bool IsLeftOfSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        Vector2 ap = p - a;
        return ab.x * ap.y - ab.y * ap.x > 0;
    }

    // Selects k random elements from an array
    public static T[] ChooseK<T>(T[] array, int k)
    {
        T[] result = new T[k];
        for (int i = 0; i < k; i++)
        {
            int index = Random.Range(i, array.Length);
            (array[i], array[index]) = (array[index], array[i]);
            result[i] = array[i];
        }
        return result;
    }

    // Performs a random cut on a polygon defined by vertices
    public static Vector2[][] CutRandom(Vector2[] vertices)
    {
        int randomIndex = Random.Range(0, vertices.Length);
        int randomIndex2 = (randomIndex + 2) % vertices.Length;
        Vector2 mid1 = (vertices[randomIndex] + vertices[(randomIndex + 1) % vertices.Length]) / 2;
        Vector2 mid2 = (vertices[randomIndex2] + vertices[(randomIndex2 + 1) % vertices.Length]) / 2;
        return Cut(vertices, mid1, mid2);
    }

    // Creates a polygon game object from an array of vertices
    public static GameObject MakePolygon(Vector2[] vertices)
    {
        GameObject polygon = new("Polygon");
        Polygon polyScript = polygon.AddComponent<Polygon>();
        polyScript.Init(vertices);
        return polygon;
    }

    // Creates a polygon game object with the necessary components for physics
    public static GameObject MakePhysicsPolygon(Vector2[] vertices, float mass = 1)
    {
        GameObject polygon = MakePolygon(vertices);
        polygon.AddComponent<MyPolygonCollider>();
        MyRigidbody rb = polygon.AddComponent<MyRigidbody>();
        rb.SetMass(mass);
        PhysicsManager.instance.RefreshRigidbodies();
        return polygon;
    }

    // Returns the normal of a line segment (a, b)
    public static Vector2 GetNormal(Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        Vector2 normal = new Vector2(-ab.y, ab.x).normalized;
        return normal;
    }

    // Returns the normal of an egde on a polygon defined by vertices, given its index
    public static Vector2 GetNormal(Vector2[] vertices, int index)
    {
        Vector2 a = vertices[index];
        Vector2 b = vertices[(index + 1) % vertices.Length];
        return GetNormal(a, b);
    }

    // Gets the normals of the edges of a polygon defined by vertices
    public static Vector2[] GetNormals(Vector2[] vertices)
    {
        Vector2[] normals = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++) normals[i] = GetNormal(vertices, i);
        return normals;
    }

    // Check if a point p is a corner of a polygon defined by vertices
    public static bool IsPointOfPolygon(Vector2[] vertices, Vector2 p)
    {
        foreach (Vector2 vertex in vertices)
        {
            if (Vector2.Distance(vertex, p) < 0.01f) return true;
        }
        return false;
    }

    // Gets the closest normal of a polygon defined by vertices to a point p
    public static Vector2 GetClosestNormal(Vector2[] vertices, Vector2 p)
    {
        int closestIndex = GetClosestSegmentIndex(vertices, p);
        Vector2 closestPoint = ClosestPointOnSegment(p, vertices[closestIndex], vertices[(closestIndex + 1) % vertices.Length]);
        Vector2 normal = GetNormal(vertices, closestIndex);
        if (IsPointOfPolygon(vertices, closestPoint))
        {
            normal += GetNormal(vertices, (closestIndex + 1) % vertices.Length);
            return normal.normalized;
        }
        else
        {
            return normal;
        }
    }
}