using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class Tower : HexPosition
{
    [SerializeField] public int TowerID;
    public TowerData Data;

    void Start()
    {
        // Assets/Resources/Enemies/Enemy_"".json
        // Creates a TextAsset containing the data from Enemy_"".json
        var FileData = Resources.Load<TextAsset>("Towers/Tower_" + TowerID);

        if (FileData != null)
        {
            // TextAsset -> String (JSON)
            string JSONPlainText = FileData.text;
            // String (JSON) -> EnemyData Class
            this.Data = JsonConvert.DeserializeObject<TowerData>(JSONPlainText);
            // We can then read the values from the Data class as needed
        }
        else
        {
            Debug.Log("Unable to load Tower_" + TowerID);
        }

    }

    

}

#region JSON Data Structures

[System.Serializable]
public class TowerData
{
    public int MaxHP { get; set; }
    public IList<TowerAttack> Attacks { get; set; }
    

    
    // More fields based on what we need to store about an enemy
}


//[System.Serializable]
public class TowerAttack
{
    public string DamageType { get; set; }
    public int DamageAmount { get; set; }
    // More fields based on attack type
}

#endregion
