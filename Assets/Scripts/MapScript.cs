using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Map {
    public class MapScript : MonoBehaviour
    {
        MapManager mapManager;
        // On Scene Loaded, call generate for nodemap
        private void OnSceneLoaded()
        {
            mapManager.GenerateNewMap();
        }

        // Switches Scene to input Scene, sceneName
        public void GoToScene(string sceneName)
        {
            if (MusicManager.Instance != null)
            {
                if (sceneName == "Level0")
                {
                    MusicManager.Instance.PlayInGameMusic();
                }
                else
                {
                    MusicManager.Instance.PlayMainMenuMusic();
                }
            }
            SceneManager.LoadScene(sceneName);
        }
    }
}
