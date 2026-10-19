using UnityEngine;

public class CharacterJuice : MonoBehaviour
{
    [Header("References")]
    public Transform spriteVisual;
    public Rigidbody2D rb;
    public ParticleSystem landingParticles;

    [Header("Flip")]
    public bool flipWithMovement = true;

    [Header("Jump Stretch")]
    public float jumpStretchY = 1.07f;
    public float jumpSquashX = 0.96f;

    [Header("Fall Stretch")]
    public float fallStretchY = 1.05f;
    public float fallSquashX = 0.97f;

    [Header("Landing Squash")]
    public float landingSquashX = 1.10f;
    public float landingSquashY = 0.90f;
    public float landingDuration = 0.07f;

    [Header("Smoothness")]
    public float returnSpeed = 14f;

    private Vector3 normalScale;

    private bool wasGrounded;
    private bool landing;
    private float landingTimer;

    private PlayableCharacter character;

    private void Awake()
    {
        character = GetComponent<PlayableCharacter>();

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (spriteVisual != null)
        {
            normalScale = spriteVisual.localScale;
        }
    }

    private void Start()
    {
        if (landingParticles != null)
        {
            landingParticles.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }

        if (character != null)
        {
            wasGrounded = character.IsGrounded();
        }
    }

    private void Update()
    {
        if (spriteVisual == null ||
            rb == null ||
            character == null)
        {
            return;
        }

        HandleFlip();
        HandleLanding();
        HandleStretch();

        wasGrounded = character.IsGrounded();
    }

    private void HandleFlip()
    {
        if (!flipWithMovement)
            return;

        float horizontal =
            Input.GetAxisRaw("Horizontal");

        if (horizontal > 0.01f)
        {
            Vector3 scale =
                spriteVisual.localScale;

            scale.x =
                Mathf.Abs(scale.x);

            spriteVisual.localScale =
                scale;
        }
        else if (horizontal < -0.01f)
        {
            Vector3 scale =
                spriteVisual.localScale;

            scale.x =
                -Mathf.Abs(scale.x);

            spriteVisual.localScale =
                scale;
        }
    }

    private void HandleLanding()
    {
        bool grounded =
            character.IsGrounded();

        // The character was in the air
        // and has just touched the ground.
        if (!wasGrounded && grounded)
        {
            landing = true;
            landingTimer = landingDuration;

            PlayLandingParticles();
        }

        if (landing)
        {
            landingTimer -=
                Time.deltaTime;

            float direction =
                Mathf.Sign(
                    spriteVisual.localScale.x
                );

            if (direction == 0)
            {
                direction = 1;
            }

            spriteVisual.localScale =
                new Vector3(
                    normalScale.x *
                    landingSquashX *
                    direction,

                    normalScale.y *
                    landingSquashY,

                    normalScale.z
                );

            if (landingTimer <= 0f)
            {
                landing = false;
            }
        }
    }

    private void PlayLandingParticles()
    {
        if (landingParticles == null)
            return;

        landingParticles.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );

        landingParticles.Play();
    }

    private void HandleStretch()
    {
        if (landing)
            return;

        Vector3 targetScale =
            normalScale;

        float direction =
            Mathf.Sign(
                spriteVisual.localScale.x
            );

        if (direction == 0)
        {
            direction = 1;
        }

        // Rising
        if (!character.IsGrounded() &&
            rb.linearVelocity.y > 0.5f)
        {
            targetScale =
                new Vector3(
                    normalScale.x *
                    jumpSquashX *
                    direction,

                    normalScale.y *
                    jumpStretchY,

                    normalScale.z
                );
        }

        // Falling
        else if (!character.IsGrounded() &&
                 rb.linearVelocity.y < -0.5f)
        {
            targetScale =
                new Vector3(
                    normalScale.x *
                    fallSquashX *
                    direction,

                    normalScale.y *
                    fallStretchY,

                    normalScale.z
                );
        }

        // Normal
        else
        {
            targetScale.x *=
                direction;
        }

        spriteVisual.localScale =
            Vector3.Lerp(
                spriteVisual.localScale,
                targetScale,
                returnSpeed *
                Time.deltaTime
            );
    }
}