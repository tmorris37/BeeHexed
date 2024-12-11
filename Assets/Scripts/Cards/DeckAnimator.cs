using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckAnimator : MonoBehaviour
{
    [SerializeField] private float yCardOffset;
    [SerializeField] private float xCardOffset;
    [SerializeField] private int cardsShownInDeck = 4;
    [SerializeField] private GameObject cardBackPrefab;
    private Sprite cardBack;
    private GameObject[] deckDisplay;
    private HandManager handManager;
    private DrawPileManager drawPileManager;
    void Awake()
    {
        deckDisplay = new GameObject[cardsShownInDeck];
        handManager = FindObjectOfType<HandManager>();
        drawPileManager = FindObjectOfType<DrawPileManager>();
        cardBack = GetCardBack();
    }

    void Start() {
        CreateDeck();
    }

    private void CreateDeck()
    {
        for (int i = 0; i < cardsShownInDeck; i++) {
            cardBackPrefab.GetComponent<Image>().sprite = cardBack;
            //Debug.Log("Instantiating at position: " + cardPosition);
            GameObject instance = Instantiate(cardBackPrefab, transform);
            Vector3 cardPosition = new(instance.transform.localPosition.x + xCardOffset * i, instance.transform.localPosition.y + yCardOffset * i);
            instance.transform.localPosition = cardPosition;
            deckDisplay[i] = instance;
        }
    }

    private Sprite GetCardBack()
    {
        return PlayerData.cardBack.sprite;
    }

    public void DrawCardAnimation(GameObject card) {
        card.SetActive(false);
        int deckSize = drawPileManager.deck.Count;
        // remove cards such that cards match actual deck size
        // Currently depends on deck size being updated before card is added to hand (BAD)
        if (deckSize < cardsShownInDeck) {
            Debug.Log("Read deck size = " + deckSize);
            deckDisplay[deckSize].SetActive(false);
        }
        StartCoroutine(DrawCardCoroutine(card));
    }

    private IEnumerator DrawCardCoroutine(GameObject card) {
        yield return new WaitForSeconds(0.5f);
        card.SetActive(true);
    }

    public void Shuffle() {
        for (int card = 0; card < drawPileManager.deck.Count; card++) {
            if (card < cardsShownInDeck) {
                deckDisplay[card].SetActive(true);
            }
        }
    }
}
