using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Map;
public class MapScript : MonoBehaviour
{
    MapManager mapManager;
    // On Scene Loaded, call generate for nodemap
    private void Awake() {
        //QuestionDialogueScript.Instance.ShowQuestion("Are you sure?", () => {
            // Do nothing for now, update this to update the Map
        //}, () => {
            // Do nothing (hides automatically)
        //});
    }

        // Switches Scene to input Scene, sceneName
        public void GoToScene(string sceneName)
        {
            if (MusicManager.Instance != null)
            {
                if (sceneName == "MainMenu")
                {
                    MusicManager.Instance.PlayMainMenuMusic();
                }
                else
                {
                    MusicManager.Instance.PlayInGameMusic();
                }
            }
            SceneManager.LoadScene(sceneName);
    }
}

