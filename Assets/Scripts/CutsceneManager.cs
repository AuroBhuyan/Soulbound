using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public GameObject portal;
    public Animator portalAnimator;
    public GameObject playButton;
    public GameObject title;
    public Animator playerAnimator;
    
    public float idleDuration = 3.0f;
    public float portalAppearDuration = 3.0f;
    public float soulSuckTriggerDelay = 2.0f; 
    public float soulSuckDuration = 2.5f;

    public void StartCutscene()
    {
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        Debug.Log("Cutscene Started");

        playButton.SetActive(false);
        title.SetActive(false);

        Debug.Log("Waiting for idle duration...");
        yield return new WaitForSeconds(idleDuration);

        Debug.Log("Portal Appearing");
        portal.SetActive(true);
        portalAnimator.SetTrigger("Appear");

        Debug.Log("Waiting before triggering SoulSucked animation...");
        yield return new WaitForSeconds(soulSuckTriggerDelay);

        Debug.Log("Playing SoulSucked animation");
        playerAnimator.SetTrigger("SoulSucked");

        yield return new WaitUntil(() => portalAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        yield return new WaitUntil(() => playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        Debug.Log("Waiting extra pause before scene transition...");
        yield return new WaitForSeconds(2.0f);

        Debug.Log("Loading Level 1...");
        SceneManager.LoadScene("Level1");
    }
}
