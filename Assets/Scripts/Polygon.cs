using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon : MonoBehaviour
{
    public Vector2[] vertices;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    void AdjustPositions()
    {
        Vector2 centroid = PolygonUtil.GetBBCentroid(vertices);
        //Debug.Log("Centroid is " + centroid);
        //Vector2 centroid = PolygonUtil.GetCentroid(vertices);
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] -= centroid;
            if (transform.childCount > i) transform.GetChild(i).position += transform.position - (Vector3)centroid;
        }
        transform.position = centroid;
    }

    public void Init(Vector2[] vertices)
    {
        this.vertices = vertices;

        AdjustPositions();

        if (meshFilter == null) gameObject.AddComponent<MeshFilter>();
        if (meshRenderer == null) gameObject.AddComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        RebuildMesh();
    }

    public Vector2[] GetVerticesWorld()
    {
        return PolygonUtil.OffsetVertices(vertices, transform.position);
    }

    public void RebuildMesh()
    {
        if (vertices.Length < 3)
            return;
        Vector3[] verts = new Vector3[vertices.Length + 1];
        verts[vertices.Length] = PolygonUtil.GetCentroid(vertices[0], vertices[1], vertices[2]);
        for (int i = 0; i < vertices.Length; i++)
        {
            verts[i] = vertices[i];
        }
        int[] tris = new int[vertices.Length * 3];
        for (int i = 0; i < vertices.Length; i++)
        {
            tris[i * 3] = vertices.Length;
            tris[i * 3 + 1] = i;
            tris[i * 3 + 2] = (i + 1) % vertices.Length;
        }

        Mesh mesh = new()
        {
            vertices = verts,
            triangles = tris
        };
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
    }

}
