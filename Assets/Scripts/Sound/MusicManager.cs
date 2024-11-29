using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    public AudioMixer musicMixer;
    private AudioSource audioSource;
    public float volume;
    public AudioClip mainMenuClip;
    public AudioClip newGameClip;
    public AudioClip inGameClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
    }

    public void Start()
    {
        SetVolume(volume);
        PlayMainMenuMusic();
    }

    public void SetVolume(float volume)
    {
        this.volume = volume;
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void PlayMainMenuMusic()
    {
        PlayMusic(mainMenuClip);
    }

    public void PlayInGameMusic()
    {
        PlayMusic(inGameClip);
    }

    public void PlayNewGameMusic()
    {
        PlayMusic(newGameClip);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return; // Avoid restarting the same audio
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
