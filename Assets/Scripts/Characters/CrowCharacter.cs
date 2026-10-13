using UnityEngine;

public class CrowCharacter : PlayableCharacter
{
    [Header("Crow Ability")]
    public float highJumpForce = 12f;

    public override void SpecialAbility()
    {
        if (!IsGrounded())
            return;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            highJumpForce
        );
    }
}