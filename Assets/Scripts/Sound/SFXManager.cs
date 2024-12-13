using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    public AudioMixer sfxMixer;
    public float volume;
    private AudioSource currentPlaying;
    public AudioClip Beem;
    public AudioClip Pulse;
    public AudioClip Explosion;
    public AudioClip TossBomb;
    public AudioClip Fireball1;
    public AudioClip Fireball2;
    public AudioClip ElectricGust;
    public AudioClip MechanicalArmor;
    public AudioClip Fanfare;
    public AudioClip FireballCast;
    public AudioClip FireballExplode;
    public AudioClip Blizzard;
    public AudioClip buttonClick;
    public AudioClip towerPlace;

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

    public void PlayFire1()
    {
        PlaySFX(Fireball1);
    }

    public void PlayFire2()
    {
        PlaySFX(Fireball2);
    }
    public void PlayFanfare()
    {
        PlaySFX(Fanfare);
    }
    public void PlayElectric()
    {
        PlaySFX(ElectricGust);
    }
    public void PlayMechanical()
    {
        PlaySFX(MechanicalArmor);
    }
    public void PlayBlizzard() {
        PlaySFX(Blizzard);
    }
    public void PlayButtonClick()
    {
        PlaySFX(buttonClick);
    }
    public void PlayTowerPlace()
    {
        PlaySFX(towerPlace);
    }

    public void stopCurrentSFX() {
        if (currentPlaying != null) {
            // Debug.Log("Stopping " + currentPlaying);
            currentPlaying.Stop();
        }
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
        DontDestroyOnLoad(audioSource);

        // Set up the AudioSource
        
        currentPlaying = audioSource;
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = sfxMixer.FindMatchingGroups("SFX")[0]; // Assuming you have an "SFX" group
        audioSource.volume = 1f;
        audioSource.Play();

        // Destroy the GameObject after the clip finishes playing
        Destroy(sfxObject, clip.length);
    }
}
