using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager instance;
    private CameraShake cameraShake;

    public GameObject shockwavePrefab;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    public void SpawnShockwave(Vector2 pos)
    {
        GameObject shockwave = Instantiate(shockwavePrefab, pos, Quaternion.identity);
        SmallShake();
        Destroy(shockwave, 0.1f);
    }

    public void SmallShake()
    {
        cameraShake.ShakeCamera(0.1f, 0.05f);
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