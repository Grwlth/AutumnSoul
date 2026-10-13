using UnityEngine;

public class FallKillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayableCharacter player =
            other.GetComponent<PlayableCharacter>();

        if (player != null)
        {
            player.Die();
        }
    }
}