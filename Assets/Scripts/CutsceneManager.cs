using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public GameObject portal;          
    public GameObject player;          
    public Animator playerAnimator;    
    public GameObject playButton;      
    public GameObject title;           // Added reference for Title
    public float portalDelay = 1.5f;   
    public float soulSuckDuration = 2f; 

    public void StartCutscene()
    {
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        // Step 1: Hide the Play button and Title
        playButton.SetActive(false);
        title.SetActive(false);

        // Step 2: Delay before the portal appears
        yield return new WaitForSeconds(portalDelay);

        // Step 3: Activate the portal
        portal.SetActive(true);

        // Step 4: Wait briefly before starting the player's soul-suck effect
        yield return new WaitForSeconds(1f);

        // Step 5: Trigger the soul-sucking animation
        playerAnimator.SetTrigger("SoulSucked");

        // Step 6: Wait for the soul-sucking effect to finish
        yield return new WaitForSeconds(soulSuckDuration);

        // Step 7: Start the main game (Load Level 1)
        SceneManager.LoadScene("Level1");
    }
}
