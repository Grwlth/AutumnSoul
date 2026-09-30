using UnityEngine;

public abstract class PlayableCharacter : MonoBehaviour, IDamagable
{
    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 7f;

    [Header("Health")]
    public int maxHp = 100;
    public int currentHp;

    protected Rigidbody2D rb;
    protected bool isGrounded;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        currentHp = maxHp;
    }

protected virtual void Update()
{
    Movement();

    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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

    isGrounded = false;
}

    public virtual void TakeDamage(int howMuch)
    {
        currentHp -= howMuch;

        Debug.Log(
            gameObject.name +
            " took " +
            howMuch +
            " damage. HP: " +
            currentHp
        );

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log(gameObject.name + " died!");
    }

    public virtual void ApplyDamage(IDamagable damagable)
    {
        damagable.TakeDamage(20);
    }

    public abstract void SpecialAbility();

    private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Ground"))
    {
        isGrounded = true;
    }
}
}