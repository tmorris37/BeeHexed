using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiscardManager : MonoBehaviour
{

    [SerializeField] public List<Card> graveyard = new List<Card>();
    public TextMeshProUGUI graveyardSizeText;
    public int graveyardSize;
    public void Awake()
    {
        UpdateGraveyardSize();
    }

  private void UpdateGraveyardSize()
  {
    graveyardSizeText.text = graveyard.Count.ToString();
    graveyardSize = graveyard.Count;
  }

  public void discard(Card card) {
    // sanity check
    if (card != null) {
      graveyard.Add(card);
      UpdateGraveyardSize();
      // TODO: Make graveyard show most recently discarded card
    }
  }

  public bool drawFromGraveyard(Card card) {
    if (graveyardSize > 0 && graveyard.Contains(card)) {
      graveyard.Remove(card);
      UpdateGraveyardSize();
      return true;
    } else {
      return false;
    }
  }

  public List<Card> drawAllGraveyard() {
    List<Card> returnList = new List<Card>(graveyard);
    graveyard.Clear();
    UpdateGraveyardSize();
    return returnList;
  }

  // Update is called once per frame
  void Update()
    {
        
    }
}
