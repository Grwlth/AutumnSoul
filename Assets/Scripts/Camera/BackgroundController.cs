using Unity.VisualScripting;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Vector2 startPos;
    public GameObject cam;
    public Vector2 parallaxEffect = new Vector2(0.5f, 0.3f); // The speed at which the background should move relative to the camera on different axis

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = new Vector2
            (transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate distance background move based on cam movement
        float distanceX = cam.transform.position.x * parallaxEffect.x; // 0 = move with cam || 1 = won't move || 0.5 = half
        float distanceY = cam.transform.position.y * parallaxEffect.y;

        transform.position = new Vector3(startPos.x + distanceX, startPos.y + distanceY, transform.position.z);
    }
}
