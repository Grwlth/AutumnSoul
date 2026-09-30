using UnityEngine;

public class CrowCharacter : PlayableCharacter
{
    public override void SpecialAbility()
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce * 1.8f
        );
    }
}