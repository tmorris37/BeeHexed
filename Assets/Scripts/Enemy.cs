using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace EnemyAndTowers
{
    public class Enemy : HexPosition
    {
        [SerializeField] public int EnemyID;
        public EnemyData Data;

        void Start()
        {
            this.DEBUG = true;
            // Instantiates the Enemy at the Provided Spawn Location
            // If The Enemy could not spawn, despawn the sprite
            if (!SetPosition())
            {
                if (DEBUG)
                    Debug.Log("Despawning");
                Destroy(gameObject);
                return;
            }
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
}