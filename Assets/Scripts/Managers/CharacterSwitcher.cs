using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    [Header("Characters")]
    public PlayableCharacter shadow;
    public PlayableCharacter cat;
    public PlayableCharacter crow;

    [Header("Spawn")]
    public Transform spawnPoint;

    [Header("Mana")]
    public int manaCost = 3;

    private PlayableCharacter[] characters;
    private int currentCharacterIndex = 0;

    private void Awake()
    {
        characters = new PlayableCharacter[]
        {
            shadow,
            cat,
            crow
        };

        // Move all forms to the chosen spawn point
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

        // Always start as Shadow
        if (shadow != null)
            shadow.gameObject.SetActive(true);

        if (cat != null)
            cat.gameObject.SetActive(false);

        if (crow != null)
            crow.gameObject.SetActive(false);
    }

    private void Start()
    {
        currentCharacterIndex = 0;

        Debug.Log("Starting character: Shadow");
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

        // Check Mana
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

        // Move to next form
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

        // Keep the exact same position
        next.transform.position =
            currentPosition;

        next.SetHp(currentHp);

        // Reset vertical/horizontal movement on switch
        Rigidbody2D nextRb =
            next.GetComponent<Rigidbody2D>();

        if (nextRb != null)
        {
            nextRb.linearVelocity =
                Vector2.zero;
        }

        next.gameObject.SetActive(true);

        Debug.Log(
            "Switched to: " +
            next.gameObject.name
        );
    }
}