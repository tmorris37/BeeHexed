
using UnityEngine;
using UnityEngine.UI;

public class Card : ScriptableObject
{
  [SerializeField] public string cardName;
  [SerializeField] public int cost;
  [SerializeField] public string bodyText;
  [SerializeField] public string health;
  [SerializeField] public Sprite art;
  //[SerializeField] public UnityEngine.UI.Image art;
  [SerializeField] public CardType cardType;
  [SerializeField] public GameObject prefab;

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
