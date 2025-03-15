using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgMusicSource;
    public AudioSource sfxSource;
    public AudioSource cutsceneSource;

    [Header("Audio Clips")]
    public AudioClip bgMusic;
    public AudioClip[] sfxClips;
    public AudioClip[] cutsceneClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Assign AudioSources from child GameObjects
        bgMusicSource = transform.Find("BGMusic").GetComponent<AudioSource>();
        sfxSource = transform.Find("SFX").GetComponent<AudioSource>();
        cutsceneSource = transform.Find("CutsceneAudio").GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayBGMusic();
    }

    public void PlayBGMusic()
    {
        if (bgMusicSource && bgMusic)
        {
            bgMusicSource.clip = bgMusic;
            bgMusicSource.loop = true;
            bgMusicSource.Play();
        }
    }

    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfxClips.Length)
        {
            sfxSource.PlayOneShot(sfxClips[index]);
        }
    }

    public void PlayCutsceneAudio(int index)
    {
        if (index >= 0 && index < cutsceneClips.Length)
        {
            cutsceneSource.clip = cutsceneClips[index];
            cutsceneSource.Play();
        }
    }

    public void StopCutsceneAudio()
    {
        if (cutsceneSource.isPlaying)
        {
            cutsceneSource.Stop();
        }
    }
}
