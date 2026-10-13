using UnityEngine;

public class GoldenLeaf : MonoBehaviour
{
    public int manaAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayableCharacter player =
            other.GetComponent<PlayableCharacter>();

        if (player == null)
            return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddMana(manaAmount);
        }

        Destroy(gameObject);
    }
}