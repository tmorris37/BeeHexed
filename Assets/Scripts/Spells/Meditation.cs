using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meditation : MonoBehaviour
{
    void Start()
    {
      DrawPileManager drawPileManager = FindObjectOfType<DrawPileManager>();
      HandManager handManager = FindObjectOfType<HandManager>();
      drawPileManager.DrawCard(handManager);
      drawPileManager.DrawCard(handManager);
      Destroy(gameObject);
    }
}
