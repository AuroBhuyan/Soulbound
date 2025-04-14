using UnityEngine;

public class SunMovement : MonoBehaviour
{
    public Transform player;     // Reference to the player's Transform
    public float followFactor = 1f;  // Set to 1 for exact match, lower for parallax effect

    private Vector3 offset;

    void Start()
    {
        // Store initial offset between sun and player
        offset = transform.position - player.position;
    }

    void Update()
    {
        // Only follow X-axis movement of the player
        transform.position = new Vector3(player.position.x + offset.x * followFactor, transform.position.y, transform.position.z);
    }
}
