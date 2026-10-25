
using UnityEngine;

public class TextBubbleTrigger : MonoBehaviour
{
    [Header("Text Bubble")]
    public GameObject textBubble;

    [Header("Position")]
    public Vector3 offset = new Vector3(0, 2f, 0);

    private Transform activePlayerTransform;
    private bool isPlayerInside = false;

    void Start()
    {
        if (textBubble != null)
        {
            textBubble.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInside &&
            activePlayerTransform != null &&
            textBubble != null)
        {
            textBubble.transform.position =
                activePlayerTransform.position + offset;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        activePlayerTransform = other.transform;
        isPlayerInside = true;

        if (textBubble != null)
        {
            textBubble.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.transform != activePlayerTransform)
            return;

        isPlayerInside = false;

        if (textBubble != null)
        {
            textBubble.SetActive(false);
        }

        activePlayerTransform = null;
    }
}
