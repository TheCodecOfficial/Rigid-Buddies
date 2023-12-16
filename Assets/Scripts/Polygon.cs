using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon : MonoBehaviour
{
    public Vector2[] vertices;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;


    public void Init(Vector2[] vertices)
    {
        this.vertices = vertices;
        if (meshFilter == null) meshFilter = gameObject.AddComponent<MeshFilter>();
        if (meshRenderer == null) meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        RebuildMesh();
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
