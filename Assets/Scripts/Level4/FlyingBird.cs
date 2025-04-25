using UnityEngine;

public class FlyingBird : MonoBehaviour
{
    private Vector2 direction;
    private float speed;

    public void Initialize(Vector2 dir, float moveSpeed)
    {
        direction = dir.normalized;
        speed = moveSpeed;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        // Destroy the bird if it goes too far from camera view
        float distanceFromCamera = Mathf.Abs(transform.position.x - Camera.main.transform.position.x);
        if (distanceFromCamera > 20f)
        {
            Destroy(gameObject);
        }
    }
}
