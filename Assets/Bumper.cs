using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MyCollider>().OnCollisionEnterEvent.AddListener(OnHit);
    }

    void OnHit((MyCollider col, Vector2 point) collisionInfo)
    {
        float size = transform.localScale.x;
        EffectsManager.instance.SpawnShockwave(transform.position, size);
    }
}
