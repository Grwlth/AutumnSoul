
using UnityEngine;

public class HutGoal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayableCharacter player =
            other.GetComponentInParent<PlayableCharacter>();

        if (player == null)
            return;

        if (GameManager.Instance == null)
        {
            Debug.LogWarning(
                "[HUT GOAL] GameManager not found!"
            );
            return;
        }

        Debug.Log("[HUT GOAL] Player entered hut.");

        if (GameManager.Instance.HasAllIngredients())
        {
            GameManager.Instance.WinGame();
        }
        else
        {
            Debug.Log(
                "[HUT GOAL] Missing ingredients: " +
                (GameManager.Instance.totalIngredients -
                 GameManager.Instance.collectedIngredients)
            );
        }
    }
}
