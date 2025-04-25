using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private float delayBeforeLoad = 1f; // Optional delay

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the exit. Loading next level...");
            StartCoroutine(LoadNextLevel());
        }
    }

    private System.Collections.IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(delayBeforeLoad);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // If next scene exists
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels. Maybe return to main menu?");
            SceneManager.LoadScene(0); // Go back to menu if final level is done
        }
    }

}
