using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused) {
            MusicManager.Instance.ApplyPauseFilter();
        } else {
            MusicManager.Instance.RemovePauseFilter();
        }
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        Debug.Log("Pause Toggled:" + isPaused);
    }
}
