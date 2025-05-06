using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float crouchSpeed = 2.5f;
    public float climbSpeed = 3f;

    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = true;

    private bool isGrounded = false;
    private bool isCrouching = false;
    private bool isClimbing = false;
    private bool isNearLadder = false;

    private float horizontalDirection = 0f;
    private bool jumpPressed = false;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Horizontal Movement
        float moveSpeedCurrent = isCrouching ? crouchSpeed : moveSpeed;
        rb.linearVelocity = new Vector2(horizontalDirection * moveSpeedCurrent, rb.linearVelocity.y);

        // Climbing
        if (isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPressed ? climbSpeed : 0f);
        }

        // Jump
        if (jumpPressed && isGrounded && !isCrouching && !isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }

        // Reset jump
        jumpPressed = false;

        // Animations
        anim.SetBool("isRunning", Mathf.Abs(horizontalDirection) > 0.1f && isGrounded);
        anim.SetBool("isCrouching", isCrouching);
        anim.SetBool("isClimbing", isClimbing);

        // Flip sprite
        if (horizontalDirection > 0 && !facingRight) Flip();
        else if (horizontalDirection < 0 && facingRight) Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void OnLeftButtonDown() => horizontalDirection = -1f;
    public void OnLeftButtonUp() => horizontalDirection = 0f;

    public void OnRightButtonDown() => horizontalDirection = 1f;
    public void OnRightButtonUp() => horizontalDirection = 0f;

    public void OnJumpButtonDown()
    {
        // Context-sensitive: climb if near ladder, else jump
        if (isNearLadder)
        {
            StartClimbing();
        }
        else
        {
            jumpPressed = true;
        }
    }

    public void OnCrouchButtonToggle()
    {
        if (isGrounded && !isClimbing)
            isCrouching = !isCrouching;
    }

    private void StartClimbing()
    {
        isClimbing = true;
        rb.gravityScale = 0f;
    }

    private void StopClimbing()
    {
        isClimbing = false;
        rb.gravityScale = 1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isNearLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isNearLadder = false;
            StopClimbing();
        }
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
