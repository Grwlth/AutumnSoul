
using UnityEngine;

public class AutumnCharacter : PlayableCharacter
{
    [Header("Autumn Ability")]
    public Transform attackPoint;
    public float attackRange = 1.2f;
    public LayerMask trapLayer;

    [Header("Facing Direction")]
    public Transform visual;

    private float initialAttackX;

    protected override void Awake()
    {
        base.Awake();

        if (attackPoint != null)
        {
            initialAttackX =
                Mathf.Abs(attackPoint.localPosition.x);
        }
    }

    protected override void Update()
    {
        base.Update();

        UpdateAttackDirection();
    }

    private void UpdateAttackDirection()
    {
        if (attackPoint == null || visual == null)
            return;

        float direction =
            visual.localScale.x >= 0f ? 1f : -1f;

        Vector3 position = attackPoint.localPosition;

        position.x = initialAttackX * direction;

        attackPoint.localPosition = position;
    }

    public override void SpecialAbility()
    {
        if (attackPoint == null)
            return;

        // Update the direction before attacking
        UpdateAttackDirection();

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
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            attackPoint.position,
            attackRange
        );
    }
}
