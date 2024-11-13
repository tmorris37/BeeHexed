using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MapScript : MonoBehaviour
{
    // Switches Scene to input Scene, sceneName
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
