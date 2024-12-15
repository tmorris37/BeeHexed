
using System;
using UnityEngine;
using UnityEngine.UI;

public class Card : ScriptableObject, IComparable {
    [SerializeField] public string cardName;
    [SerializeField] public int cost;
    [SerializeField] public string bodyText;
    [SerializeField] public string health;
    [SerializeField] public Sprite art;
    //[SerializeField] public UnityEngine.UI.Image art;
    [SerializeField] public CardType cardType;
    [SerializeField] public GameObject prefab;

    public int CompareTo(object obj) {
        if (obj == null) return 1;
        Card other = obj as Card;
        if (other != null) {
            return this.cardName.CompareTo(other.cardName);
        } else {
            throw new ArgumentException("Compared object was not a card");
        }
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
    
    public enum CardType {
    Tower,
    Spell
    }
}
