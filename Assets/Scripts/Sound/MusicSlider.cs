using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    private Slider slider;

    private void Start() {
        slider = GetComponent<Slider>();

        // Set the slider's initial value based on the MusicManager
        float volume;
        if (MusicManager.Instance != null && MusicManager.Instance.musicMixer.GetFloat("MusicVolume", out volume)) {
            // Convert decibel value back to linear (0 to 1)
            slider.value = Mathf.Pow(10, volume / 20);
        } else {
            slider.value = 0.5f; // Default value
        }

        // Add listener to update volume when slider value changes
        slider.onValueChanged.AddListener(UpdateVolume);
    }

    private void UpdateVolume(float value) {
        // Call MusicManager's SetVolume method
        if (MusicManager.Instance != null) MusicManager.Instance.SetVolume(value);
    }
}
