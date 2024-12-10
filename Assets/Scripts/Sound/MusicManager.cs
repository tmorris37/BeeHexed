using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    public AudioMixer musicMixer;
    private AudioSource audioSource;
    private AudioLowPassFilter lowPassFilter;
    public float volume;
    public AudioClip mainMenuClip;
    public AudioClip newGameClip;
    public AudioClip inGameClip;
    public float normalCutoffFrequency = 22000f; // Normal playback
    public float pausedCutoffFrequency = 500f;  // Muffled effect
    public float transitionDuration = 0.1f;     // Transition time in seconds

    private Coroutine transitionCoroutine;
    

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
        if (!audioSource)
        {
            Debug.LogError("No AudioSource found on MusicManager.");
            return;
        }

        lowPassFilter = GetComponent<AudioLowPassFilter>();
        if (!lowPassFilter)
        {
            // Add a Low Pass Filter if not present
            lowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
        }
        audioSource.volume = volume;
        lowPassFilter.cutoffFrequency = normalCutoffFrequency;
    }

    public void Start()
    {
        SetVolume(volume);
        PlayMainMenuMusic();
    }

    public void SetVolume(float volume)
    {
        this.volume = volume;
        // audioSource.volume = volume;
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

    public void ApplyPauseFilter()
    {
        ApplyLowPassFilter(pausedCutoffFrequency);
    }

    public void RemovePauseFilter()
    {
        ApplyLowPassFilter(normalCutoffFrequency);
    }

    private void ApplyLowPassFilter(float targetCutoffFrequency)
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }

        transitionCoroutine = StartCoroutine(TransitionLowPassFilter(targetCutoffFrequency));
    }

    private System.Collections.IEnumerator TransitionLowPassFilter(float targetCutoffFrequency)
    {
        float startFrequency = lowPassFilter.cutoffFrequency;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaled time to ignore Time.timeScale
            lowPassFilter.cutoffFrequency = Mathf.Lerp(startFrequency, targetCutoffFrequency, elapsed / transitionDuration);
            yield return null;
        }

        lowPassFilter.cutoffFrequency = targetCutoffFrequency;
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
