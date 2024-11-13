using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeScript : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider volumeSlider;

    // on start sets volume to slider value
    private void Start()
    {
        SetVolume();
    }

    // sets volume through float value in slider
    public void SetVolume()
    {
        float volume = volumeSlider.value;
        myMixer.SetFloat("volumeMixer", Mathf.Log10(volume) * 20);
    }

    // TODO add load volume function to save previous volume level
}
