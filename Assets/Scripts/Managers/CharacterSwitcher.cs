using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    [Header("Characters")]
    public PlayableCharacter shadow;
    public PlayableCharacter cat;
    public PlayableCharacter crow;

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

    if (shadow == null || cat == null || crow == null)
    {
        Debug.LogError(
            "CharacterSwitcher: Shadow, Cat or Crow is not assigned in the Inspector!"
        );

        return;
    }

    Debug.Log(
        "Shadow active: " + shadow.gameObject.activeSelf +
        " | Cat active: " + cat.gameObject.activeSelf +
        " | Crow active: " + crow.gameObject.activeSelf
    );
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
        if (characters == null || characters.Length != 3)
            return;

        PlayableCharacter current = characters[currentCharacterIndex];

        if (current == null)
        {
            Debug.LogError("Current character is missing!");
            return;
        }

        // Mana check
        if (GameManager.Instance != null)
        {
            if (!GameManager.Instance.TrySpendMana(manaCost))
            {
                Debug.Log("Not enough Mana!");
                return;
            }
        }

        Vector3 currentPosition = current.transform.position;
        int currentHp = current.GetHp();

        // Turn current character off
        current.gameObject.SetActive(false);

        // Next character
        currentCharacterIndex++;

        if (currentCharacterIndex >= characters.Length)
        {
            currentCharacterIndex = 0;
        }

        PlayableCharacter next = characters[currentCharacterIndex];

        if (next == null)
        {
            Debug.LogError("Next character is missing!");
            return;
        }

        // Move new form to same position
        next.transform.position = currentPosition;

        // Preserve HP
        next.SetHp(currentHp);

        // Turn new form on
        next.gameObject.SetActive(true);

        Debug.Log("Switched to: " + next.gameObject.name);
    }
}