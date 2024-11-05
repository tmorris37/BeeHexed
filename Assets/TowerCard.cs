using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "New Tower Card", menuName = "Card/Tower Card")]
public class TowerCard : Card
{
  [SerializeField] private int towerID;
  public TowerData towerData;

  void Start() {
    var FileData = Resources.Load<TextAsset>("Towers/Tower_" + towerID);

        if (FileData != null) {
            string JSONPlainText = FileData.text;
            this.towerData = JsonConvert.DeserializeObject<TowerData>(JSONPlainText);
        }
        else {
            Debug.Log("Unable to load Tower_" + towerID);
        }
  }
}


public class TowerData {
  public int MaxHealth{ get; set;}
  public int Damage{ get; set;}
  public int AttackSpeed{ get; set;}

}

