using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int currentHealth;

    [Header("Events")]
    public UnityEvent onPlayerDeath;
    public UnityEvent onPlayerDamaged;

    private Animator animator;
    private PlayerMovement movement;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        // Trigger damage event
        onPlayerDamaged?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        // Trigger death animation if possible
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Disable player movement
        if (movement != null)
        {
            movement.enabled = false;
        }

        // Disable player collider
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Trigger death event
        onPlayerDeath?.Invoke();

        // Optionally restart level after a delay
        Invoke("RestartLevel", 2f);
    }

    private void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
