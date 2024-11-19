using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private AudioSource audioSource;

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
    }

    private void Start()
    {
        PlayMainMenuAudio();
    }

    public void PlayMainMenuAudio()
    {
        PlayAudio(mainMenuClip);
    }

    public void PlayInGameAudio()
    {
        PlayAudio(inGameClip);
    }

    public void PlayNewGameAudio()
    {
        PlayAudio(newGameClip);
    }

    private void PlayAudio(AudioClip clip)
    {
        if (audioSource.clip == clip) return; // Avoid restarting the same audio
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
