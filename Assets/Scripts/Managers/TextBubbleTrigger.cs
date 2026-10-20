using UnityEngine;

public class TextBubbleTrigger : MonoBehaviour
{
    //insert the correct text bubble
    public GameObject textBubble;
    
    //height of the bubble above the player
    public Vector3 offset = new Vector3(0, 2f, 0); 

    private Transform activePlayerTransform;
    private bool isPlayerInside = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // make sure the bubble is hidden when start
        if (textBubble != null)
        {
            textBubble.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // when player is inside trigger area, the bubble follows him
        if (isPlayerInside && activePlayerTransform != null && textBubble != null)
        {
            textBubble.transform.position = activePlayerTransform.position + offset;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // if any of the 3 players enter the trigger area the bubble appears
        if (other.CompareTag("Player"))
        {
            activePlayerTransform = other.transform; 
            isPlayerInside = true;                   
            textBubble.SetActive(true);              
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // if the player leaves the trigger area the bubble disappears
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;                  
            textBubble.SetActive(false);             
            activePlayerTransform = null;            
        }
    }
}
