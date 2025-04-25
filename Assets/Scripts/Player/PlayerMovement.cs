   /* using UnityEngine;

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
*/


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

    public float crouchSpeed = 2.5f;
    private bool isCrouching = false;

    public float climbSpeed = 3f;
    private bool isClimbing = false;
    private bool isNearLadder = false;
    public LayerMask ladderLayer;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private bool swipeDetected = false;

    private float horizontalDirection = 0f;
    private float verticalDirection = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        DetectSwipeGesture();

        // === Movement ===
        if (isCrouching)
        {
            rb.linearVelocity = new Vector2(horizontalDirection * crouchSpeed, rb.linearVelocity.y);
        }
        else if (isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalDirection * climbSpeed);
        }
        else
        {
            rb.linearVelocity = new Vector2(horizontalDirection * moveSpeed, rb.linearVelocity.y);
        }

        // === Jump ===
        if (swipeDetected && verticalDirection > 0 && isGrounded && !isCrouching && !isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }

        anim.SetBool("isRunning", isGrounded && Mathf.Abs(horizontalDirection) > 0.1f);

        if (!isClimbing)
        {
            if (horizontalDirection > 0 && !facingRight) Flip();
            else if (horizontalDirection < 0 && facingRight) Flip();
        }

        if (!isNearLadder) StopClimbing();
    }

    private void DetectSwipeGesture()
{
    swipeDetected = false;
    verticalDirection = 0f;

#if UNITY_EDITOR || UNITY_STANDALONE
    if (Input.GetMouseButtonDown(0))
    {
        touchStartPos = Input.mousePosition;
    }
    else if (Input.GetMouseButtonUp(0))
    {
        touchEndPos = Input.mousePosition;
        Vector2 swipeDelta = touchEndPos - touchStartPos;

        if (swipeDelta.magnitude > 50f)
        {
            swipeDetected = true;

            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                horizontalDirection = swipeDelta.x > 0 ? 1f : -1f;
            }
            else
            {
                if (swipeDelta.y > 0)
                {
                    verticalDirection = 1f;
                }
                else
                {
                    ToggleCrouch();
                }
            }
        }
        else
        {
            // ✨ No valid swipe — STOP movement
            horizontalDirection = 0f;
        }
    }
#else
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            touchStartPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            touchEndPos = touch.position;
            Vector2 swipeDelta = touchEndPos - touchStartPos;

            if (swipeDelta.magnitude > 50f)
            {
                swipeDetected = true;

                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    horizontalDirection = swipeDelta.x > 0 ? 1f : -1f;
                }
                else
                {
                    if (swipeDelta.y > 0)
                    {
                        verticalDirection = 1f;
                    }
                    else
                    {
                        ToggleCrouch();
                    }
                }
            }
            else
            {
                // ✨ No valid swipe — STOP movement
                horizontalDirection = 0f;
            }
        }
    }
#endif
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
            StartClimbing();
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
