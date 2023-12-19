using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringObject : MonoBehaviour
{
    MyCollider myCollider;
    public ScoreManager scoreManager;
    public int maxHitNumber = 3;
    int hitNumber = 0;
    SpriteRenderer spriteRenderer;
    Color intialColor;
    float intensity = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<MyCollider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider.OnCollisionEnterEvent.AddListener(OnHit);
        intialColor = spriteRenderer.color;

        transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(intialColor.r * intensity, intialColor.g * intensity, intialColor.b * intensity));
    }
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
                //gameObject.SetActive(false);
                Debug.Log("Object hit at: " + collisionInfo.point);
                Destroy(gameObject);
                EffectsManager.instance.ShatterBox(transform.position, collisionInfo.point);
            }
            else
            {
                //float intensity = (maxHitNumber - hitNumber + 1) / (float)maxHitNumber;
                //intensity = Mathf.Pow(intensity, 5f);
                //intensity += 1f;
                //intensity *= 2f;

                // Set emission intensity
                transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(intialColor.r * intensity, intialColor.g * intensity, intialColor.b * intensity));
            }
        }
    }
}
