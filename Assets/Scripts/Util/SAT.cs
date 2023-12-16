using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAT
{
    public static (bool, Vector2) PolyPolyCollision(Vector2[] vertices_A, Vector2[] vertices_B)
    {
        // Get edge normals of both polygons
        Vector2[] normals_A = PolygonUtil.GetNormals(vertices_A);
        Vector2[] normals_B = PolygonUtil.GetNormals(vertices_B);
        Vector2[] normals = new Vector2[normals_A.Length + normals_B.Length];
        normals_A.CopyTo(normals, 0);
        normals_B.CopyTo(normals, normals_A.Length);

        // For each normal
        // 1. Project each vertex of both polygons onto the normal
        // 2. Check if the projections overlap

        List<Vector2> points = new List<Vector2>();
        List<Vector2> directions = new List<Vector2>();
        foreach (Vector2 normal in normals)
        {
            // 1. Projection
            Vector2 projection_A = Project(vertices_A, normal);
            Vector2 projection_B = Project(vertices_B, normal);

            // 2. Overlap Check
            // SAT: If there is no overlap, then the polygons cannot be intersecting
            bool no_overlap = projection_A.x > projection_B.y || projection_B.x > projection_A.y;
            if (no_overlap) return (false, Vector2.zero);
            else
            {
                Vector2 overlap = new(Mathf.Max(projection_A.x, projection_B.x), Mathf.Min(projection_A.y, projection_B.y));
                Vector2 midpoint = (overlap.x + overlap.y) / 2 * normal;
                Vector2 direction = new(-normal.y, normal.x);

                points.Add(midpoint);
                directions.Add(direction);
            }
        }

        // To approximate the intersection point, we reconstruct the line segments from the midpoints and directions
        // and find the average of the intersection point of each pair of neighboring line segments
        Vector2 collisionPoint = Vector2.zero;
        int count = 0;
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 intersection = Intersection(points[i], directions[i], points[i + 1], directions[i + 1]);
            if (!float.IsNaN(intersection.x))
            {
                collisionPoint += intersection;
                count++;
            }
        }
        collisionPoint /= count;
        //Vector2 collisionPointA = PolygonUtil.GetClosestPoint(vertices_A, collisionPoint);
        //Vector2 collisionPointB = PolygonUtil.GetClosestPoint(vertices_B, collisionPoint);
        //collisionPoint = (collisionPointA + collisionPointB) / 2;

        // SAT: If all projections overlap, then the polygons must be intersecting
        return (true, collisionPoint);

    }

    // Project point onto vector
    static Vector2 Project(Vector2[] vertices, Vector2 normal)
    {
        if (vertices.Length == 0)
        {
            Debug.LogError("No vertices to project!");
            return Vector2.zero;
        }
        float min = Vector2.Dot(vertices[0], normal);
        float max = min;
        for (int i = 1; i < vertices.Length; i++)
        {
            float projection = Vector2.Dot(vertices[i], normal);
            if (projection < min) min = projection;
            else if (projection > max) max = projection;
        }
        return new Vector2(min, max);
    }

    // Get the intersection point of two lines
    static Vector2 Intersection(Vector2 A, Vector2 dA, Vector2 B, Vector2 dB)
    {
        // Check if the lines are parallel or coincident
        float det = dA.x * dB.y - dA.y * dB.x;
        if (det == 0) return new Vector2(float.NaN, float.NaN);

        // Solve for t
        float t = ((B.x - A.x) * dB.y - (B.y - A.y) * dB.x) / det;
        //float s = ((B.x - A.x) * dA.y - (B.y - A.y) * dA.x) / det;

        // Calculate the intersection point
        Vector2 intersection_point = new(A.x + t * dA.x, A.y + t * dA.y);

        return intersection_point;
    }
}