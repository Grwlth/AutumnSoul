using Unity.VisualScripting;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Vector2 startPos;
    private float length;
    public GameObject cam;
    public Vector2 parallaxEffect = new Vector2(0.5f, 0.3f); // The speed at which the background should move relative to the camera on different axis

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = new Vector2
            (transform.position.x, transform.position.y);
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate distance background move based on cam movement
        float distanceX = cam.transform.position.x * parallaxEffect.x; // 0 = move with cam || 1 = won't move || 0.5 = half
        float distanceY = cam.transform.position.y * parallaxEffect.y;

        float movementX = cam.transform.position.x * (1f - parallaxEffect.x);

        transform.position = new Vector3(startPos.x + distanceX, startPos.y + distanceY, transform.position.z);

        //If background has reached the end of its length adjust its position for infinite scrolling
        if (movementX > startPos.x + length)
        {
            startPos.x += length;
        }
        else if (movementX < startPos.x - length)
        {
            startPos.x -= length;
        }
    }
}
