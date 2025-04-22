using UnityEngine;

public class PlatformMovementV : MonoBehaviour
{
    public float floatRange = 2f;      // Total distance to move up and down
    public float floatSpeed = 2f;      // Speed of floating
    private Vector3 startPos;

    void Start()
    {
        // Store the starting position
        startPos = transform.position;
    }

    void Update()
    {
        // Calculate new Y position using sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * (floatRange / 2f);

        // Apply the new position
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
    
}
