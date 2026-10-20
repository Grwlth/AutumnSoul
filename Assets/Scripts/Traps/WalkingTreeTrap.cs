using UnityEngine;

public class WalkingTreeTrap : Trap
{
    public override void ApplyDamage(
        IDamagable damagable
    )
    {
        // HP system is not used anymore.
        // This obstacle does not deal damage.
    }

    private void OnCollisionEnter2D(
        Collision2D collision
    )
    {
        // The obstacle only blocks the player.
        // No damage is applied.
    }
}