using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Mana")]
    public int maxMana = 10;
    public int currentMana = 10;

    [Header("Ingredients")]
    public int totalIngredients = 0;
    public int collectedIngredients = 0;

    [Header("UI Text")]
    public TMP_Text manaText;
    public TMP_Text ingredientsText;

    [Header("Win / Lose UI")]
    public GameObject winPanel;
    public GameObject losePanel;

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
        // Every trap spawn point eventually produces one ingredient
        TrapSpawner[] trapSpawners =
            FindObjectsByType<TrapSpawner>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None
            );

        totalIngredients = trapSpawners.Length;

        // Also count any ingredients that already exist in the level
        IngredientCollectible[] existingIngredients =
            FindObjectsByType<IngredientCollectible>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None
            );

        totalIngredients += existingIngredients.Length;
    }

    public bool TrySpendMana(int amount)
    {
        if (currentMana < amount)
        {
            Debug.Log("Not enough Mana!");
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

        return true;
    }

    public void AddMana(int amount)
    {
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
        collectedIngredients++;

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

        Debug.Log("YOU LOST!");

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