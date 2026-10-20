using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [Header("Mana UI")]
    public Image manaBar;

    [Tooltip("Order: Full, 75%, 50%, 25%, Empty")]
    public Sprite[] manaSprites;

    [Header("Ingredients UI")]
    public Image ingredientBarFill;
    public TMP_Text ingredientsText;

    public void UpdateMana(int currentMana, int maxMana)
    {
        if (manaBar == null ||
            manaSprites == null ||
            manaSprites.Length < 5 ||
            maxMana <= 0)
        {
            return;
        }

        float percentage = (float)currentMana / maxMana;

        if (percentage > 0.75f)
        {
            manaBar.sprite = manaSprites[0];
        }
        else if (percentage > 0.50f)
        {
            manaBar.sprite = manaSprites[1];
        }
        else if (percentage > 0.25f)
        {
            manaBar.sprite = manaSprites[2];
        }
        else if (percentage > 0f)
        {
            manaBar.sprite = manaSprites[3];
        }
        else
        {
            manaBar.sprite = manaSprites[4];
        }
    }

    public void UpdateIngredients(int current, int total)
    {
        if (ingredientBarFill != null)
        {
            if (total > 0)
            {
                ingredientBarFill.fillAmount =
                    (float)current / total;
            }
            else
            {
                ingredientBarFill.fillAmount = 0f;
            }
        }

        if (ingredientsText != null)
        {
            ingredientsText.text = current + "/" + total;
        }
    }
}