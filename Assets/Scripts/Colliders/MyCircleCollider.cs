using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCircleCollider : MyCollider
{
    [SerializeField]
    private Vector2 center;
    [SerializeField]
    private float radius;
    public float GetRadius() {return radius;}
}
