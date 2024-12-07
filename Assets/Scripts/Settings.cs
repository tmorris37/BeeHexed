using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    public string settingSavePath = "Assets/Deck/Settings.json";
    [SerializeField] private Slider playStyleSlider;
    [SerializeField] private Color clickColor;
    [SerializeField] private Color dragColor;
    void Awake() {
        LoadSettingsFromFile();
        ChangeSliderColor();
    }

    private void LoadSettingsFromFile()
    {
        string json = File.ReadAllText(settingSavePath);
        CardPlaystyle playstyle = JsonConvert.DeserializeObject<CardPlaystyle>(json);
        if (playstyle == CardPlaystyle.Clicking) {
            playStyleSlider.value = 0;
        } else {
            playStyleSlider.value = 1;
        }
    }

    // Sets quality of 
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // Brings User to previous scene
    public void GoBack(string path)
    {
        // Hard Coded to bring back to MainMenu
        SceneManager.LoadScene("MainMenu");
    }

    // Sets card playstyle preferenes
    public void SetPlayStylePreference() {
        // 0 is click, 1 is drag
        // converts 0 to 1 and 1 to 0 (AKA toggles)
        playStyleSlider.value = playStyleSlider.value + 1 + playStyleSlider.value * -2;
        SavePlayStylePreferences(playStyleSlider.value);
    }

    public void ChangeSliderColor() {
        if (playStyleSlider.value == 0) {
            playStyleSlider.handleRect.GetComponent<Image>().color = clickColor; 
        } else {
            playStyleSlider.handleRect.GetComponent<Image>().color = dragColor;
        }
    }

    private void SavePlayStylePreferences(float value)
    {
        string json;
        if (value == 0) {
            json = JsonConvert.SerializeObject(CardPlaystyle.Clicking);
        } else {
            json = JsonConvert.SerializeObject(CardPlaystyle.Dragging);
        }
        File.WriteAllText(settingSavePath, json);
    }
}

public enum CardPlaystyle {
    Clicking,
    Dragging
}
