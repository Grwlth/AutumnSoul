using UnityEngine;

public class MushroomTrap : Trap, IShadowBreakable
{
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
        PlayableCharacter player =
            collision.gameObject.GetComponent<PlayableCharacter>();

        if (player != null)
        {
            ApplyDamage(player);
        }
    }
}