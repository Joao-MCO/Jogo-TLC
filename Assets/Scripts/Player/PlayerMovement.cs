using System.Collections;
using UnityEngine;

public class PlayerMovement : BaseBehaviour
{
    public int lives = 3;
    public float movementSpeed = 5f;
    public float jumpForce = 10f;
    public float dobuleJumpForce = 5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public int extraJumpsValue = 1;
    private int extraJumps;
    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isDoubleJumping;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (isGrounded)
        {        
            extraJumps = extraJumpsValue;
            isDoubleJumping = false;
        }

        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * movementSpeed, rb.linearVelocityY);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
            }
            else if(extraJumps > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, dobuleJumpForce);
                isDoubleJumping = true;
                extraJumps--;
            }
        }

        SetAnimation(moveInput);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (rb.linearVelocity.magnitude < 0.05f &&
            rb.IsTouchingLayers())
        {
            Debug.Log(
                $"Player quase parado. " +
                $"Velocidade: {rb.linearVelocity}, " +
                $"Posição: {rb.position}",
                this
            );
        }
    }

    private void SetAnimation(float moveInput)
    {
        if(isGrounded)
        {
            if(moveInput == 0)
            {
                animator.Play("Idle");
            }else
            {
                Vector3 scale = animator.transform.localScale;
                if (moveInput < 0) scale.x = -1;
                else scale.x = 1;
                animator.Play("Walk");
            }
        }
        else
        {
            if(rb.linearVelocityY > 0)
            {
                if(isDoubleJumping) animator.Play("Double Jump");
                else animator.Play("Jump");
                
            }
            else
            {
                animator.Play("Fall");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Damage")
        {
            lives--;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
            StartCoroutine(BlinkRed());
        }

        if(lives == 0 || collision.gameObject.tag == "Die")
        {
            Die();
        }
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
