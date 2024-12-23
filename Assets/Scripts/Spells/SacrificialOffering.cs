using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SacrificialOffering : CardDrawingSpell {
    PlayerHealthManager playerHealth;
    void Awake() {
        playerHealth = FindObjectOfType<PlayerHealthManager>();
    }

    // Start is called before the first frame update
    void Start() {
        DrawCards(3);
        playerHealth.TakeDamage(25);
    }
}
