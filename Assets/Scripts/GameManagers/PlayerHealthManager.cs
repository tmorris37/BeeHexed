using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Codice.CM.Common.Serialization;

public class PlayerHealthManager : MonoBehaviour
{

    public TextMeshProUGUI playerHealthText;
    private int currentPlayerHealth = 100;
    //GameObject playerHealth;
    void Awake()
    {
        //playerHealth = GameObject.FindWithTag("PlayerHealth");
    }

    public void TakeDamage(int value) {
        if (currentPlayerHealth <= value) {
            currentPlayerHealth = 0;
        } else {
            currentPlayerHealth -= value;
        }
    }

    public int GetPlayerHealth() {
      return currentPlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //currentPlayerHealth = (int)Math.Ceiling(playerHealth.GetComponent<FloatingHealthBar>().GetValue());
        playerHealthText.text = currentPlayerHealth.ToString();
    }
}
