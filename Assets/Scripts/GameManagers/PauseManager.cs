using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject pauseMenu;
    public bool DEBUG_MODE = false;
    private HandManager handManager;
    // Start is called before the first frame update
    void Start() {
        handManager = FindObjectOfType<HandManager>();
    }

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
                handManager.SetHandLayer(-1);
            } else {
                MusicManager.Instance.RemovePauseFilter();
                handManager.SetHandLayer(1);
            }
        }
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : GameSpeedManager.Instance.gameSpeed;
        if (DEBUG_MODE) Debug.Log("Pause Toggled:" + isPaused);
    }
}
