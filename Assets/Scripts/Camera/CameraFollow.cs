using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow Settings")]
    public float smoothSpeed = 5f;

    [Header("Offset")]
    public Vector3 offset = new Vector3(0f, 1f, -10f);

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

        transform.position =
            smoothedPosition;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}