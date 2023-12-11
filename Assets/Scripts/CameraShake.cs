using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Vector3 originalPos;

    void Awake()
    {
        instance = this;
        originalPos = transform.localPosition;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;
        while(elapsed < duration)
        {
            float x = Random.Range(-1f,1f) * magnitude;
            float y = Random.Range(-1f,1f) * magnitude;
            transform.localPosition = new Vector3(x,y,originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    public void SmallShake()
    {
        ShakeCamera(0.1f, 0.2f);
    }

    public void MediumShake()
    {
        ShakeCamera(0.1f, 0.3f);
    }

    public void BigShake()
    {
        ShakeCamera(0.1f, 0.5f);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            BigShake();
        }
    }
}
