using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{

    PlayerHealthManager playerHealthManager;
    [SerializeField] private TextMeshProUGUI victoryText;
    // Start is called before the first frame update
    void Start()
    {
        playerHealthManager = FindObjectOfType<PlayerHealthManager>();
        victoryText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealthManager.GetPlayerHealth() <= 0) {
          Lose();
        }
    }

    public void Win() {
      victoryText.text = "You WIN!!!!";
      victoryText.color = Color.blue;
    }

    public void Lose() {
      victoryText.text = "You LOSE!!!";
          victoryText.color = Color.red;
    }
}
