using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class DeckSummary : MonoBehaviour {   
    public bool DEBUG_MODE = false;
    public bool DEVELOPER_MODE = false;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private int maxCardsInRow = 6;
    [SerializeField] private int maxRows = 2;
    [SerializeField] private float cardScaleFactor = 0.75f;
    [SerializeField] private int rowScaleFactor = -275;
    [SerializeField] private float verticalCardDisplayOffset = 75;
    public GameObject towerCardPrefab;
    public GameObject spellCardPrefab;
    private List<Card> deck;
    private RectTransform rectTransform;
    private string deckName;
    public int testWidth;
    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        titleText = FindObjectOfType<TextMeshProUGUI>();
        deck = new();
    }

    void Start() {
        string filepath = Application.persistentDataPath + "/DeckAssets";
        string jsonPlayerData = File.ReadAllText(filepath + "/Save.json");

        System.IO.Directory.CreateDirectory(filepath);

        PlayerData saveData = JsonConvert.DeserializeObject<PlayerData>(jsonPlayerData);
        deckName = saveData.deckName;
        titleText.text = deckName;
        titleText.color = saveData.themeColor;
        LoadDeck();
        DisplayDeck();
    }

    void LoadDeck() {
        string deckFilepath = "DeckAssets/" + deckName;
        string jsonDeck = Resources.Load<TextAsset>(deckFilepath).text;
        
        IList<string> cardPaths = JsonConvert.DeserializeObject<List<string>>(jsonDeck);
        // add each card once
        foreach (string path in cardPaths) {
            deck.Add(Resources.Load<Card>(path));
        }
        deck.Sort();
    }

    void DisplayDeck() {
        Vector3[] cardPostions = CalculatePositions();
        Card[] deckArr = deck.ToArray();
        for (int card = 0; card < deck.Count; card++) {
            GameObject displayedCard;
            if (DEBUG_MODE) Debug.Log("Instantiating card at transform: " + transform.position);
            if (deckArr[card].cardType == Card.CardType.Tower) {
                displayedCard = Instantiate(towerCardPrefab, cardPostions[card], Quaternion.identity, transform);
            } else {
                displayedCard = Instantiate(spellCardPrefab, cardPostions[card], Quaternion.identity, transform);
            }
            displayedCard.GetComponent<CardMovement>().enabled = false;
            displayedCard.GetComponent<CardDisplay>().cardData = deckArr[card];
            displayedCard.transform.localScale *= cardScaleFactor;
            
        }
    }


    public Vector3[] CalculatePositions() {
        Vector3[] positions = new Vector3[maxCardsInRow * maxRows];
        int numCards = deck.Count;
        if (DEBUG_MODE) Debug.Log("Number of Cards: " + numCards);
        if (numCards > maxRows * maxCardsInRow) {
            throw new OverflowException("Max deck size exceeded");
        }
        float width;
        if (DEVELOPER_MODE) {
            width = testWidth;
        } else {
            width = rectTransform.rect.width;
        }
        // what row we are on
        if (DEBUG_MODE) Debug.Log("Rows required " + Math.Ceiling((double)numCards / maxCardsInRow));
        for (int row = 0; row < Math.Ceiling((double)numCards / maxCardsInRow); row++) {
            if (DEBUG_MODE) Debug.Log("On row " + row);
            int numCardsDone = row * maxCardsInRow;
            if (DEBUG_MODE) Debug.Log("Cards processed: " + numCardsDone);
            // which card we are on, given what row we are on
            // loops up to maxCardsInRow times, this clamp is inelegant I feel
            if (DEBUG_MODE) Debug.Log("Cards in this row =  " + Math.Clamp(numCards - numCardsDone, 0, 6));
            int cardsInRow = Math.Clamp(numCards - numCardsDone, 0, 6);
            for (int card = numCardsDone; card < cardsInRow + numCardsDone; card++) {
                if (DEBUG_MODE) Debug.Log("On card " + card);
                int relativeCard = card % maxCardsInRow;
                if (DEBUG_MODE) Debug.Log("Relative card " + relativeCard);
                float cardWidth = towerCardPrefab.GetComponent<RectTransform>().rect.width * towerCardPrefab.transform.localScale.x;
                float spacing = cardWidth + (width - cardsInRow * cardWidth) / (cardsInRow - 1);
                positions[card] = new Vector3(cardWidth / 2 + relativeCard * spacing, transform.localPosition.y + row * rowScaleFactor + verticalCardDisplayOffset);
                if (DEBUG_MODE) Debug.Log("added position " + positions[card]);
            }
        }
        return positions;
    }


    public void TEST_SetDeck(List<Card> deck) {
        if (DEVELOPER_MODE) this.deck = deck;
    }
}
