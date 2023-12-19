using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    private float growSpeed = 10f;

    void Update()
    {
        transform.localScale += Vector3.one * growSpeed * Time.deltaTime;
    }
}