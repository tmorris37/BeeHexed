using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RewardManager : MonoBehaviour
{
    [SerializeField] private GameObject spellCardPrefab;
    [SerializeField] private GameObject towardCardPrefab;
    [SerializeField] private Image acceptButton;
    [SerializeField] private int cardSeed;
    [SerializeField] private Card rewardCard;
    private GameObject displayCard;
    // Money and power-ups would go here
    
    void Awake() {
      Card[] rewardCardList = Resources.LoadAll<Card>("RewardCards");
      cardSeed = Random.Range(1,100);
      rewardCard = rewardCardList[cardSeed % rewardCardList.Length];
    }
    void Start()
    {   
      if (rewardCard.cardType == Card.CardType.Tower) {
        displayCard = Instantiate(towardCardPrefab, transform);
      } else {
        displayCard = Instantiate(spellCardPrefab, transform);
      }
      displayCard.GetComponent<CardDisplay>().cardData = rewardCard;
      displayCard.GetComponent<CardMovement>().enabled = false;
    }

    public Card GetRewardCard() {
      return rewardCard;
    }
}
