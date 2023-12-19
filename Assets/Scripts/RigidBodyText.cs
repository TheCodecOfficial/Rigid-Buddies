using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RigidBodyText : MonoBehaviour
{
    public MyRigidbody rb;
    public TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetVisualizationText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SetVisualizationText(){
        while(true)
        {
            Vector3 rbposition = rb.transform.position;
            rbposition.z = -0.1f;
            transform.position = rbposition;
            text.text =rb.velocity + "\n" + Mathf.Round(rb.angularVelocity * 100f) / 100f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    
}
