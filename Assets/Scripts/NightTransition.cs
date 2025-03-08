using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour
{
    public Gradient dayNightGradient; // A gradient for smooth color transitions
    public float cycleDuration = 20f; // Time for a full cycle (day -> night -> day)
    private float timeElapsed = 0f;
    private SpriteRenderer[] sprites;

    void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        StartCoroutine(CycleDayNight());
    }

    IEnumerator CycleDayNight()
    {
        while (true)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.PingPong(timeElapsed / cycleDuration, 1f); // Loops between 0 and 1 smoothly

            // Get the color from gradient
            Color newColor = dayNightGradient.Evaluate(t);

            // Apply to all background sprites
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.color = newColor;
            }

            yield return null;
        }
    }
}
