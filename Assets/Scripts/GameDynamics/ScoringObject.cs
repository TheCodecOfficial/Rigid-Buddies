using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringObject : MonoBehaviour
{
    MyCollider myCollider;
    public ScoreManager scoreManager;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<MyCollider>();
        myCollider.OnCollisionEnterEvent.AddListener(scoreManager.AddPoints);
        myCollider.OnCollisionEnterEvent.AddListener(DisableObject);

    }

    void DisableObject(MyCollider myCollider)
    {
        if(myCollider.tag == "projectile"){
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }





}
