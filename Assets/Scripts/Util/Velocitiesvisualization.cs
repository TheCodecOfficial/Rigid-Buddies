using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Velocitiesvisualization : MonoBehaviour
{
    public List<MyRigidbody> projectilesRB;
    public List<MyRigidbody> flippersRB;
    public TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {

        MyRigidbody[] allRigidbodies = FindObjectsOfType<MyRigidbody>();
        // add to rigidbodies only the ones that are not static
        foreach (MyRigidbody rb in allRigidbodies)
        {
            if (rb.gameObject.tag == "projectile")
            {
                projectilesRB.Add(rb);
            }
            else if (rb.gameObject.tag == "Flipper")
            {
                flippersRB.Add(rb);
            }
        }
        //sort the lists by name
        projectilesRB.Sort((x, y) => x.name.CompareTo(y.name));
        flippersRB.Sort((x, y) => x.name.CompareTo(y.name));
        StartCoroutine(SetVisualizationText());
    }

    private IEnumerator SetVisualizationText(){
        while(true)
        {
            string textToDisplay = "VELOCITIES VISUALIZATION\n\n";
            textToDisplay += "Projectiles:\n";
            foreach (MyRigidbody rb in projectilesRB)
            {
                //round rb.angular velocity to 2 decimal numbers
                float angVelRound = Mathf.Round(rb.angularVelocity * 100f) / 100f;
                textToDisplay += rb.name + " Velocity: " + rb.velocity + " Angular velocity: " + angVelRound+ "\n";
            }
            textToDisplay += "Flipper components:\n";
            foreach (MyRigidbody rb in flippersRB)
            {
                //round rb.angular velocity to 2 decimal numbers
                float angVelRound = Mathf.Round(rb.angularVelocity * 100f) / 100f;
                textToDisplay += rb.name + " Velocity: " + rb.velocity + " Angular velocity: " + angVelRound + "\n";
            }
            text.text = textToDisplay;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
