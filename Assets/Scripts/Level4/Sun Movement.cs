using UnityEngine;

public class SunMovement : MonoBehaviour
{
    public Transform cameraTransform;     // Main camera's transform
    public float followFactor = 1f;       // 1 = exact follow, 0.5 = parallax effect
    public float smoothSpeed = 2f;        // Higher = faster catch-up

    private Vector3 initialOffset;

    void Start()
    {
        // Store the initial offset between the sun and the camera
        initialOffset = transform.position - cameraTransform.position;
    }

    void Update()
    {
        // Target X position based on camera and initial offset
        float targetX = cameraTransform.position.x + initialOffset.x * followFactor;

        // Smoothly interpolate current position to target position
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }
}
