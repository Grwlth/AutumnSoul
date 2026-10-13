using UnityEngine;

public class CatCharacter : PlayableCharacter
{
    [Header("Cat Ability")]
    public BoxCollider2D catCollider;
    public Transform visual;

    public float crouchHeightMultiplier = 0.5f;

    private Vector2 normalColliderSize;
    private Vector2 normalColliderOffset;

    private Vector3 normalVisualScale;

    private bool isCrouching;

    protected override void Awake()
    {
        base.Awake();

        if (catCollider == null)
            catCollider = GetComponent<BoxCollider2D>();

        if (catCollider != null)
        {
            normalColliderSize = catCollider.size;
            normalColliderOffset = catCollider.offset;
        }

        if (visual != null)
        {
            normalVisualScale = visual.localScale;
        }
    }

    public override void SpecialAbility()
    {
        if (isCrouching)
        {
            StandUp();
        }
        else
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouching = true;

        if (catCollider != null)
        {
            catCollider.size = new Vector2(
                normalColliderSize.x,
                normalColliderSize.y * crouchHeightMultiplier
            );

            catCollider.offset = new Vector2(
                normalColliderOffset.x,
                normalColliderOffset.y -
                normalColliderSize.y * 0.25f
            );
        }

        if (visual != null)
        {
            visual.localScale = new Vector3(
                normalVisualScale.x,
                normalVisualScale.y * crouchHeightMultiplier,
                normalVisualScale.z
            );
        }
    }

    private void StandUp()
    {
        isCrouching = false;

        if (catCollider != null)
        {
            catCollider.size = normalColliderSize;
            catCollider.offset = normalColliderOffset;
        }

        if (visual != null)
        {
            visual.localScale = normalVisualScale;
        }
    }
}