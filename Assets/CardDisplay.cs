using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;

    public Image cardImage;
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text costText;
    public TMP_Text bodyText;
    // Start is called before the first frame update
    void Start()
    {
        UpdateCardDisplay();
    }

    void UpdateCardDisplay()
    {
      nameText.text = cardData.cardName;
      bodyText.text = cardData.bodyText;
      costText.text = cardData.cost.ToString();
      healthText.text = cardData.health.ToString();
      // if (cardData.cardType == Card.CardType.Tower) {
      //   healthText.text = ((TowerCard)cardData).towerData.MaxHealth.ToString();
      // } else {
      //   healthText.text = "";
      // }

    }
}
