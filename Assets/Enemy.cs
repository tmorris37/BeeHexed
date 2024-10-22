using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int EnemyID;
    private EnemyData Data;

    private int q, r, s;
    void Start()
    {
        // Assets/Resources/Enemies/Enemy_"".json
        // Creates a TextAsset containing the data from Enemy_"".json
        var FileData = Resources.Load<TextAsset>("Enemies/Enemy_" + EnemyID);

        if (FileData != null)
        {
            // TextAsset -> String (JSON)
            string JSONPlainText = FileData.text;
            // String (JSON) -> EnemyData Class
            this.Data = JsonConvert.DeserializeObject<EnemyData>(JSONPlainText);
            // We can then read the values from the Data class as needed
        }
        else
        {
            Debug.Log("Unable to load Enemy_" + EnemyID);
        }

    }

    void Update()
    {
        
    }
}

#region JSON Data Structures

[System.Serializable]
public class EnemyData
{
    public int MaxHP { get; set; }
    public IList<Attack> Attacks { get; set; }
    public string MovementType { get; set; }
    // More fields based on what we need to store about an enemy
}

[System.Serializable]
public class Attack
{
    public string DamageType { get; set; }
    public int DamageAmount { get; set; }
    // More fields based on attack type
}

#endregion