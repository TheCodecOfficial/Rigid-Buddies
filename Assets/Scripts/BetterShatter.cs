using System;
using System.Collections.Generic;
using UnityEngine;

public class BetterShatter : MonoBehaviour
{
    public static GameObject[] Shatter(Vector2[] vertices, Vector2 point)
    {
        Vector2 closestA = PolygonUtil.GetClosestPoint(vertices, point);
        int index = PolygonUtil.GetClosestSegmentIndex(vertices, point);
        int[] segmentIndices = new int[vertices.Length - 1];
        for (int i = 0; i < segmentIndices.Length; i++)
        {
            segmentIndices[i] = (index + 1 + i) % vertices.Length;
        }

        // Choose 3 segments that get a cut
        int[] chosen = PolygonUtil.ChooseK(segmentIndices, 3);
        Array.Sort(chosen, (a, b) =>
        {
            return (a - index + vertices.Length) % vertices.Length - (b - index + vertices.Length) % vertices.Length;
        });
        Vector2[] poly = vertices;
        List<Vector2[]> polysL = new();
        for (int i = 0; i < chosen.Length; i++)
        {
            Vector2 a = vertices[chosen[i]];
            Vector2 b = vertices[(chosen[i] + 1) % vertices.Length];
            Vector2 m = (a + b) / 2;
            Vector2[][] cut = PolygonUtil.Cut(poly, closestA, m);
            Vector2[][] cut2 = PolygonUtil.CutRandom(cut[0]);
            //polys[i] = cut[0];
            polysL.Add(cut2[0]);
            polysL.Add(cut2[1]);
            poly = cut[1];
            if (poly == null) break;
        }
        polysL.Add(poly);
        //polys[chosen.Length] = poly;
        GameObject[] shards = new GameObject[polysL.Count];
        int j = 0;
        foreach (Vector2[] polyg in polysL)
        {
            if (polyg.Length < 3) continue;
            GameObject shard = PolygonUtil.MakePolygon(polyg);
            shards[j] = shard;
            j++;
        }

        return shards;
    }
}