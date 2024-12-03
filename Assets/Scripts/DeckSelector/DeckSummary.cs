using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class DeckSummary : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private string savePath = "Assets/Deck/Save.json";
    [SerializeField] private string deckPath = "Assets/Deck/";
    [SerializeField] private bool DEBUG_MODE = false;
    private List<Card> deck;
    private string deckName;
    void Awake()
    {
        titleText = FindObjectOfType<TextMeshProUGUI>();
        deck = new();
    }

    void Start() {
        string jsonDeckName = File.ReadAllText(savePath);
        deckName = JsonConvert.DeserializeObject<string>(jsonDeckName);
        titleText.text = deckName;
        LoadDeck();
        DisplayDeck();
    }

    void LoadDeck() {
        string jsonDeck = File.ReadAllText(deckPath + deckName);
        IList<string> cardPaths = JsonConvert.DeserializeObject<List<string>>(jsonDeck);
        // add each card once
        foreach (string path in cardPaths) {
            deck.Add(Resources.Load<Card>(path));
        }
    }

    void DisplayDeck() {

    }
}
