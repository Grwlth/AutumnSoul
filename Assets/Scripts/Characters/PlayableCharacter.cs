using UnityEngine;

public abstract class PlayableCharacter : MonoBehaviour, IDamagable
{
    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 7f;

    [Header("Health")]
    public int maxHp = 100;
    public int currentHp;

    [Header("Attack")]
    public int attackDamage = 50;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHp = maxHp;
    }

    protected virtual void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SpecialAbility();
        }
    }

    public virtual void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(
            horizontal * speed,
            rb.linearVelocity.y
        );
    }

    public virtual void Jump()
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
        );
    }

    public bool IsGrounded()
    {
        if (groundCheck == null)
            return false;

        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    public virtual void TakeDamage(int howMuch)
    {
        currentHp -= howMuch;

        Debug.Log(gameObject.name + " HP: " + currentHp);

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

    public virtual void ApplyDamage(IDamagable damagable)
    {
        damagable.TakeDamage(attackDamage);
    }

    public void SetHp(int hp)
    {
        currentHp = Mathf.Clamp(hp, 0, maxHp);
    }

    public int GetHp()
    {
        return currentHp;
    }

    public Vector2 GetVelocity()
    {
        return rb.linearVelocity;
    }

    public void SetVelocity(Vector2 velocity)
    {
        rb.linearVelocity = velocity;
    }

    public abstract void SpecialAbility();
}