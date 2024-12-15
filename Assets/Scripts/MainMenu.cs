using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;

public class MainMenu : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        if (MusicManager.Instance != null) {
            MusicManager.Instance.RemovePauseFilter();
            MusicManager.Instance.PlayMainMenuMusic();
        }
    }
    // Switches Scene to input Scene, sceneName
    public void GoToScene(string sceneName) {
        DeckSelected.selectedDeck = "None";
        SceneManager.LoadScene(sceneName);
    }

    // Quits the Game
    public void QuitApp() {
        Application.Quit();
        Debug.Log("Application has quit");
    }
}
