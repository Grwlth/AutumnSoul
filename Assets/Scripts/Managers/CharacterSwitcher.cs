using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    [Header("Characters")]
    public PlayableCharacter autumn;
    public PlayableCharacter cat;
    public PlayableCharacter crow;

    [Header("Spawn")]
    public Transform spawnPoint;

    [Header("Camera")]
    public CameraFollow cameraFollow;

    [Header("Mana")]
    public int manaCost = 3;

    [Header("Switch Safety")]
    [Tooltip("Small upward offset to prevent the new form from spawning inside the floor.")]
    public float switchGroundOffset = 0.03f;

    private PlayableCharacter[] characters;
    private int currentCharacterIndex = 0;

    private void Awake()
    {
        characters = new PlayableCharacter[]
        {
            autumn,
            cat,
            crow
        };

        // Move all forms to the spawn point
        if (spawnPoint != null)
        {
            foreach (PlayableCharacter character in characters)
            {
                if (character == null)
                    continue;

                character.transform.position =
                    spawnPoint.position;

                Rigidbody2D characterRb =
                    character.GetComponent<Rigidbody2D>();

                if (characterRb != null)
                {
                    characterRb.linearVelocity =
                        Vector2.zero;
                }
            }
        }
        else
        {
            Debug.LogWarning(
                "CharacterSwitcher: No PlayerSpawnPoint assigned!"
            );
        }

        // Always start as Autumn
        if (autumn != null)
        {
            autumn.gameObject.SetActive(true);
        }

        if (cat != null)
        {
            cat.gameObject.SetActive(false);
        }

        if (crow != null)
        {
            crow.gameObject.SetActive(false);
        }

        // Camera starts by following Autumn
        if (cameraFollow != null && autumn != null)
        {
            cameraFollow.SetTarget(autumn.transform);
        }
    }

    private void Start()
    {
        currentCharacterIndex = 0;

        Debug.Log("Starting character: Autumn");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchCharacter();
        }
    }

    private void SwitchCharacter()
    {
        if (characters == null ||
            characters.Length != 3)
        {
            return;
        }

        PlayableCharacter current =
            characters[currentCharacterIndex];

        if (current == null)
        {
            Debug.LogError(
                "Current character is missing!"
            );

            return;
        }

        // Spend Mana
        if (GameManager.Instance != null)
        {
            if (!GameManager.Instance.TrySpendMana(manaCost))
            {
                Debug.Log("Not enough Mana!");
                return;
            }
        }

        Rigidbody2D currentRb =
            current.GetComponent<Rigidbody2D>();

        Collider2D currentCollider =
            current.GetComponent<Collider2D>();

        // Save current horizontal position
        float currentX =
            current.transform.position.x;

        // Save where the player's feet currently are
        float currentFeetY =
            current.transform.position.y;

        if (currentCollider != null)
        {
            currentFeetY =
                currentCollider.bounds.min.y;
        }

        Vector2 currentVelocity =
            Vector2.zero;

        if (currentRb != null)
        {
            currentVelocity =
                currentRb.linearVelocity;
        }

        // Disable current form
        current.gameObject.SetActive(false);

        // Move to next character
        currentCharacterIndex++;

        if (currentCharacterIndex >= characters.Length)
        {
            currentCharacterIndex = 0;
        }

        PlayableCharacter next =
            characters[currentCharacterIndex];

        if (next == null)
        {
            Debug.LogError(
                "Next character is missing!"
            );

            current.gameObject.SetActive(true);
            return;
        }

        // Enable new form first so Unity can calculate its collider bounds
        next.gameObject.SetActive(true);

        Physics2D.SyncTransforms();

        Collider2D nextCollider =
            next.GetComponent<Collider2D>();

        // Start at the same X position
        next.transform.position =
            new Vector3(
                currentX,
                current.transform.position.y,
                current.transform.position.z
            );

        Physics2D.SyncTransforms();

        // Align the new character's feet
        // with the old character's feet
        if (nextCollider != null)
        {
            float nextFeetY =
                nextCollider.bounds.min.y;

            float verticalDifference =
                currentFeetY -
                nextFeetY;

            next.transform.position +=
                new Vector3(
                    0f,
                    verticalDifference + switchGroundOffset,
                    0f
                );
        }

        Physics2D.SyncTransforms();

        Rigidbody2D nextRb =
            next.GetComponent<Rigidbody2D>();

        if (nextRb != null)
        {
            // Keep horizontal movement,
            // but don't force the new form down into the floor.
            nextRb.linearVelocity =
                new Vector2(
                    currentVelocity.x,
                    Mathf.Max(currentVelocity.y, 0f)
                );
        }

        // Update camera target
        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(
                next.transform
            );
        }

        // Play switching SFX
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance
                .PlaySwitchCharacter();
        }

        Debug.Log(
            "Switched to: " +
            next.gameObject.name
        );
    }
}