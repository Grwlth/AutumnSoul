
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

    [Header("Debug")]
    public bool debugGroundCheck = true;
    public float debugRayDistance = 1.5f;

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
            normalGravityScale = rb.gravityScale;
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
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (JumpButtonDown())
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    private void HandleJumpInput()
    {
        if (JumpButtonDown() && debugGroundCheck)
        {
            DebugGroundSurface();
        }

        if (rb == null)
        {
            return;
        }

        if (jumpBufferCounter > 0f &&
            coyoteTimeCounter > 0f)
        {
            Jump();

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }

        // Releasing jump early makes it shorter
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

    private void DebugGroundSurface()
    {
        if (groundCheck == null)
        {
            Debug.LogWarning(
                gameObject.name +
                ": GroundCheck is missing!",
                gameObject
            );

            return;
        }

        Collider2D[] nearbyColliders =
            Physics2D.OverlapCircleAll(
                groundCheck.position,
                groundCheckRadius,
                Physics2D.AllLayers
            );

        Collider2D playerCollider =
            GetComponent<Collider2D>();

        string nearbyInfo = "";

        foreach (Collider2D col in nearbyColliders)
        {
            if (col == playerCollider ||
                col.transform.IsChildOf(transform) ||
                col.transform == transform)
            {
                continue;
            }

            bool included =
                (groundLayer.value &
                (1 << col.gameObject.layer)) != 0;

            nearbyInfo +=
                $"\n- {col.name}" +
                $" | Layer: {LayerMask.LayerToName(col.gameObject.layer)}" +
                $" | Ground Layer: {included}" +
                $" | Trigger: {col.isTrigger}";
        }

        if (string.IsNullOrEmpty(nearbyInfo))
        {
            nearbyInfo = "\n- None";
        }

        Vector2 rayOrigin = groundCheck.position;

        RaycastHit2D[] hits = Physics2D.RaycastAll(
            rayOrigin,
            Vector2.down,
            debugRayDistance,
            Physics2D.AllLayers
        );

        Collider2D surfaceCollider = null;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null)
                continue;

            if (hit.collider == playerCollider ||
                hit.collider.transform == transform ||
                hit.collider.transform.IsChildOf(transform))
            {
                continue;
            }

            surfaceCollider = hit.collider;
            break;
        }

        string surfaceInfo = "No surface detected";

        if (surfaceCollider != null)
        {
            bool included =
                (groundLayer.value &
                (1 << surfaceCollider.gameObject.layer)) != 0;

            surfaceInfo =
                $"{surfaceCollider.name}" +
                $" | Layer: {LayerMask.LayerToName(surfaceCollider.gameObject.layer)}" +
                $" | Ground Layer: {included}" +
                $" | Trigger: {surfaceCollider.isTrigger}";
        }

        Debug.Log(
            $"CHARACTER: {gameObject.name}\n" +
            $"Grounded: {IsGrounded()}\n" +
            $"Coyote: {coyoteTimeCounter:F2}\n" +
            $"VelocityY: {(rb != null ? rb.linearVelocity.y : 0f):F2}\n" +
            $"GroundCheck Position: {groundCheck.position}\n" +
            $"GroundCheck Radius: {groundCheckRadius}\n" +
            $"Surface below: {surfaceInfo}\n" +
            $"Colliders near GroundCheck: {nearbyInfo}",
            surfaceCollider != null ?
                (Object)surfaceCollider.gameObject :
                gameObject
        );
    }

    public virtual void Movement()
    {
        if (rb == null)
        {
            return;
        }

        float targetSpeed =
            horizontalInput * speed;

        float movementRate;

        if (Mathf.Abs(targetSpeed) > 0.01f)
        {
            movementRate = acceleration;
        }
        else
        {
            movementRate = deceleration;
        }

        float newXVelocity =
            Mathf.MoveTowards(
                rb.linearVelocity.x,
                targetSpeed,
                movementRate * Time.fixedDeltaTime
            );

        rb.linearVelocity =
            new Vector2(
                newXVelocity,
                rb.linearVelocity.y
            );
    }

    public virtual void Jump()
    {
        if (rb == null)
        {
            return;
        }

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
        if (animator == null || rb == null)
        {
            return;
        }

        bool grounded = IsGrounded();

        bool running =
            grounded &&
            Mathf.Abs(rb.linearVelocity.x) > 0.1f;

        bool jumping = !grounded;

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
            Debug.LogWarning(
                gameObject.name +
                ": GroundCheck is missing!",
                gameObject
            );

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

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(
            groundCheck.position,
            groundCheck.position +
            Vector3.down * debugRayDistance
        );
    }

    public virtual void TakeDamage(int howMuch)
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

        damagable.TakeDamage(attackDamage);
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

    public void SetVelocity(Vector2 velocity)
    {
        if (rb != null)
        {
            rb.linearVelocity = velocity;
        }
    }

    public abstract void SpecialAbility();
}
