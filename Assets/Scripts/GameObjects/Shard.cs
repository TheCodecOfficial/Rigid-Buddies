using UnityEngine;

public class Shard : MonoBehaviour
{
    private Vector2 direction;
    private float rotationSpeed;

    private float time;
    private Vector3 originalScale;
    public void Init(Vector2 direction)
    {
        this.direction = direction.normalized * Random.Range(0.5f, 1.5f);
        this.direction += Random.insideUnitCircle * 0.1f;
        this.rotationSpeed = Random.Range(-1f, 1f) * 20f;
        this.originalScale = transform.localScale;
        Destroy(gameObject, 10f);
    }

    void FixedUpdate()
    {
        transform.position += (Vector3)direction * Time.deltaTime * 4f;
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        time += Time.deltaTime;
        if (time > 0.2f)
        {
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, (time - 0.2f) / 1f);
        }
    }
}
