using UnityEngine;
using UnityEngine.UI;

public class GameSpeedSlider : MonoBehaviour
{
    private Slider slider;

    private void Start() {
        slider = GetComponent<Slider>();

        // Set the slider's initial value based on the MusicManager
        if (GameSpeedManager.Instance != null && slider != null) {
            // Convert decibel value back to linear (0 to 1)
            slider.value = GameSpeedManager.Instance.gameSpeed;
        } else {
            slider.value = 1f; // Default value
        }

        // Add listener to update volume when slider value changes
        slider.onValueChanged.AddListener(UpdateSpeed);
    }

    private void UpdateSpeed(float speed) {
        // Call MusicManager's SetVolume method
        if (GameSpeedManager.Instance != null) GameSpeedManager.Instance.SetGameSpeed(speed);
    }
}
