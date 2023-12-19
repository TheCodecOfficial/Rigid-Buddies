using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Polygon))]
public class PolygonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Polygon polygon = (Polygon)target;
        if (GUILayout.Button("Create Mesh"))
        {
            Vector2[] vertices = new Vector2[polygon.transform.childCount];
            for (int i = 0; i < polygon.transform.childCount; i++)
            {
                vertices[i] = polygon.transform.GetChild(i).position;
            }

            vertices = PolygonUtil.Polygonize(vertices);
            polygon.Init(vertices);
        }
    }
}