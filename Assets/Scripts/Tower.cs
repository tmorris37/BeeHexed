using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace EnemyAndTowers
{
    public class Tower : HexPosition
    {
        [SerializeField] public int TowerID;
        public TowerData Data;

        public float fireRate = 1f;

        public GameObject projectilePrefab;

        private List<Transform> targets;

        private float fireCountdown;

        private int HP;

        //public GameObject prefab;

        [SerializeField] FloatingHealthBar healthBar;

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
            this.HP = this.Data.MaxHP;
            healthBar = GetComponentInChildren<FloatingHealthBar>();
            healthBar.UpdateHealthBar(this.HP, this.Data.MaxHP);
            targets = new List<Transform>();

        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log("It detects a thing");
            if (other.CompareTag("Enemy"))
            {
                //target = other.transform;
                targets.Add(other.transform);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            //if (other.CompareTag("Enemy") && other.transform == target)
            if (other.CompareTag("Enemy"))
            {
                //target = null;
                targets.Remove(other.transform);
            }
        }

        private void Update()
        {
            //Debug.Log(this.HP);
            if (targets.Count > 0)
            {
                // shoot at intervals based on fireRate
                if (fireCountdown <= 0f)
                {
                    Shoot();
                    fireCountdown = 1f / fireRate;
                }
                fireCountdown -= Time.deltaTime;
            }
        }

        private void Shoot()
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            TowerProjectile projScript = projectile.GetComponent<TowerProjectile>();
            if (projScript != null)
            {
                //Transform target;
                //if (targets.Count > 0)
                
                 Transform target = targets[0];
                
                projScript.Seek(target);
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

        public int Speed { get; set;}
        // More fields based on attack type
    }

    #endregion
}
