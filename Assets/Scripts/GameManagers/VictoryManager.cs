using System.Collections;
using System.Collections.Generic;
using EnemyAndTowers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{

    PlayerHealthManager playerHealthManager;
    WaveManager waveManager;
    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private GameObject window;
    [SerializeField] private GameObject losedow;
    [SerializeField] private GameObject grayout;
    // Start is called before the first frame update
    void Start()
    {
      waveManager = FindObjectOfType<WaveManager>();
      playerHealthManager = FindObjectOfType<PlayerHealthManager>();
      victoryText.text = "";
      grayout.SetActive(false);
      window.SetActive(false);
      losedow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
      // this could potentially be checked on enemy attack or damage take for more efficiency
      determineVictory();
    }

    public void LoadRewardScreen() {
      SceneManager.LoadScene("Rewards");
    }

    public void LoadMainMenu() {
      SceneManager.LoadScene("MainMenu");
    }

    private void Win() {
      victoryText.text = "Victory!";
      victoryText.color = new Color(18, 28, 95);
      window.SetActive(true);
      StopGame();
    }

    private void Lose() {
      victoryText.text = "Defeat";
      victoryText.color = new Color(53, 4, 4);
      losedow.SetActive(true);
      StopGame();
    }

    private void StopGame() {
      grayout.SetActive(true);
      waveManager.Stop();
      CardMovement[] moveScripts = FindObjectsByType<CardMovement>(FindObjectsSortMode.None);
      foreach (CardMovement moveScript in moveScripts) {
        moveScript.Reset();
      } 
      GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
      foreach (GameObject enemy in enemies) {
        enemy.GetComponent<Enemy>().Stop();
      }
    }

    private void determineVictory() {
      if (playerHealthManager.GetPlayerHealth() <= 0) {
          Lose();
        }
      if (waveManager.GetLastEnemySpawned()) {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) {
          Win();
        }
      }
    }
}
