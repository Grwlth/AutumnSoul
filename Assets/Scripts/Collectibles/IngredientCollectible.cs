using UnityEngine;

public class IngredientCollectible :
    MonoBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter2D(
        Collider2D other
    )
    {
        if (collected)
        {
            return;
        }

        PlayableCharacter player =
            other.GetComponent<PlayableCharacter>();

        if (player == null)
        {
            return;
        }

        collected = true;

        // Add ingredient to counter
        if (GameManager.Instance != null)
        {
            GameManager.Instance
                .CollectIngredient();
        }

        // Play pickup SFX
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance
                .PlayIngredient();
        }

        // Remove ingredient
        Destroy(gameObject);
    }
}