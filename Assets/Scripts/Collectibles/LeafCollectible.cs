
using UnityEngine;

public class LeafCollectible : MonoBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
        {
            return;
        }

        PlayableCharacter player =
            other.GetComponentInParent<PlayableCharacter>();

        if (player == null)
        {
            return;
        }

        collected = true;

        // Play the dedicated leaf pickup sound.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLeaf();
        }

        // Remove the collected leaf.
        Destroy(gameObject);
    }
}
