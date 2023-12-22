using UnityEngine;

public class Border : MonoBehaviour
{

    private LineRenderer lineRenderer;

    // The points that define the border (in counter-clockwise order!)
    public Vector2[] points;

    // Using gizmos to draw the border
    // To see this, gizmos have to be enabled in the editor/game view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawLine(points[i], points[(i + 1) % points.Length]);
        }
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // Set the number of positions to the number of points
        lineRenderer.positionCount = points.Length;

        // Set each position
        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }

        // Close the loop
        lineRenderer.loop = true;
    }
}