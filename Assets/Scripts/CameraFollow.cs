using UnityEngine;

public class CameraFollowDeadZone : MonoBehaviour
{
    [Header("References")]
    public Transform player; // Drag your player GameObject here

    [Header("Camera Settings")]
    // Set the offset so that the camera sits at the proper distance/height relative to the player.
    public Vector3 offset = new Vector3(0f, 2f, -5f);

    // The dead zone dimensions in world units. Think of these as the total width/height of the zone.
    // The player will be allowed to move within a rectangle of size deadZone,
    // centered on the target position (player.position + offset), before the camera starts moving.
    public Vector2 deadZone = new Vector2(2f, 2f);

    // How quickly the camera moves toward the new position when the player exits the dead zone.
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (player == null)
            return;

        // Calculate the "ideal" target position for the camera.
        Vector3 targetPosition = player.position + offset;

        // Determine how far the current camera position is from that target.
        Vector3 difference = targetPosition - transform.position;
        Vector3 adjustment = Vector3.zero;

        // Check the horizontal (x) axis.
        if (Mathf.Abs(difference.x) > deadZone.x * 0.5f)
        {
            // Move the camera so that the player remains within the dead zone on the x-axis.
            float sign = Mathf.Sign(difference.x);
            adjustment.x = difference.x - sign * (deadZone.x * 0.5f);
        }

        // Check the vertical (y) axis.
        if (Mathf.Abs(difference.y) > deadZone.y * 0.5f)
        {
            // Move the camera so that the player remains within the dead zone on the y-axis.
            float sign = Mathf.Sign(difference.y);
            adjustment.y = difference.y - sign * (deadZone.y * 0.5f);
        }

        // For many third-person views you want to maintain the offset on the z-axis.
        adjustment.z = difference.z;

        // Compute the desired camera position by applying the adjustment.
        Vector3 desiredPosition = transform.position + adjustment;

        // Smoothly move the camera to the desired position.
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
