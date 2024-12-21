using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

using GridSystem;

namespace EnemyAndTowers
{
    // Base class for all towers
    public class Tower : HexPosition
    {
        [SerializeField] public int towerID;  // ID of the tower, used to load data from JSON
        public TowerData data;                // Data loaded from JSON
        public TowerDetection detection;      // Reference to the detection script
        public float fireRate;                // Rate of fire in seconds
        public int health;                    // Current health of the tower
        public int damage;                    // Damage dealt by the tower

        public bool active;                   // Whether the tower is active
        public float fireCountdown;           // Time until the tower can fire again
        protected List<Transform> targets;    // List of targets in range


        protected virtual void Start() {
            // Find the TowerDetection script attached to the tower
            detection = GetComponentInChildren<TowerDetection>();
            // Creates a TextAsset containing the data from Enemy_"".json
            var FileData = Resources.Load<TextAsset>("Towers/Tower_" + towerID);

            if (FileData != null) {
                // TextAsset -> String (JSON)
                string JSONPlainText = FileData.text;
                // String (JSON) -> EnemyData Class
                data = JsonConvert.DeserializeObject<TowerData>(JSONPlainText);
                // We can then read the values from the Data class as needed
            } else {
                Debug.Log("Unable to load Tower_" + towerID);
            }

            // Set the tower's health to the maximum health
            // And create an empty list of targets
            health = data.MaxHealth;
            targets = new List<Transform>();
        }

        // Update is called once per frame and is used to see if the 
        // tower still has health. If not, the tower is destroyed.
        protected virtual void Update() {
            if (health <= 0) {
                HexTile tile = gridManager.FetchTile(q,r,s);
                tile.LeaveTile(gameObject);
                Destroy(gameObject);
            }
        }

        // Function to take damage
        public virtual void TakeDamage(int damage) {
            GoRed();
            StartCoroutine(FadeBackColor(0.5f));
            health = health - damage;
        }

        // Function to change the color of the tower to red
        public virtual void GoRed() {
            SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
            sprite.color = Color.red;
        }

        // Coroutine to fade the color of the tower back to white
        private IEnumerator FadeBackColor(float duration) {
            SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
            Color startColor = sprite.color;
            Color endColor = Color.white;
            float elapsed = 0f;

            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                sprite.color = Color.Lerp(startColor, endColor, elapsed / duration);
                yield return null; // Wait for the next frame
            }

            sprite.color = endColor; // Ensure the final color is set
        }

        // Function to check if the tower is rotatable
        // False by default, but can be overridden in subclasses
        public virtual bool IsRotatable() {
            return false;
        }
    }

    #region JSON Data Structures

    [System.Serializable]
    public class TowerData {
        public int MaxHealth { get; set; }
        public IList<TowerAttack> Attacks { get; set; }
        

        // More fields based on what we need to store about an enemy
    }


    //[System.Serializable]
    public class TowerAttack {
        public string DamageType { get; set; }
        public int DamageAmount { get; set; }

        public int Speed { get; set;}
        // More fields based on attack type
    }

    #endregion
}
