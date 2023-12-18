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
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<MyCollider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider.OnCollisionEnterEvent.AddListener(OnHit);
        intialColor = spriteRenderer.color;

    }
    void OnHit(MyCollider myCollider){
        if(myCollider.tag == "projectile")
        {
            hitNumber++;
            scoreManager.AddPoints( myCollider);

            if(hitNumber >= maxHitNumber){
                //gameObject.SetActive(false);
                Destroy(gameObject);
            }else{
                
                float darkness = ((float)maxHitNumber - (float)hitNumber)/(float)maxHitNumber;
                spriteRenderer.color = new Color(intialColor.r*darkness, intialColor.g*darkness, intialColor.b*darkness);
            }
        }
    }
    

}
