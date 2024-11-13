using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Settings : MonoBehaviour
{

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
}
