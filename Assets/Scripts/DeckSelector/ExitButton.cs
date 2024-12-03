using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    // Returns to the main menu, discarding the run
    public void ExitToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void BackToDeckSelect() {
        SceneManager.LoadScene("DeckSelect");
    }
}
