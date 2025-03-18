using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public Transform target; // The player or object to follow
    public BoxCollider2D boundsCollider; // The collider defining camera limits

    private float minX, maxX, minY, maxY;
    private float camHalfWidth, camHalfHeight;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;

        // Get bounds from collider
        Bounds bounds = boundsCollider.bounds;
        minX = bounds.min.x + camHalfWidth;
        maxX = bounds.max.x - camHalfWidth;
        minY = bounds.min.y + camHalfHeight;
        maxY = bounds.max.y - camHalfHeight;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            float clampedX = Mathf.Clamp(target.position.x, minX, maxX);
            float clampedY = Mathf.Clamp(target.position.y, minY, maxY);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}
