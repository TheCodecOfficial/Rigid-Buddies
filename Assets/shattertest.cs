using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shattertest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        /*Vector2[] vertices = new Vector2[12];
        for (int i = 0; i < 5; i++)
        {
            vertices[2 * i] = new Vector2(i - 2, 1);
            vertices[2 * i + 1] = new Vector2(i - 2, -1);
        }
        vertices[10] = new Vector2(2, 0);
        vertices[11] = new Vector2(-2, 0);*/

        Vector2[] vertices = GetVertices("left");
        vertices = PolygonUtil.OffsetVertices(vertices, new Vector2(10, 0));

        Debug.Log("Vertices are: ");
        foreach (Vector2 v in vertices)
        {
            Debug.Log(v);
        }

        //Polygon polygon = GetComponent<Polygon>();
        //Vector2[] vertices = polygon.GetVerticesWorld();

        BetterShatter.Shatter(vertices, new Vector2(0, 0));
    }

    Vector2[] GetVertices(string direction)
    {
        List<Vector2> vertices = new List<Vector2>();

        // Add the four corners
        vertices.Add(new Vector2(-2, 1));
        vertices.Add(new Vector2(-2, -1));
        vertices.Add(new Vector2(2, -1));
        vertices.Add(new Vector2(2, 1));

        // Depending on the direction, add some extra vertices
        if (direction == "up")
        {
            vertices.Add(new Vector2(-1, -1));
            vertices.Add(new Vector2(0, -1));
            vertices.Add(new Vector2(1, -1));
        }
        else if (direction == "down")
        {
            vertices.Add(new Vector2(-1, 1));
            vertices.Add(new Vector2(0, 1));
            vertices.Add(new Vector2(1, 1));
        }
        else if (direction == "left")
        {
            vertices.Add(new Vector2(2, 0));
        }
        else if (direction == "right")
        {
            vertices.Add(new Vector2(-2, 0));
        }

        // Scale each vertex by 0.5
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] *= 0.5f;
        }
        Vector2[] verticesArray = vertices.ToArray();
        verticesArray = PolygonUtil.SortVertices(verticesArray);
        return verticesArray;
    }
}
