using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab;  // Assign your Bird prefab here
    public float spawnInterval = 4f;
    public float minY = 2f;
    public float maxY = 6f;
    public float birdSpeed = 2f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnBird();
            timer = 0f;
        }
    }

    void SpawnBird()
    {
        bool fromLeft = Random.value > 0.5f;
        float y = Random.Range(minY, maxY);

        Vector3 spawnPos = fromLeft ?
            new Vector3(Camera.main.transform.position.x - 12f, y, 0) :
            new Vector3(Camera.main.transform.position.x + 12f, y, 0);

        GameObject bird = Instantiate(bi rdPrefab, spawnPos, Quaternion.identity);

        // Flip sprite if flying left
        if (!fromLeft)
        {
            Vector3 scale = bird.transform.localScale;
            scale.x *= -1;
            bird.transform.localScale = scale;
        }

        bird.AddComponent<FlyingBird>().Initialize(fromLeft ? Vector2.right : Vector2.left, birdSpeed);
    }
}
