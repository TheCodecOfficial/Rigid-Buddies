using UnityEngine;

public class Ball : MonoBehaviour
{
    public MyRigidbody myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<MyRigidbody>();
        
        if (!TryGetComponent(out MyCircleCollider myCollider))
        {
            myCollider = gameObject.AddComponent<MyCircleCollider>();
        }

        // Get the SpriteRenderer component (or add it if it doesn't exist)
        if (!TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        // Set its sprite to the circle sprite and scale accordingly
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Circle");
        transform.localScale = Vector3.one * myCollider.GetRadius() * 2;
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(myRigidbody != null)
                myRigidbody.AddForce(Vector2.up * 10000, Vector2.zero);
        }
    }

    // Update the velocity and position of the ball
    // This is taken from the tutorial
    // https://github.com/matthias-research/pages/blob/master/tenMinutePhysics/04-pinball.html

}
