using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("HUD")]
    public GameHUD hud;

    [Header("Mana")]
    public int maxMana = 10;
    public int currentMana = 10;

    [Header("Ingredients")]
    public int totalIngredients = 0;
    public int collectedIngredients = 0;

    [Header("Legacy UI Text (Optional)")]
    public TMP_Text manaText;
    public TMP_Text ingredientsText;

    [Header("End Game UI")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject outOfManaPanel;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;

        currentMana = maxMana;
        collectedIngredients = 0;

        CountRequiredIngredients();

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }

        if (outOfManaPanel != null)
        {
            outOfManaPanel.SetActive(false);
        }

        UpdateUI();

        Debug.Log(
            "Ingredients needed: " +
            totalIngredients
        );

        Debug.Log(
            "Mana: " +
            currentMana +
            "/" +
            maxMana
        );
    }

    private void CountRequiredIngredients()
    {
        TrapSpawner[] trapSpawners =
            FindObjectsByType<TrapSpawner>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None
            );

        totalIngredients = trapSpawners.Length;

        IngredientCollectible[] existingIngredients =
            FindObjectsByType<IngredientCollectible>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None
            );

        totalIngredients += existingIngredients.Length;
    }

    public bool TrySpendMana(int amount)
    {
        if (gameEnded)
        {
            return false;
        }

        if (currentMana < amount)
        {
            CheckOutOfMana(amount);

            return false;
        }

        currentMana -= amount;

        UpdateUI();

        Debug.Log(
            "Mana: " +
            currentMana +
            "/" +
            maxMana
        );

        CheckOutOfMana(amount);

        return !gameEnded;
    }

    private void CheckOutOfMana(
        int shapeshiftCost
    )
    {
        // If all ingredients are already collected,
        // the player is allowed to finish the level
        // even if there is not enough Mana to shapeshift.
        if (HasAllIngredients())
        {
            return;
        }

        // If the player cannot afford another shapeshift
        // while ingredients are still missing,
        // the game ends with the Out Of Mana screen.
        if (currentMana < shapeshiftCost)
        {
            OutOfMana();
        }
    }

    public void OutOfMana()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;

        Debug.Log("OUT OF MANA!");

        if (outOfManaPanel != null)
        {
            outOfManaPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void AddMana(int amount)
    {
        if (gameEnded)
        {
            return;
        }

        currentMana += amount;

        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }

        UpdateUI();

        Debug.Log(
            "Mana: " +
            currentMana +
            "/" +
            maxMana
        );
    }

    public void CollectIngredient()
    {
        if (gameEnded)
        {
            return;
        }

        collectedIngredients++;

        // Extra protection so the counter
        // can never go above the total.
        if (collectedIngredients > totalIngredients)
        {
            collectedIngredients = totalIngredients;
        }

        UpdateUI();

        Debug.Log(
            "Ingredients: " +
            collectedIngredients +
            "/" +
            totalIngredients
        );
    }

    public bool HasAllIngredients()
    {
        return totalIngredients > 0 &&
               collectedIngredients >= totalIngredients;
    }

    private void UpdateUI()
    {
        // OLD TEXT UI
        // These can stay empty in the Inspector
        // if you're only using the new GameHUD.

        if (manaText != null)
        {
            manaText.text =
                "Mana: " +
                currentMana +
                "/" +
                maxMana;
        }

        if (ingredientsText != null)
        {
            ingredientsText.text =
                "Ingredients: " +
                collectedIngredients +
                "/" +
                totalIngredients;
        }

        // NEW HUD
        if (hud != null)
        {
            hud.UpdateMana(
                currentMana,
                maxMana
            );

            hud.UpdateIngredients(
                collectedIngredients,
                totalIngredients
            );
        }
    }

    public void WinGame()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;

        Debug.Log("YOU WIN!");

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void PlayerDied()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;

        Debug.Log("GAME OVER!");

        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}