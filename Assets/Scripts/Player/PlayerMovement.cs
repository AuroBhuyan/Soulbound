using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;      // Movement speed
    public float jumpForce = 12f;     // Jump force
    public float smoothTime = 0.05f;  // Smoothing for movement

    private Rigidbody2D rb;
    private Vector2 velocity = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;  // Prevent player from toppling over
    }

    void Update()
    {
        // Move left or right using A/D or Arrow Keys
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 targetVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, targetVelocity, ref velocity, smoothTime);

        // Jumping logic (Press Space to jump)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}
