
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomIngredientSprite : MonoBehaviour
{
    [Header("Possible Ingredient Sprites")]
    public Sprite[] ingredientSprites;

    [Header("Settings")]
    public bool randomizeOnAwake = true;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (randomizeOnAwake)
        {
            RandomizeSprite();
        }
    }

    public void RandomizeSprite()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (ingredientSprites == null ||
            ingredientSprites.Length == 0)
        {
            Debug.LogWarning(
                $"{gameObject.name}: No ingredient sprites assigned!"
            );

            return;
        }

        int randomIndex =
            Random.Range(0, ingredientSprites.Length);

        Sprite selectedSprite =
            ingredientSprites[randomIndex];

        if (selectedSprite != null)
        {
            spriteRenderer.sprite = selectedSprite;
        }
    }
}
