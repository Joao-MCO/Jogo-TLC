using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public int lives = 3;
    public int collectables = 0;
    public float movementSpeed = 5f;
    public float jumpForce = 10f;
    [FormerlySerializedAs("dobuleJumpForce")]
    public float doubleJumpForce = 5f;
    public float invincibleSeconds = 1f;
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
    private float invincibleUntil;
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

        if (JumpPressed())
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
            }
            else if(extraJumps > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, doubleJumpForce);
                isDoubleJumping = true;
                extraJumps--;
            }
        }

    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        float moveInput = ReadMoveInput();

        rb.linearVelocity = new Vector2(
            moveInput * movementSpeed,
            rb.linearVelocityY
        );

        SetAnimation(moveInput);
    }

    // Equivalente a Input.GetKeyDown(KeyCode.Space), com suporte a gamepad.
    private bool JumpPressed()
    {
        Keyboard keyboard = Keyboard.current;
        Gamepad gamepad = Gamepad.current;
        return (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
            || (gamepad != null && gamepad.buttonSouth.wasPressedThisFrame);
    }

    // Equivalente a Input.GetAxisRaw("Horizontal"): retorna -1, 0 ou 1.
    private float ReadMoveInput()
    {
        float move = 0f;

        Keyboard keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) move -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) move += 1f;
        }

        Gamepad gamepad = Gamepad.current;
        if (move == 0f && gamepad != null)
        {
            float stick = gamepad.leftStick.x.ReadValue() + gamepad.dpad.x.ReadValue();
            if (Mathf.Abs(stick) > 0.5f) move = Mathf.Sign(stick);
        }

        return move;
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
                scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveInput);
                animator.transform.localScale = scale;
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
        if (collision.gameObject.CompareTag("Damage") && Time.time >= invincibleUntil)
        {
            lives--;
            invincibleUntil = Time.time + invincibleSeconds;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
            StartCoroutine(BlinkRed());
        }

        if (lives <= 0 || collision.gameObject.CompareTag("Die"))
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
