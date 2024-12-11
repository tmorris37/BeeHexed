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
    private List<GameObject> deckDisplay;
    void Awake()
    {
        deckDisplay = new();
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
            deckDisplay.Add(instance);
        }
    }

    private Sprite GetCardBack()
    {
        return PlayerData.cardBack.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
