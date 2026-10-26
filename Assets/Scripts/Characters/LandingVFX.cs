
using UnityEngine;

[RequireComponent(typeof(PlayableCharacter))]
public class LandingVFX : MonoBehaviour
{
    [Header("Landing Effect")]
    public ParticleSystem landingParticles;

    [Header("Landing Settings")]
    [Tooltip("Minimum falling speed required for particles.")]
    public float minimumFallSpeed = 1.5f;

    private PlayableCharacter character;

    private bool wasGrounded;
    private float strongestFallSpeed;

    private void Awake()
    {
        character = GetComponent<PlayableCharacter>();

        if (landingParticles != null)
        {
            landingParticles.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }
    }

    private void OnEnable()
    {
        if (character == null)
            character = GetComponent<PlayableCharacter>();

        wasGrounded = character.IsGrounded();
        strongestFallSpeed = 0f;
    }

    private void Update()
    {
        if (landingParticles == null || character == null)
            return;

        bool isGrounded = character.IsGrounded();

        // Remember how fast we were falling.
        if (!isGrounded)
        {
            float verticalSpeed = character.GetVelocity().y;

            if (verticalSpeed < 0f)
            {
                strongestFallSpeed = Mathf.Max(
                    strongestFallSpeed,
                    -verticalSpeed
                );
            }
        }

        // Trigger particles only when landing.
        if (!wasGrounded && isGrounded)
        {
            if (strongestFallSpeed >= minimumFallSpeed)
            {
                landingParticles.Stop(
                    true,
                    ParticleSystemStopBehavior.StopEmittingAndClear
                );

                landingParticles.Play();
            }

            strongestFallSpeed = 0f;
        }

        wasGrounded = isGrounded;
    }
}
