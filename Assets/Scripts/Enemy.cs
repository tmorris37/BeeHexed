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

        public int health;

        [SerializeField] FloatingHealthBar healthBar;

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
            this.health = this.Data.MaxHP;
            healthBar = GetComponentInChildren<FloatingHealthBar>();
            healthBar.UpdateHealthBar(this.health, this.Data.MaxHP);

        }



        void Update()
        {
             Debug.Log(this.health);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
           // Debug.Log("It detects a thing");
            if (other.CompareTag("Projetile"))
            {
                TakeDamage(1);
                /*if (this.Data.MaxHP <= 0)
                {
                    Destroy(this);
                }*/
            }

        }

        public void TakeDamage(int damage)
        {
            this.health = this.health - damage;
            healthBar.UpdateHealthBar(this.health, this.Data.MaxHP);
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
