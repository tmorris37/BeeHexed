using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;

public class MainMenu : MonoBehaviour 
{ 

    // Switches Scene to input Scene, sceneName
    public void GoToScene(string sceneName) 
    {
        DeckSelected.selectedDeck = "None";
        SceneManager.LoadScene(sceneName);
    }

    // Quits the Game
    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Application has quit");
    }

}
