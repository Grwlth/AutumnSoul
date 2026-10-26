using UnityEngine;

public class EndGameFailText : MonoBehaviour
{
    public GameObject textBubble;
    public Vector3 offset = new Vector3(0, 2f, 0);

    private Transform activePlayerTransform;
    private bool isPlayerInside = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (textBubble != null) textBubble.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInside && activePlayerTransform != null && textBubble != null)
        {
            textBubble.transform.position = activePlayerTransform.position + offset;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // checks if it is a player and if has all ingredients
        if (other.CompareTag("Player") && GameManager.Instance.HasAllIngredients() == false)
        {
            activePlayerTransform = other.transform;
            isPlayerInside = true;
            textBubble.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            textBubble.SetActive(false);
            activePlayerTransform = null;
        }
    }
}
