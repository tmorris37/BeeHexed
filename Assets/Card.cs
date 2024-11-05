using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject
{
  [SerializeField] public string cardName;
  [SerializeField] public int cost;
  [SerializeField] public string bodyText;
  [SerializeField] public string health;

  [SerializeField] public CardType cardType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public enum CardType {
      Tower,
      Spell
    }
}
