
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("HUD")]
    public GameHUD hud;

    [Header("Mana")]
    public int maxMana = 100;
    public int currentMana = 100;

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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CountRequiredIngredients();
    }

    private void Start()
    {
        Time.timeScale = 1f;

        currentMana = maxMana;
        collectedIngredients = 0;
        gameEnded = false;

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
            "[GAME START] Ingredients needed: " +
            totalIngredients
        );

        Debug.Log(
            "[GAME START] Mana: " +
            currentMana + "/" + maxMana
        );
    }

    private void CountRequiredIngredients()
    {
        IngredientCollectible[] sceneIngredients =
            FindObjectsByType<IngredientCollectible>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        totalIngredients = sceneIngredients.Length;

        Debug.Log(
            "[INGREDIENT COUNT]\n" +
            "Scene Ingredients: " + totalIngredients + "\n" +
            "TOTAL REQUIRED: " + totalIngredients
        );

        if (totalIngredients != 10)
        {
            Debug.LogWarning(
                "[INGREDIENT COUNT] Expected 10 ingredients, " +
                "but found " + totalIngredients +
                ". Check Ingredients_All!"
            );
        }
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
            currentMana + "/" + maxMana
        );

        CheckOutOfMana(amount);

        return !gameEnded;
    }

    private void CheckOutOfMana(int shapeshiftCost)
    {
        if (HasAllIngredients())
        {
            return;
        }

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

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLose();
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
        currentMana = Mathf.Min(currentMana, maxMana);

        UpdateUI();

        Debug.Log(
            "Mana: " +
            currentMana + "/" + maxMana
        );
    }

    public void CollectIngredient()
    {
        if (gameEnded)
        {
            return;
        }

        collectedIngredients++;

        if (collectedIngredients > totalIngredients)
        {
            collectedIngredients = totalIngredients;
        }

        UpdateUI();

        Debug.Log(
            "Ingredients: " +
            collectedIngredients + "/" + totalIngredients
        );
    }

    public bool HasAllIngredients()
    {
        return totalIngredients > 0 &&
               collectedIngredients >= totalIngredients;
    }

    private void UpdateUI()
    {
        if (manaText != null)
        {
            manaText.text =
                "Mana: " +
                currentMana + "/" + maxMana;
        }

        if (ingredientsText != null)
        {
            ingredientsText.text =
                "Ingredients: " +
                collectedIngredients + "/" + totalIngredients;
        }

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

        int manaUsed = maxMana - currentMana;

        float manaUsedPercent =
            maxMana > 0
                ? (manaUsed / (float)maxMana) * 100f
                : 0f;

        Debug.Log(
            "\n========== LEVEL COMPLETE ==========\n" +
            "Remaining Mana: " +
            currentMana + "/" + maxMana + "\n" +
            "Mana Used: " + manaUsed + "\n" +
            "Mana Used (%): " +
            manaUsedPercent.ToString("F1") + "%\n" +
            "Ingredients Collected: " +
            collectedIngredients + "/" + totalIngredients + "\n" +
            "===================================="
        );

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayWin();
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

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLose();
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
