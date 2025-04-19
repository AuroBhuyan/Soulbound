using UnityEngine;

public class PlayerwithPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 platformVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MovingPlatform"))
        {
            platformVelocity = collision.collider.attachedRigidbody.linearVelocity;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MovingPlatform"))
        {
            platformVelocity = collision.collider.attachedRigidbody.linearVelocity;

            // Add platform's velocity to player
            rb.linearVelocity += new Vector2(platformVelocity.x, 0);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MovingPlatform"))
        {
            platformVelocity = Vector2.zero;
        }
    }
}
