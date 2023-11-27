using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    public float radius;
    public float length;
    float restAngle = 0; //in degrees
    public float maxRotation; //in degrees
    public float angularVelocity; //in degrees per second
    [HideInInspector] public float rotationSign;

    // changing
    public float rotation { get { return transform.localRotation.eulerAngles.z; } set { transform.localRotation = Quaternion.Euler(new Vector3(0,0,value)); } }
    public Vector2 pos { get { return transform.position; } set { transform.position = value; } }
    public float currentAngularVelocity = 0f;
    public float touchIdentifier = -1;


    // Start is called before the first frame update
    void Start()
    {
        //print(gameObject.name + restAngle);
        maxRotation = Mathf.Abs(maxRotation);
        rotationSign = Mathf.Sign(maxRotation);
        //rotation = restAngle;
        //restAngle = rotation;
    }


    public void Simulate() 
    {
        float prevRotation = rotation;
        bool pressed = touchIdentifier >= 0;
        if (pressed) 
            rotation = Mathf.Min(rotation + Time.deltaTime * angularVelocity, maxRotation);
        else 
            //rotation = Mathf.Max(rotation - Time.deltaTime * angularVelocity, restAngle);
            rotation = Mathf.Max(rotation - Time.deltaTime * angularVelocity, 0f);
        
        currentAngularVelocity = rotationSign * (rotation - prevRotation) / Time.deltaTime;
        print(rotation);
    }

    // todo ?
    public bool select(Vector2 _pos) {
        Vector2 d = pos - _pos;
        return d.magnitude < length;
    }
    
    public Vector2 getTip() 
    {
        float angle = restAngle + rotationSign * rotation;
        Vector2 dir = new Vector2(Mathf.Cos(angle* (Mathf.PI / 180)), Mathf.Sin(angle* (Mathf.PI / 180)));
        Vector2 tip = pos + dir * length;

        return tip;
    }

}