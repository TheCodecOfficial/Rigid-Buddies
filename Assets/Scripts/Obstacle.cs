using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    void Start()
    {
        MyCapsuleCollider col = this.GetComponent<MyCapsuleCollider>();
        col.SetRadius(transform.localScale.y / 2);
        col.SetLength(transform.localScale.x - col.GetRadius());
    }
}
