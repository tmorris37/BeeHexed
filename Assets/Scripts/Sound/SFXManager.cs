using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    public AudioMixer sfxMixer;
    public float volume;
    public AudioClip Beem;
    public AudioClip Pulse;
    public AudioClip Explosion;
    public AudioClip TossBomb;
    public AudioClip FireballCast;
    public AudioClip FireballExplode;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        SetVolume(volume);
    }

    public void SetVolume(float volume)
    {
        this.volume = volume;
        sfxMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }


    public void PlayBeem()
    {
        PlaySFX(Beem);
    }

    public void PlayToss()
    {
        PlaySFX(TossBomb);
    }

    public void PlayExplosion()
    {
        PlaySFX(Explosion);
    }

    public void PlayPulse()
    {
        PlaySFX(Pulse);
    }

    public void PlayFireballCast()
    {
        PlaySFX(FireballCast);
    }

    public void PlayFireballExplode()
    {
        PlaySFX(FireballExplode);
    }

    private void PlaySFX(AudioClip clip)
    {
        // Create a new GameObject with an AudioSource to play the clip
        GameObject sfxObject = new GameObject("SFX_" + clip.name);
        AudioSource audioSource = sfxObject.AddComponent<AudioSource>();

        // Set up the AudioSource
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = sfxMixer.FindMatchingGroups("SFX")[0]; // Assuming you have an "SFX" group
        audioSource.volume = 1f;
        audioSource.Play();

        // Destroy the GameObject after the clip finishes playing
        Destroy(sfxObject, clip.length);
    }
}
