
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Settings")]
    public float smoothSpeed = 5f;

    [Header("World Bounds")]
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null || cam == null)
            return;

        Vector3 desiredPosition = target.position;
        desiredPosition.z = transform.position.z;

        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        // Clamp camera position to level boundaries
        float minX = minBounds.x + halfWidth;
        float maxX = maxBounds.x - halfWidth;
        float minY = minBounds.y + halfHeight;
        float maxY = maxBounds.y - halfHeight;

        // If the level is smaller than the camera view,
        // center the camera on that axis.
        if (minX > maxX)
            smoothedPosition.x = (minBounds.x + maxBounds.x) / 2f;
        else
            smoothedPosition.x = Mathf.Clamp(
                smoothedPosition.x, minX, maxX
            );

        if (minY > maxY)
            smoothedPosition.y = (minBounds.y + maxBounds.y) / 2f;
        else
            smoothedPosition.y = Mathf.Clamp(
                smoothedPosition.y, minY, maxY
            );

        transform.position = smoothedPosition;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
