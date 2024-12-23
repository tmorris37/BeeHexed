using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerHealthManager : MonoBehaviour
{

    public TextMeshProUGUI playerHealthText;
    private int currentPlayerHealth = 100;
    //GameObject playerHealth;

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
    void Update() {
        playerHealthText.text = currentPlayerHealth.ToString();
    }
}
