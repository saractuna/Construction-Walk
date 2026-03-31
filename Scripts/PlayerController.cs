using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D ladderCollider;
    Animator playerAnimator;
    PlayerAudio playerAudio;
    AudioSource playerAudioSource;

    [SerializeField] float xMoveSpeed = 1.0f;
    [SerializeField] public float jumpForce = 1.0f;
    [SerializeField] float jumpTime;
    [SerializeField] float buttonTime = 1.0f;
    [SerializeField] bool jumping;
    [SerializeField] float jumpRaycastDistance;
    [SerializeField] LayerMask groundLayerMask;

    [SerializeField] Vector2 movementValue;
    float xInput, yInput;

    public float enemyBounceForce = 1.5f;
    public bool falling;

    [SerializeField] bool onLadder = false;
    [SerializeField] float ladderSpeed = 1f;
    [SerializeField] LayerMask ladderLayerMask;
    [SerializeField] float initialGravity = 1.5f;

    bool isWalking = false;

    // Add this variable to keep track of the surface type
    private string currentSurface = "Concrete"; // Default surface

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ladderCollider = GetComponentInChildren<BoxCollider2D>();
        initialGravity = rb.gravityScale;
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<PlayerAudio>();
        playerAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GetInput();
        movementValue.x = xInput * xMoveSpeed;
        Jump();
        CheckFalling();
        CheckLadder();
        SetAnimation();
    }

    void SetAnimation()
    {
        if (falling)
        {
            playerAnimator.SetBool("isJumping", true);
        }

        RaycastHit2D landtest = Physics2D.Raycast(transform.position, Vector2.down, jumpRaycastDistance, groundLayerMask);
        if (landtest.collider && falling)
        {
            playerAnimator.SetBool("isJumping", false);
        }

        playerAnimator.SetBool("isOnLadder", onLadder);
        playerAnimator.SetBool("isWalking", isWalking);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            RaycastHit2D jumpTest = Physics2D.Raycast(transform.position, Vector2.down, jumpRaycastDistance, groundLayerMask);

            if (jumpTest.collider)
            {
                jumping = true;
                jumpTime = 0;

                playerAudio.PlayJumpSound();
            }

            if (onLadder)
            {
                onLadder = false;
            }
        }

        if (jumping)
        {
            rb.velocity = new Vector2(movementValue.x, jumpForce);
            jumpTime += Time.deltaTime;
            playerAnimator.SetBool("isJumping", true);
        }

        if (Input.GetButtonUp("Jump") | jumpTime > buttonTime)
        {
            jumping = false;
        }
    }

    void GetInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (xInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Face right
        }
        else if (xInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Face left
        }
    }

    public void CheckFalling()
    {
        if (rb.velocity.y < 0)
        {
            falling = true;
        }
        else
        {
            falling = false;
        }
    }

    void CheckLadder()
    {
        if ((Input.GetAxisRaw("Vertical") != 0) && ladderCollider.IsTouchingLayers(ladderLayerMask))
        {
            onLadder = true;
        }

        if (!ladderCollider.IsTouchingLayers(ladderLayerMask))
        {
            onLadder = false;
        }

        if (onLadder)
        {
            rb.gravityScale = 1;

            if (!playerAudioSource.isPlaying)
            {
                playerAudio.PlayClimbSound();
            }
        }
        else
        {
            rb.gravityScale = initialGravity;
        }
    }

    private void FixedUpdate()
    {
        if (onLadder)
        {
            // Ladder Mode
            movementValue.x = xInput * ladderSpeed;
            movementValue.y = yInput * ladderSpeed;

            rb.velocity = new Vector2(movementValue.x, movementValue.y);
        }
        else
        {
            // Normal Mode
            movementValue.x = xInput * xMoveSpeed;
            rb.velocity = new Vector2(movementValue.x, rb.velocity.y);

            if (movementValue.x != 0)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            if (movementValue.x != 0 && !playerAudioSource.isPlaying)
            {
                playerAudio.PlayWalkSound();
            }
        }
    }

    // Detect collision with ground and update surface type
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Concrete"))
        {
            currentSurface = "Concrete";
        }
        else if (collision.gameObject.CompareTag("Platform"))
        {
            currentSurface = "Platform";
        }

        // Update the PlayerAudio script with the new surface type
        playerAudio.SetCurrentSurface(currentSurface);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Continuously update surface type while colliding
        if (collision.gameObject.CompareTag("Concrete"))
        {
            currentSurface = "Concrete";
        }
        else if (collision.gameObject.CompareTag("Platform"))
        {
            currentSurface = "Platform";
        }

        // Update the PlayerAudio script with the new surface type
        playerAudio.SetCurrentSurface(currentSurface);
    }
}