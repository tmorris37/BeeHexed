using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DeckManager : MonoBehaviour
{

    public List<Card> deck = new List<Card>();
    private int currIndex;

    [SerializeField] int handLimit = 10;


    public void DrawCard(HandManager handManager) {
      if (deck.Count == 0 || currIndex >= deck.Count || handManager.handSize == handLimit) {
        return;
      }
      Card nextCard = deck[currIndex];
      handManager.AddToHand(nextCard);
      currIndex++;
      handManager.handSize++;
    }

    // Start is called before the first frame update
    void Start()
    {
      // load all cards from resources
      Card[] cardList = Resources.LoadAll<Card>("Cards");
      // fill deck with cards
      deck.AddRange(cardList);

      // on start, draw 4 cards
      HandManager hand = FindObjectOfType<HandManager>();
      for (int i = 0; i < 4; i++) {
        DrawCard(hand);
      }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
