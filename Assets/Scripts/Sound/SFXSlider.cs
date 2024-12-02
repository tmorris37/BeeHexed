using UnityEngine;
using UnityEngine.UI;

public class SFXSlider : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();

        // Initialize the slider's value based on the SFXManager
        if (SFXManager.Instance != null)
        {
            float volume;
            // Check if we can get the SFXVolume from the AudioMixer
            if (SFXManager.Instance.sfxMixer.GetFloat("SFXVolume", out volume))
            {
                // Convert decibel value back to linear (0 to 1)
                slider.value = Mathf.Pow(10, volume / 20);
            }
            else
            {
                slider.value = 0.5f; // Default value if unable to get volume
            }
        }
        else
        {
            slider.value = 0.5f; // Default value if SFXManager is not available
        }

        // Add listener to update volume when slider value changes
        slider.onValueChanged.AddListener(UpdateVolume);
    }

    private void UpdateVolume(float value)
    {
        // Call MusicManager's SetVolume method
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.SetVolume(value);
        }
    }
}
