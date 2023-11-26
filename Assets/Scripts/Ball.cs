using UnityEngine;

public class Ball : MonoBehaviour
{
    public float radius;
    public float mass;
    public float restitution;
    // The position of the ball which in this case is
    // the same as transform.position (unity's builtin position)
    public Vector2 pos { get { return transform.position; } set { transform.position = value; } }
    public Vector2 vel;

    void Start()
    {
        // Get the SpriteRenderer component (or add it if it doesn't exist)
        if (!TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        // Set its sprite to the circle sprite and scale accordingly
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Circle");
        transform.localScale = Vector3.one * radius * 2;
        
    }

    // Update the velocity and position of the ball
    // This is taken from the tutorial
    // https://github.com/matthias-research/pages/blob/master/tenMinutePhysics/04-pinball.html
    public void Simulate()
    {
        vel += (Vector2)Physics.gravity * Time.deltaTime;
        pos += vel * Time.deltaTime;
    }
}
