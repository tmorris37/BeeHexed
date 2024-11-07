using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    // assigned within the Unity inspector
    public GameObject cardPrefab;
    // where the bottom of the hand is (also assigned in Unity)
    public Transform handLocation;
    // determines how much the hand is spread out, can be altered in inspector
    public float fanSpread = 10f;
    // determines how far apart each card is in the hand
    public float horizCardSpacing = -100f;
    // determines how far down each card goes
    public float vertCardSpacing = 100f;
    // a list of the cards in our hand
    public List<GameObject> cardsInHand = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
      // add 6 cards to hand
        for (int i = 0; i < 5; i++) {
          AddToHand();  
        }
    }

  private void AddToHand()
  {
    GameObject addCard = Instantiate(cardPrefab, handLocation.position, Quaternion.identity, handLocation);
    cardsInHand.Add(addCard);
    UpdateHandDisplay();
  }

  private void UpdateHandDisplay()
  {

    int numCards = cardsInHand.Count;

    if (numCards == 1) {
      cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
      cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
    } else {
      for (int i=0; i < numCards; i++) {
      float cardAngle = fanSpread * (i - (numCards - 1) / 2f);
      float normalizePosition = 2f * i / (numCards - 1) - 1f;  // centers hand arc
      float horizPosition = horizCardSpacing * (i - numCards) / 2f;
      float vertPosition = vertCardSpacing * (1 - normalizePosition * normalizePosition);
      cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, cardAngle);
      cardsInHand[i].transform.localPosition = new Vector3(horizPosition, vertPosition, 0f);
      }
    }
    

    
  }

  // Update is called once per frame
  void Update()
    {
        UpdateHandDisplay();
    }
}
