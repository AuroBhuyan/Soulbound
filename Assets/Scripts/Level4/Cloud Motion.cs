using UnityEngine;

public class CloudMotion : MonoBehaviour
{
    public GameObject[] clouds;      // Assign your cloud GameObjects in the Inspector
    public float speed = 1f;

    private float cloudWidth;

    void Start()
    {
        if (clouds.Length == 0)
        {
            Debug.LogError("No clouds assigned!");
            return;
        }

        // Assume all clouds are same width
        cloudWidth = clouds[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        foreach (GameObject cloud in clouds)
        {
            // Move each cloud to the left
            cloud.transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        // Loop clouds
        for (int i = 0; i < clouds.Length; i++)
        {
            GameObject cloud = clouds[i];

            // If cloud goes off-screen to the left
            if (cloud.transform.position.x < Camera.main.transform.position.x - (cloudWidth / 2) - 10f)
            {
                // Find the rightmost cloud
                GameObject rightmost = FindRightmostCloud();
                float newX = rightmost.transform.position.x + cloudWidth;

                // Move current cloud to the right end
                cloud.transform.position = new Vector2(newX, cloud.transform.position.y);
            }
        }
    }

    GameObject FindRightmostCloud()
    {
        GameObject rightmost = clouds[0];

        foreach (GameObject cloud in clouds)
        {
            if (cloud.transform.position.x > rightmost.transform.position.x)
            {
                rightmost = cloud;
            }
        }

        return rightmost;
    }
}
