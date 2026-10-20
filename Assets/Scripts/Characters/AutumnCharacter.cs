using UnityEngine;

public class AutumnCharacter : PlayableCharacter
{
    [Header("Autumn Ability")]
    public Transform attackPoint;
    public float attackRange = 1.2f;
    public LayerMask trapLayer;

    public override void SpecialAbility()
    {
        if (attackPoint == null)
        {
            return;
        }

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                attackPoint.position,
                attackRange,
                trapLayer
            );

        foreach (Collider2D hit in hits)
        {
            WalkingTreeTrap walkingTree =
                hit.GetComponent<WalkingTreeTrap>();

            if (walkingTree != null)
            {
                walkingTree.TakeDamage(
                    walkingTree.maxHp
                );

                Debug.Log(
                    "Autumn stopped the Walking Tree!"
                );
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(
            attackPoint.position,
            attackRange
        );
    }
}