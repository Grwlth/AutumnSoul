using UnityEngine;

public class CrowCharacter : PlayableCharacter
{
    [Header("Crow Ability")]
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
            ScarecrowTrap scarecrow =
                hit.GetComponent<ScarecrowTrap>();

            if (scarecrow != null)
            {
                scarecrow.TakeDamage(
                    scarecrow.maxHp
                );

                Debug.Log(
                    "Crow attacked the Scarecrow!"
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