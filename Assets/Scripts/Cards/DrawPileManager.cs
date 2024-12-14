using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;


public class DrawPileManager : MonoBehaviour
{

    // lets you use a custom deck rather than the starting deck
    [SerializeField] public bool DEVELOPER_MODE = false;
    [SerializeField] bool DEBUG_MODE = false;
    // add the cards you want via the inspector, ensure DEVELOPER_MODE is on
    [SerializeField] private List<Card> customDeck;
    [SerializeField] private Image testCardBack;

    public List<Card> deck = new();

    [SerializeField] int handLimit = 8;
    private DiscardManager discardManager;
    private DeckAnimator deckAnimator;
    public TextMeshProUGUI drawPileCounter;


    public void Awake() {
        deckAnimator = FindObjectOfType<DeckAnimator>();
        if (DEVELOPER_MODE) {
            StoreDeck(customDeck);
        }
        GetStoredDeck();
        generateDrawPile(deck);
    }
    

    private void StoreDeck(List<Card> deck)
    {
        List<string> cardNames = new();
        foreach (Card card in deck){
            cardNames.Add("Cards/" + card.cardName);
        }
        PlayerData.themeColor = BasicColor.ConvertToBasicColor(Color.black);
        PlayerData.deckName = "DevDeck";
        PlayerData.cardPaths = cardNames;
        PlayerData.cardBack = testCardBack;
    }


  void GetStoredDeck() {
      deck.Clear();
      IList<string> cardPaths = PlayerData.cardPaths;
      // add each card once
      foreach (string path in cardPaths) {
        deck.Add(Resources.Load<Card>(path));
      }
    }

    public void DrawCard(HandManager handManager) {
        if (deck.Count == 0) {
            ShuffleGraveIntoDeck();
        }
        if (deck.Count != 0 && handManager.handSize < handLimit) {
            Card nextCard = deck[0];
            deck.RemoveAt(0);
            handManager.AddToHand(nextCard);
        }
        drawPileCounter.text = deck.Count.ToString();
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
    if (deckAnimator != null) {
        deckAnimator.Shuffle();
    }    
    Shuffle();
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

    // DEVELOPER ONLY METHODS

    public void DEV_setCustomDeck(List<Card> custDeck) {
      if (DEVELOPER_MODE) {
        customDeck = custDeck;
      }
    }

}
