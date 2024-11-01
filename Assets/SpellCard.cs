using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "New Spell Card", menuName = "Card/Spell Card")]
public class SpellCard : Card
{
  [SerializeField] private int SpellID;
  private SpellData SpellData;

  void Start() {
    var FileData = Resources.Load<TextAsset>("Spells/Spell_" + SpellID);

        if (FileData != null) {
            string JSONPlainText = FileData.text;
            this.SpellData = JsonConvert.DeserializeObject<SpellData>(JSONPlainText);
        }
        else {
            Debug.Log("Unable to load Spell_" + SpellID);
        }
  }
}


public class SpellData {

  public enum SpellType {
      Blessing,
      Hex
    }
  public SpellType Type{ get; set;}
  public int Damage{ get; set;}
  public int AttackSpeed{ get; set;}
}
