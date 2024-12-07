using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

using GridSystem;

namespace EnemyAndTowers
{
    public class Tower : HexPosition
    {
        [SerializeField] public int towerID;
        public TowerData data;
        public TowerDetection detection;
        public float fireRate;
        public int health;
        public int damage;

        public bool active;
        protected List<Transform> targets;
        protected float fireCountdown;

        [SerializeField] protected FloatingHealthBar healthBar;

        protected virtual void Start()
        {
            detection = GetComponentInChildren<TowerDetection>();
            // Assets/Resources/Enemies/Enemy_"".json
            // Creates a TextAsset containing the data from Enemy_"".json
            var FileData = Resources.Load<TextAsset>("Towers/Tower_" + towerID);

            if (FileData != null)
            {
                // TextAsset -> String (JSON)
                string JSONPlainText = FileData.text;
                // String (JSON) -> EnemyData Class
                this.data = JsonConvert.DeserializeObject<TowerData>(JSONPlainText);
                // We can then read the values from the Data class as needed
            }
            else
            {
                Debug.Log("Unable to load Tower_" + towerID);
            }
            this.health = this.data.MaxHealth;
            // healthBar = GetComponentInChildren<FloatingHealthBar>();
            // healthBar.UpdateHealthBar(this.health, this.Data.MaxHealth);
            targets = new List<Transform>();
        }

        protected virtual void Update()
        {
            this.targets = detection.targets;
            if (this.health <= 0)
            {
                HexTile tile = gridManager.FetchTile(q,r,s);
                tile.LeaveTile(gameObject);
                Destroy(gameObject);
            }
        }

        public virtual void TakeDamage(int damage)
        {
            SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
            sprite.color = Color.red;
            StartCoroutine(FadeBackColor(sprite, 0.5f));
            this.health = this.health - damage;
        }

        /*public virtual void GoRed()
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.red;
        }*/

        private IEnumerator FadeBackColor(SpriteRenderer sprite, float duration)
        {
            Color startColor = sprite.color;
            Color endColor = Color.white;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                sprite.color = Color.Lerp(startColor, endColor, elapsed / duration);
                yield return null; // Wait for the next frame
            }

            sprite.color = endColor; // Ensure the final color is set
        }
    }

    #region JSON Data Structures

    [System.Serializable]
    public class TowerData
    {
        public int MaxHealth { get; set; }
        public IList<TowerAttack> Attacks { get; set; }
        

        
        // More fields based on what we need to store about an enemy
    }


    //[System.Serializable]
    public class TowerAttack
    {
        public string DamageType { get; set; }
        public int DamageAmount { get; set; }

        public int Speed { get; set;}
        // More fields based on attack type
    }

    #endregion
}
