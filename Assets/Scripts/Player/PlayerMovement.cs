using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = true;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    private bool isGrounded;

    // Crouch-related variables
    public float crouchSpeed = 2.5f;
    private bool isCrouching = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Check crouch input
        if (Input.GetKeyDown(KeyCode.C) && isGrounded)
        {
            ToggleCrouch();
        }

        // Crouch movement
        if (isCrouching)
        {
            rb.linearVelocity = new Vector2(move * crouchSpeed, rb.linearVelocity.y);  // Move at crouch speed
        }
        else
        {
            rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);  // Move at normal speed
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }

        anim.SetBool("isRunning", isGrounded && Mathf.Abs(move) > 0.1f);

        // Flip the character based on movement direction
        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void ToggleCrouch()
    {
        isCrouching = !isCrouching;

        // Trigger crouch animation without changing collider size
        anim.SetBool("isCrouching", isCrouching);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
