using UnityEngine;

public class PlatformMovementH : MonoBehaviour
{
    public float floatRange = 2f;     // Total distance left/right
    public float floatSpeed = 2f;     // Speed of movement

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newX = startPos.x + Mathf.Sin(Time.time * floatSpeed) * (floatRange / 2f);
        transform.position = new Vector3(newX, startPos.y, startPos.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Make player a child of platform
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Unparent the player when they jump off
            collision.collider.transform.SetParent(null);
        }
    }
}
