using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour 
{

   // Switches Scene to input Scene, sceneName
    public void GoToScene(string sceneName) 
    {
        if (MusicManager.Instance != null)
            {
                if (sceneName == "Map")
                {
                    MusicManager.Instance.PlayNewGameMusic();
                }
            }
        SceneManager.LoadScene(sceneName);
    }

    // Quits the Game
    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Application has quit");
    }
}
