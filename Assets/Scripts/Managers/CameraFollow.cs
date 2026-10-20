using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float FollowSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }

}