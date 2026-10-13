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
            return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            trapLayer
        );

        foreach (Collider2D hit in hits)
        {
            IDamagable target = hit.GetComponent<IDamagable>();

            if (target != null)
            {
                ApplyDamage(target);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(
            attackPoint.position,
            attackRange
        );
    }
}