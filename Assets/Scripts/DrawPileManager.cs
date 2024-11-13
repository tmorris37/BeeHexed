using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class DrawPileManager : MonoBehaviour
{

    public List<Card> deck = new();

     //private int currIndex;

    [SerializeField] int handLimit = 8;

    private HandManager handManager;
    private DiscardManager discardManager;

    public TextMeshProUGUI drawPileCounter;


    public void Awake() {
      handManager = FindObjectOfType<HandManager>();
      // load all cards from resources
      Card[] cardList = Resources.LoadAll<Card>("Cards");
      // fill deck with cards, twice for no
      deck.AddRange(cardList);
      deck.AddRange(cardList);
      generateDrawPile(deck);
    }

    void Start() {
      
    }

    public void DrawCard(HandManager handManager) {
      if (deck.Count == 0) {
        ShuffleGraveIntoDeck();
      }
      if (deck.Count == 0 || handManager.handSize >= handLimit) {
        return;
      } else {
        Card nextCard = deck[0];
        handManager.AddToHand(nextCard);
        deck.RemoveAt(0);
        // handManager.handSize++;
        drawPileCounter.text = deck.Count.ToString();
      }
    }

  private void ShuffleGraveIntoDeck()
  {
    if (discardManager == null) {
      discardManager = FindObjectOfType<DiscardManager>();
    }
    if (discardManager.graveyardSize == 0) {
      return;
    }
    deck = discardManager.drawAllGraveyard();
    Shuffle();
    // currIndex = 0;
  }

  // Fisher-Yates Shuffle algorithm
  public void Shuffle() {
      System.Random randy = new System.Random();
      List<Card> shuffled = new List<Card>();
      while (deck.Count > 0) {
        int k = randy.Next(0, deck.Count - 1);
        shuffled.Add(deck[k]);
        deck.Remove(deck[k]);
      }
      deck = new List<Card>(shuffled);
    }


    public void generateDrawPile(List<Card> cards) {
      deck = new List<Card>(cards);
      Shuffle();
      drawPileCounter.text = deck.Count.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
