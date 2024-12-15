using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject pauseMenu;
    public bool DEBUG_MODE = false;
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    public void LoadMainMenu() {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void TogglePause() {
        isPaused = !isPaused;
        if (MusicManager.Instance != null)
        {
            if (isPaused) {
                MusicManager.Instance.ApplyPauseFilter();
            } else {
                MusicManager.Instance.RemovePauseFilter();
            }
        }
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        if (DEBUG_MODE) Debug.Log("Pause Toggled:" + isPaused);
    }
}
