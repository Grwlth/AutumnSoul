using UnityEngine;

public class PumpkinTrap : Trap, IShadowBreakable
{
    [Header("Cat Stomp")]
    public float bounceForce = 6f;

    public override void ApplyDamage(IDamagable damagable)
    {
        damagable.TakeDamage(damage);
    }

    public void BreakWithShadow()
    {
        TakeDamage(maxHp);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CatCharacter cat =
            collision.gameObject.GetComponent<CatCharacter>();

        if (cat != null)
        {
            Rigidbody2D catRb =
                collision.gameObject.GetComponent<Rigidbody2D>();

            bool catIsAbove =
                cat.transform.position.y >
                transform.position.y + 0.2f;

            bool catIsFalling =
                catRb != null &&
                catRb.linearVelocity.y <= 0.1f;

            if (catIsAbove && catIsFalling)
            {
                Debug.Log("Cat stomped the Pumpkin!");

                TakeDamage(maxHp);

                if (catRb != null)
                {
                    catRb.linearVelocity =
                        new Vector2(
                            catRb.linearVelocity.x,
                            bounceForce
                        );
                }

                return;
            }
        }

        PlayableCharacter player =
            collision.gameObject.GetComponent<PlayableCharacter>();

        if (player != null)
        {
            ApplyDamage(player);
        }
    }
}