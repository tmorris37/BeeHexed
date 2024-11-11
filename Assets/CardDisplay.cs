using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data.Common;


public class CardDisplay : MonoBehaviour
{
    public Card cardData;

    public Image cardTemplate;
    public Image cardArt;
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text costText;
    public TMP_Text bodyText;
    public TMP_Text typeText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCardDisplay();
        cardArt.sprite = cardData.art;
    }

    void UpdateCardDisplay()
    {
      nameText.text = cardData.cardName;
      bodyText.text = cardData.bodyText;
      costText.text = cardData.cost.ToString();
      if (cardData.cardType == Card.CardType.Tower) {
        healthText.text = cardData.health.ToString();
        typeText.text = "Tower";
      } else if (cardData.cardType == Card.CardType.Spell) {
        typeText.text = "Spell - " + ((SpellCard)cardData).Type.ToString();
      }
    }
}
