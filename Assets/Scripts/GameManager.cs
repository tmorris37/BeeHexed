using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int wave = 0;
    [SerializeField] private int startingHandSize = 3;
    [SerializeField] private int startingNectar = 5;
    // [SerializeField] private int cardDrawsPerWave = 3;

    // Start is called before the first frame update
    void Start()
    {
      DrawPileManager drawPileManager = FindObjectOfType<DrawPileManager>();
      HandManager handManager = FindObjectOfType<HandManager>();
      NectarManager nectarManager = FindObjectOfType<NectarManager>();
      if (drawPileManager == null || handManager == null) {
        Debug.Log("null manager");
      }
      for (int i = 0; i < startingHandSize; i++) {
        drawPileManager.DrawCard(handManager);
      } 
      nectarManager.SetNectar(startingNectar);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
