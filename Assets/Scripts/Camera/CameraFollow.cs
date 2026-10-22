using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow Settings")]
    public float smoothSpeed = 5f;

    [Header("Offset")]
    public Vector3 offset = new Vector3(0f, 1f, -10f);

    [Header("World Limits")]
    public Transform leftLimit;
    public Transform rightLimit;
    public Transform bottomLimit;
    public Transform topLimit;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition =
            target.position + offset;

        Vector3 smoothedPosition =
            Vector3.Lerp(
                transform.position,
                desiredPosition,
                smoothSpeed * Time.deltaTime
            );

        if (cam != null && cam.orthographic)
        {
            float cameraHalfHeight =
                cam.orthographicSize;

            float cameraHalfWidth =
                cameraHalfHeight * cam.aspect;

            // Horizontal limits
            if (leftLimit != null &&
                rightLimit != null)
            {
                float minX =
                    leftLimit.position.x +
                    cameraHalfWidth;

                float maxX =
                    rightLimit.position.x -
                    cameraHalfWidth;

                if (minX <= maxX)
                {
                    smoothedPosition.x =
                        Mathf.Clamp(
                            smoothedPosition.x,
                            minX,
                            maxX
                        );
                }
            }

            // Vertical limits
            if (bottomLimit != null &&
                topLimit != null)
            {
                float minY =
                    bottomLimit.position.y +
                    cameraHalfHeight;

                float maxY =
                    topLimit.position.y -
                    cameraHalfHeight;

                if (minY <= maxY)
                {
                    smoothedPosition.y =
                        Mathf.Clamp(
                            smoothedPosition.y,
                            minY,
                            maxY
                        );
                }
            }
        }

        smoothedPosition.z =
            offset.z;

        transform.position =
            smoothedPosition;
    }

    public void SetTarget(
        Transform newTarget
    )
    {
        target =
            newTarget;
    }
}