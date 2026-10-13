using UnityEngine;

public class HutGoal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayableCharacter player =
            other.GetComponent<PlayableCharacter>();

        if (player == null)
            return;

        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.HasAllIngredients())
        {
            GameManager.Instance.WinGame();
        }
        else
        {
            Debug.Log(
                "You still need to collect all ingredients!"
            );
        }
    }
}