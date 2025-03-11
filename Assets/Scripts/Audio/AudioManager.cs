using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource bgMusicSource;
    public AudioSource sfxSource;

    public AudioClip mainMenuMusic;
    public AudioClip ominousMusic;
    public AudioClip soulSuckSFX;

    void Awake()
    {
        Debug.Log("Awake called on AudioManager");

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("AudioManager Initialized and Marked as DontDestroyOnLoad");
        }
        else
        {
            Debug.LogWarning("Duplicate AudioManager Destroyed");
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (bgMusicSource.clip != clip)
        {
            bgMusicSource.clip = clip;
            bgMusicSource.Play();
        }
    }

    public void StopMusic()
    {
        bgMusicSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
