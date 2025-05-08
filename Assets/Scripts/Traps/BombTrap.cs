using UnityEngine;
using System.Collections;

public class BombTrap : MonoBehaviour
{
    [Header("Explosion Settings")]
    [Tooltip("Time to wait before destroying the bomb object after explosion")]
    [SerializeField] private float destroyDelay = 0.7f;

    [Tooltip("Audio clip to play when bomb explodes")]
    [SerializeField] private AudioClip explosionSound;

    [Tooltip("Volume of the explosion sound")]
    [Range(0f, 1f)]
    [SerializeField] private float explosionVolume = 0.7f;

    // References
    private Animator animator;
    private AudioSource audioSource;
    private CircleCollider2D bombCollider;
    private bool hasExploded = false;

    // Animation parameter name
    private const string TRIGGER_EXPLODE = "BombBlast";

    void Awake()
    {
        // Get required components
        animator = GetComponent<Animator>();
        bombCollider = GetComponent<CircleCollider2D>();

        // Add AudioSource if not already present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && explosionSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with the player and bomb hasn't exploded yet
        if (collision.CompareTag("Player") && !hasExploded)
        {
            ExplodeBomb(collision.gameObject);
        }
    }

    private void ExplodeBomb(GameObject player)
    {
        hasExploded = true;

        // Trigger the bomb explosion animation
        if (animator != null)
        {
            animator.SetTrigger(TRIGGER_EXPLODE);
        }

        // Play explosion sound if available
        if (audioSource != null && explosionSound != null)
        {
            audioSource.clip = explosionSound;
            audioSource.volume = explosionVolume;
            audioSource.Play();
        }

        // Kill the player
        KillPlayer(player);

        // Destroy the bomb after animation completes
        StartCoroutine(DestroyAfterDelay());
    }

    private void KillPlayer(GameObject player)
    {
        // Try to get player health component if it exists
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(int.MaxValue); // Instant kill
        }
        else
        {
            // If no health component exists, let's implement a fallback
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                // Disable player movement
                playerMovement.enabled = false;
            }

            // Try to trigger death animation on player
            Animator playerAnim = player.GetComponent<Animator>();
            if (playerAnim != null)
            {
                playerAnim.SetTrigger("Die");
            }

            // Disable player collider to prevent further interactions
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                playerCollider.enabled = false;
            }

            // Optionally, restart level after a delay
            StartCoroutine(RestartLevelAfterDelay(2f));
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        // Disable the collider so it doesn't trigger again
        if (bombCollider != null)
        {
            bombCollider.enabled = false;
        }

        // Wait for animation to finish
        yield return new WaitForSeconds(destroyDelay);

        // Destroy the bomb GameObject
        Destroy(gameObject);
    }

    private IEnumerator RestartLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
