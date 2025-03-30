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

    // Climbing-related variables
    public float climbSpeed = 3f;
    private bool isClimbing = false;
    private bool isNearLadder = false;
    public LayerMask ladderLayer;

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

       
        if (Input.GetKeyDown(KeyCode.C) && isGrounded)
        {
            ToggleCrouch();
        }

       
        if (isCrouching)
        {
            rb.linearVelocity = new Vector2(move * crouchSpeed, rb.linearVelocity.y);  
        }
        else if (isClimbing)
        {
            
            float verticalMovement = Input.GetAxisRaw("Vertical");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalMovement * climbSpeed);  
        }
        else
        {
           
            rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y); 
        }

       
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching && !isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }

        anim.SetBool("isRunning", isGrounded && Mathf.Abs(move) > 0.1f);

        
        if (!isClimbing)
        {
            if (move > 0 && !facingRight)
                Flip();
            else if (move < 0 && facingRight)
                Flip();
        }

        
        if (isNearLadder)
        {
            if (Input.GetKeyDown(KeyCode.W)) 
            {
                StartClimbing();
            }
            else if (Input.GetKeyDown(KeyCode.S)) 
            {
                StartClimbingDown();
            }
        }
        else
        {
            StopClimbing();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void ToggleCrouch()
    {
        isCrouching = !isCrouching;

        
        anim.SetBool("isCrouching", isCrouching);
    }

    private void StartClimbing()
    {
        isClimbing = true;
        rb.gravityScale = 0; 
        anim.SetBool("isClimbing", true); 
    }

    private void StartClimbingDown()
    {
        isClimbing = true;
        rb.gravityScale = 0; 
        anim.SetBool("isClimbing", true); 
    }

    private void StopClimbing()
    {
        if (isClimbing)
        {
            isClimbing = false;
            rb.gravityScale = 1; 
            anim.SetBool("isClimbing", false); 
        }
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
