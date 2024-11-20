using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    private AudioSource audioSource;

    public AudioClip Beem;
    public AudioClip Pulse;

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
        
    }

    public void PlayBeem()
    {
        PlaySFX(Beem);
    }

    public void PlayPulse()
    {
        PlaySFX(Pulse);
    }

    private void PlaySFX(AudioClip clip)
    {
        // if (audioSource.clip == clip) return; // Avoid restarting the same audio
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.Play();
    }
}
