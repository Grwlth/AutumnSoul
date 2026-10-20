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
            autumn.gameObject.SetActive(true);

        if (cat != null)
            cat.gameObject.SetActive(false);

        if (crow != null)
            crow.gameObject.SetActive(false);

        //Make the camera follow Autumn at the start
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

        Vector3 currentPosition =
            current.transform.position;

        int currentHp =
            current.GetHp();

        // Disable current form
        current.gameObject.SetActive(false);

        // Go to next form
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

        // Preserve position
        next.transform.position =
            currentPosition;

        // Preserve HP
        next.SetHp(currentHp);

        Rigidbody2D nextRb =
            next.GetComponent<Rigidbody2D>();

        if (nextRb != null)
        {
            nextRb.linearVelocity =
                Vector2.zero;
        }

        // Enable new form
        next.gameObject.SetActive(true);

        // Make the camera follow the new form
        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(next.transform);
        }

        // Play shapeshifting SFX
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySwitchCharacter();
        }

        Debug.Log(
            "Switched to: " +
            next.gameObject.name
        );
    }
}