using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float radius;
    public Vector2 pos { get { return transform.position; } set { transform.position = value; } }
    public float pushVel;

    void Start()
    {
        // Get the SpriteRenderer component (or add it if it doesn't exist)
        if (!TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        // Set its sprite to the circle sprite and scale accordingly
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Circle");
        transform.localScale = Vector3.one * radius * 2;
        
    }
}
