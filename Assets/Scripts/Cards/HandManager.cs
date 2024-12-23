using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    // assigned within the Unity inspector
    public GameObject towerCardPrefab;
    public GameObject spellCardPrefab;
    // where the bottom of the hand is (also assigned in Unity)
    public Transform handLocation;
    // determines how much the hand is spread out, can be altered in inspector
    public float fanSpread = 10f;
    // determines how far apart each card is in the hand
    public float horizCardSpacing = -100f;
    // determines how far down each card goes
    public float vertCardSpacing = 100f;
    // a list of the cards in our hand
    public List<GameObject> cardsInHand = new();

    public DrawPileManager drawPileManager;

    public DiscardManager discardManager;

    // TODO: probably privatize everything
    public int handSize = 0;
    private CardPlaystyle playstyle;

    public string settingSavePath;

    private void Awake() {
        // A persistent location to store written data
        // On Windows: ..\AppData\LocalLow\defaultcompany\BeeHexed\DeckAssets\Save.json
        string filepath = Application.persistentDataPath + "/DeckAssets";
        settingSavePath = filepath + "/Settings.json";

        System.IO.Directory.CreateDirectory(filepath);

        try {
            string json = File.ReadAllText(settingSavePath);
            playstyle = JsonConvert.DeserializeObject<CardPlaystyle>(json);
        } catch {
            playstyle = 0;
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        discardManager = FindObjectOfType<DiscardManager>();
    }

    public void AddToHand(Card card) {
        GameObject addCard;
        if (card.cardType == Card.CardType.Tower) {
            addCard = Instantiate(towerCardPrefab, handLocation.position,
            Quaternion.identity, handLocation);
        } else {
            addCard = Instantiate(spellCardPrefab, handLocation.position,
            Quaternion.identity, handLocation);
        }
        cardsInHand.Add(addCard);
        handSize++;
        addCard.GetComponent<CardDisplay>().cardData = card;
        addCard.name = card.cardName;
        addCard.GetComponent<CardMovement>().playstyle = playstyle;
        UpdateHandDisplay();
    }

    public bool DiscardCard(Card card) {
        discardManager.discard(card);
        handSize--;
        UpdateHandDisplay();
        return true;
    }

    public void UpdateHandDisplay() {
        int numCards = cardsInHand.Count;
        if (numCards == 1) {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
        } else {
            for (int i=0; i < numCards; i++) {
                float cardAngle = fanSpread * (i - (numCards - 1) / 2f);
                float normalizePosition = 2f * i / (numCards - 1) - 1f;  // centers hand arc
                float horizPosition = horizCardSpacing * (i - (numCards - 1)) / 2f;
                float vertPosition = vertCardSpacing * (1 - normalizePosition * normalizePosition);
                // TODO: optimize sequential assignment
                cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, cardAngle);
                cardsInHand[i].transform.localPosition = new Vector3(horizPosition, vertPosition, 0f);
                CardMovement moveScript = cardsInHand[i].GetComponent<CardMovement>();
                moveScript.origCardPosition = cardsInHand[i].transform.localPosition;
                moveScript.origCardRotation = cardsInHand[i].transform.localRotation;
            }
        }
    }

    public void SetHandLayer(int layer) {
        Canvas[] cardCanvases = handLocation.gameObject.GetComponentsInChildren<Canvas>();
        foreach (Canvas canvas in cardCanvases) {
            canvas.sortingOrder = layer;
        }
    }
}
