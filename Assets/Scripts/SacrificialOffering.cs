using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SacrificialOffering : MonoBehaviour
{   
    DrawPileManager drawPileManager;
    HandManager handManager;
    GameObject playerHealth;
    //Slider playerHealth;
    void Awake() {
      handManager = FindObjectOfType<HandManager>();
      drawPileManager = FindObjectOfType<DrawPileManager>();
      playerHealth = GameObject.FindWithTag("PlayerHealth");
      //playerHealth = FindFirstObjectByType<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    { 
        // draw three cards, take 5 damage
        for (int i = 0; i < 3; i++) {
          drawPileManager.DrawCard(handManager);
        } 
        playerHealth.GetComponent<FloatingHealthBar>().Damage(0.25f);
        //playerHealth.value -= 0.25f;
    }

}
