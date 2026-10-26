
using UnityEngine;

public class GoldenLeaf : MonoBehaviour
{
    [Header("Mana")]
    public int manaAmount = 1;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
            return;

        PlayableCharacter player =
            other.GetComponentInParent<PlayableCharacter>();

        if (player == null)
            return;

        collected = true;

        // Restore mana to the player.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddMana(manaAmount);
        }

        // Play the dedicated leaf pickup SFX.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLeaf();
        }

        // Remove the collected leaf.
        Destroy(gameObject);
    }
}
