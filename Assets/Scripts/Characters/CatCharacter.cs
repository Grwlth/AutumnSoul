using UnityEngine;

public class CatCharacter : PlayableCharacter
{
    [Header("Cat Ability")]
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
            PumpkinTrap pumpkin =
                hit.GetComponent<PumpkinTrap>();

            if (pumpkin != null)
            {
                pumpkin.TakeDamage(
                    pumpkin.maxHp
                );

                Debug.Log(
                    "Cat smashed the Giant Pumpkin!"
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