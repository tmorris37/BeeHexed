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
            SceneManager.LoadScene(sceneName);
        }
    }
}
