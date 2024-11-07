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

        private Transform target;

        private float fireCountdown;

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

        void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log("It detects a thing");
            if (other.CompareTag("Enemy"))
            {
                target = other.transform;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && other.transform == target)
            {
                target = null;
            }
        }

        private void Update()
        {
            if (target != null)
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
        // More fields based on attack type
    }

    #endregion
}
