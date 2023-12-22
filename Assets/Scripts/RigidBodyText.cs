using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// RigidBodyText class handles the visualization of the rigidbody velocity and angular velocity.
public class RigidBodyText : MonoBehaviour
{
    public MyRigidbody rb;
    public TextMeshPro text;
    
    void Start()
    {
        StartCoroutine(SetVisualizationText());
    }

    // Update the position of the text to the rigidbody position and set the
    // text to the rigidbody linear velocity and angular velocity.
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
