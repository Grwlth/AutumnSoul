using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    [Header("Settings")]
    public bool flipWhenPlayerIsLeft = true;

    private Transform player;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer =
            GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        FindActivePlayer();

        if (player == null ||
            spriteRenderer == null)
        {
            return;
        }

        bool playerIsLeft =
            player.position.x <
            transform.position.x;

        if (flipWhenPlayerIsLeft)
        {
            spriteRenderer.flipX =
                playerIsLeft;
        }
        else
        {
            spriteRenderer.flipX =
                !playerIsLeft;
        }
    }

    private void FindActivePlayer()
    {
        PlayableCharacter[] players =
            FindObjectsByType<PlayableCharacter>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None
            );

        if (players.Length > 0)
        {
            player =
                players[0].transform;
        }
    }
}