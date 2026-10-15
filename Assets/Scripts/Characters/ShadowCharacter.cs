using UnityEngine;

public class ShadowCharacter : PlayableCharacter
{
    [Header("Shadow Ability")]
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
            IShadowBreakable breakable =
                hit.GetComponent<IShadowBreakable>();

            if (breakable != null)
            {
                breakable.BreakWithShadow();
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