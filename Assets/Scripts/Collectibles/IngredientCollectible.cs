using UnityEngine;

public class IngredientCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayableCharacter player =
            other.GetComponent<PlayableCharacter>();

        if (player == null)
            return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectIngredient();
        }

        Destroy(gameObject);
    }
}