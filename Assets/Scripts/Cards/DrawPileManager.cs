using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using Newtonsoft.Json;
using System.IO;
using UnityEditor.Experimental.GraphView;


public class DrawPileManager : MonoBehaviour
{

    // lets you use a custom deck rather than the starting deck
    [SerializeField] public bool DEVELOPER_MODE = false;
    [SerializeField] bool DEBUG_MODE = false;
    // add the cards you want via the inspector, ensure DEVELOPER_MODE is on
    [SerializeField] private List<Card> customDeck;

    public List<Card> deck = new();

    [SerializeField] int handLimit = 8;
    
    private string savePath = Paths.savePath;
    private HandManager handManager;
    private DiscardManager discardManager;

    public TextMeshProUGUI drawPileCounter;


    public void Awake() {
      handManager = FindObjectOfType<HandManager>();
      //
      if (DEVELOPER_MODE) {
        WriteDeckToFile(customDeck);
      }
      getDeckFromFile();
      generateDrawPile(deck);
    }
    

  private void WriteDeckToFile(List<Card> deck)
  {
    List<string> cardNames = new();
    foreach (Card card in deck){
      cardNames.Add("Cards/" + card.cardName);
    }
    PlayerData playerData = new PlayerData
    {
        themeColor = BasicColor.ConvertToBasicColor(Color.black),
        deckName = "DevDeck",
        cardPaths = cardNames
    };
    string jsonSave = JsonConvert.SerializeObject(playerData);
    File.WriteAllText(savePath, jsonSave);
    if (DEBUG_MODE)
      Debug.Log("Written Save" + jsonSave);
  }


  void getDeckFromFile() {
      deck.Clear();
      string JSONPlainText = File.ReadAllText(savePath);
      if (DEBUG_MODE) {
        Debug.Log("Read deck" + JSONPlainText);
      }
      PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(JSONPlainText);
      IList<string> cardPaths = playerData.cardPaths;
      // add each card once
      foreach (string path in cardPaths) {
        deck.Add(Resources.Load<Card>(path));
      }
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

    // DEVELOPER ONLY METHODS
    public void DEV_setDeckPath(string path) {
      if (DEVELOPER_MODE) {
        savePath = path;
      }
    }

    public void DEV_setCustomDeck(List<Card> custDeck) {
      if (DEVELOPER_MODE) {
        customDeck = custDeck;
      }
    }

}
