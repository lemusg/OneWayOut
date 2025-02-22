using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Vector3 rotation;
    public float smoothSpeed = 0.125f;

    void Start()
    {
        transform.rotation = Quaternion.Euler(rotation);
    }

    void LateUpdate()
    {
        // Make sure the target is assigned
        if (target != null)
        {
            // Calculate desired position with offset
            Vector3 desiredPosition = target.position + offset;

            // Smooth the camera movement to avoid jerky movement
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera position
            transform.position = smoothedPosition;
        }
    }
}