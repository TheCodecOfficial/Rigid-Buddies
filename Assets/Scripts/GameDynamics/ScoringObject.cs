using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scoring object class handles the behaviour of scoring objects in the game.
public class ScoringObject : MonoBehaviour
{
    
    public ScoreManager scoreManager;
    public int maxHitNumber = 3; // Max number of hits before the object is destroyed.
    int hitNumber = 0; // The current number of hits
    SpriteRenderer spriteRenderer; // The sprite renderer of the object.
    Color intialColor;
    MyCollider myCollider;
    float intensity = 0.2f; // The intensity of the emission color.

    void Start()
    {
        // Get the components.
        myCollider = GetComponent<MyCollider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider.OnCollisionEnterEvent.AddListener(OnHit);
        intialColor = spriteRenderer.color;
        
        // Set the emission color.
        transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.
            SetColor("_EmissionColor", new Color(intialColor.r * intensity,
            intialColor.g * intensity, intialColor.b * intensity));
    }

    // Called when the object is hit.
    void OnHit((MyCollider col, Vector2 point) collisionInfo)
    {
        myCollider = collisionInfo.col;
        if (myCollider.tag == "projectile")
        {
            hitNumber++;
            intensity += 0.3f;
            scoreManager.AddPoints(myCollider);

            if (hitNumber >= maxHitNumber)
            {
                // Shatter and destroy the object.
                Debug.Log("Object hit at: " + collisionInfo.point);
                Destroy(gameObject);
                EffectsManager.instance.ShatterBox(transform.position, collisionInfo.point);
            }
            else
            {
                transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.
                    SetColor("_EmissionColor", new Color(intialColor.r * intensity,
                    intialColor.g * intensity, intialColor.b * intensity));
            }
        }
    }
}
