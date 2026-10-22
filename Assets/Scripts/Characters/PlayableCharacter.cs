using UnityEngine;

public abstract class PlayableCharacter :
    MonoBehaviour,
    IDamagable
{
    [Header("Movement")]
    public float speed = 5f;
    public float acceleration = 35f;
    public float deceleration = 45f;

    [Header("Jump")]
    public float jumpForce = 9f;

    public float coyoteTime = 0.12f;
    public float jumpBufferTime = 0.12f;

    public float fallGravityMultiplier = 2.5f;
    public float lowJumpGravityMultiplier = 2.2f;

    [Header("Health")]
    public int maxHp = 100;
    public int currentHp;

    [Header("Attack")]
    public int attackDamage = 50;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Animation")]
    public Animator animator;

    protected Rigidbody2D rb;

    private float horizontalInput;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private float normalGravityScale;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHp = maxHp;

        if (rb != null)
        {
            normalGravityScale =
                rb.gravityScale;
        }

        if (animator == null)
        {
            animator =
                GetComponentInChildren<Animator>(true);
        }
    }

    protected virtual void Update()
    {
        ReadMovementInput();

        UpdateGroundTimers();

        HandleJumpInput();

        UpdateAnimations();

        // Special Ability / Attack
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayAttack();
            }

            PlayAttackAnimation();

            SpecialAbility();
        }
    }

    protected virtual void FixedUpdate()
    {
        Movement();

        ApplyBetterGravity();
    }

    private void ReadMovementInput()
    {
        // Supports:
        // A / D
        // Left Arrow / Right Arrow
        horizontalInput =
            Input.GetAxisRaw("Horizontal");
    }

    private bool JumpButtonDown()
    {
        return
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.UpArrow);
    }

    private bool JumpButtonUp()
    {
        return
            Input.GetKeyUp(KeyCode.Space) ||
            Input.GetKeyUp(KeyCode.W) ||
            Input.GetKeyUp(KeyCode.UpArrow);
    }

    private bool JumpButtonHeld()
    {
        return
            Input.GetKey(KeyCode.Space) ||
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.UpArrow);
    }

    private void UpdateGroundTimers()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter =
                coyoteTime;
        }
        else
        {
            coyoteTimeCounter -=
                Time.deltaTime;
        }

        if (JumpButtonDown())
        {
            jumpBufferCounter =
                jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -=
                Time.deltaTime;
        }
    }

    private void HandleJumpInput()
    {
        if (jumpBufferCounter > 0f &&
            coyoteTimeCounter > 0f)
        {
            Jump();

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }

        // Releasing any jump button early
        // makes the jump shorter
        if (JumpButtonUp() &&
            rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity =
                new Vector2(
                    rb.linearVelocity.x,
                    rb.linearVelocity.y * 0.5f
                );
        }
    }

    public virtual void Movement()
    {
        float targetSpeed =
            horizontalInput * speed;

        float movementRate;

        if (Mathf.Abs(targetSpeed) > 0.01f)
        {
            movementRate =
                acceleration;
        }
        else
        {
            movementRate =
                deceleration;
        }

        float newXVelocity =
            Mathf.MoveTowards(
                rb.linearVelocity.x,
                targetSpeed,
                movementRate *
                Time.fixedDeltaTime
            );

        rb.linearVelocity =
            new Vector2(
                newXVelocity,
                rb.linearVelocity.y
            );
    }

    public virtual void Jump()
    {
        rb.linearVelocity =
            new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayJump();
        }
    }

    private void UpdateAnimations()
    {
        if (animator == null)
        {
            return;
        }

        bool grounded =
            IsGrounded();

        bool running =
            grounded &&
            Mathf.Abs(rb.linearVelocity.x) > 0.1f;

        bool jumping =
            !grounded;

        animator.SetBool(
            "isRunning",
            running
        );

        animator.SetBool(
            "isJumping",
            jumping
        );
    }

    private void PlayAttackAnimation()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetTrigger(
            "isAttacking"
        );
    }

    private void ApplyBetterGravity()
    {
        if (rb == null)
        {
            return;
        }

        if (rb.linearVelocity.y < 0f)
        {
            rb.gravityScale =
                normalGravityScale *
                fallGravityMultiplier;
        }
        else if (
            rb.linearVelocity.y > 0f &&
            !JumpButtonHeld()
        )
        {
            rb.gravityScale =
                normalGravityScale *
                lowJumpGravityMultiplier;
        }
        else
        {
            rb.gravityScale =
                normalGravityScale;
        }
    }

    public bool IsGrounded()
    {
        if (groundCheck == null)
        {
            return false;
        }

        Collider2D groundHit =
            Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );

        return groundHit != null;
    }

    public virtual void TakeDamage(
        int howMuch
    )
    {
        currentHp -= howMuch;

        Debug.Log(
            gameObject.name +
            " HP: " +
            currentHp
        );

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerDied();
        }
    }

    public virtual void ApplyDamage(
        IDamagable damagable
    )
    {
        if (damagable == null)
        {
            return;
        }

        damagable.TakeDamage(
            attackDamage
        );
    }

    public void SetHp(int hp)
    {
        currentHp =
            Mathf.Clamp(
                hp,
                0,
                maxHp
            );
    }

    public int GetHp()
    {
        return currentHp;
    }

    public Vector2 GetVelocity()
    {
        if (rb == null)
        {
            return Vector2.zero;
        }

        return rb.linearVelocity;
    }

    public void SetVelocity(
        Vector2 velocity
    )
    {
        if (rb != null)
        {
            rb.linearVelocity =
                velocity;
        }
    }

    public abstract void SpecialAbility();
}