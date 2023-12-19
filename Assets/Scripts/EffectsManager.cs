using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager instance;
    private CameraShake cameraShake;

    public GameObject shockwavePrefab;
    public Material shardMaterial;

    public GameObject ballPrefab;
    public Transform ballSpawnPoint;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    public void SpawnShockwave(Vector2 pos, float size)
    {
        GameObject shockwave = Instantiate(shockwavePrefab, pos, Quaternion.identity);
        shockwave.transform.localScale = Vector3.one * size;
        SmallShake();
        Destroy(shockwave, 0.1f);
    }

    public void ShatterBox(Vector2 boxPos, Vector2 collisionPoint)
    {
        Vector2 dir = collisionPoint - boxPos;
        string direction = "";
        if (Mathf.Abs(dir.x) < dir.y * 2) direction = "up";
        if (Mathf.Abs(dir.x) < -dir.y * 2) direction = "down";
        if (Mathf.Abs(dir.y) < dir.x / 2) direction = "right";
        if (Mathf.Abs(dir.y) < -dir.x / 2) direction = "left";
        Debug.Log("Shattering box in direction " + direction);
        Vector2[] vertices = GetBoxVertices(direction);
        vertices = PolygonUtil.OffsetVertices(vertices, boxPos);
        GameObject[] shards = BetterShatter.Shatter(vertices, collisionPoint);
        foreach (GameObject shard in shards)
        {
            // make them a little bit smaller
            if (shard == null) continue;
            shard.transform.localScale *= 0.9f;
            Vector2 movement = (Vector2)shard.transform.position - collisionPoint;
            movement.y *= 2f;
            shard.AddComponent<Shard>().Init(movement);
            shard.GetComponent<MeshRenderer>().material = shardMaterial;
        }
        SmallShake();

        Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
        PhysicsManager.instance.RefreshRigidbodies();       
    }

    Vector2[] GetBoxVertices(string direction)
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

    public void SmallShake()
    {
        cameraShake.ShakeCamera(0.1f, 0.05f);
        Debug.Log("Shaking camera");
    }

    public void MediumShake()
    {
        cameraShake.ShakeCamera(0.1f, 0.1f);
    }

    public void BigShake()
    {
        cameraShake.ShakeCamera(0.1f, 0.15f);
    }
}