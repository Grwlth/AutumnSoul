using UnityEngine;

public class MushroomTrap : Trap
{
    public override void ApplyDamage(IDamagable damagable)
    {
        damagable.TakeDamage(damage);
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