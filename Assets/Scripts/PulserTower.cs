using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GridSystem;

namespace EnemyAndTowers
{
    public class PulserTower : HexPosition
    {
        [SerializeField] public int TowerID;
        public TowerData Data;

        
        public float pulseRate = 0.5f; // Pulse every 3 seconds

        public GameObject projectilePrefab;

        private List<Transform> targets;
        private float fireCountdown;
        private float pulseCountdown; // Countdown for the pulse effect
        private int HP;

        [SerializeField] private Animator circleAnimator;

        
        //public Animator animator;

        [SerializeField] FloatingHealthBar healthBar;

        void Start()
        {
            var FileData = Resources.Load<TextAsset>("Towers/Tower_" + TowerID);

            if (FileData != null)
            {
                string JSONPlainText = FileData.text;
                this.Data = JsonConvert.DeserializeObject<TowerData>(JSONPlainText);
            }
            else
            {
                Debug.Log("Unable to load Tower_" + TowerID);
            }
            this.HP = this.Data.MaxHP;
            healthBar = GetComponentInChildren<FloatingHealthBar>();
            healthBar.UpdateHealthBar(this.HP, this.Data.MaxHP);
            
            this.targets = new List<Transform>();
            pulseCountdown = pulseRate; // Initialize the pulse countdown
            //animator = GetComponent<Animator>();
            
        }

        void Update()
        {
            /*if (target != null && fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;*/

            // Pulse effect logic
            if (pulseCountdown <= 0f)
            {
                PulseDamage();
                pulseCountdown = pulseRate;
            }
            pulseCountdown -= Time.deltaTime;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log("It detects a thing");
            if (other.CompareTag("Enemy"))
            {
                targets.Add(other.transform);
                
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            //if (other.CompareTag("Enemy") && other.transform == target)
            if (other.CompareTag("Enemy"))
            {
                targets.Remove(other.transform);
                if (targets.Count == 0)
                {
                    circleAnimator.Play("newState");
                }
            }
        }



        /*private void Shoot()
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            TowerProjectile projScript = projectile.GetComponent<TowerProjectile>();
            if (projScript != null)
            {
                projScript.Seek(target);
            }
        }*/

        private void PulseDamage()
        {
            /*List<Transform> enemiesInRange = GetEnemiesInAdjacentHexes();
            Debug.Log("ENEMY LIST: " + enemiesInRange.Count);
            foreach (Transform enemy in enemiesInRange)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>(); // Assuming enemies have an Enemy script
                if (enemyScript != null)
                {
                    //enemyScript.TakeDamage(Data.Attacks[0].DamageAmount); // Adjust damage amount as needed
                    enemyScript.TakeDamage(1);
                }
            }*/
            //animator.SetTrigger("PulseTrigger");
           
            foreach (Transform enemy in targets)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    
                    enemyScript.TakeDamage(1);
                    if (circleAnimator != null)
                    {
                
                        circleAnimator.SetTrigger("PulseEffect");
                    }
                }
                
            }

            
        }

        /*private List<Transform> GetEnemiesInAdjacentHexes()
        {
            List<Transform> enemies = new List<Transform>();
            // Assuming you have a GridManager or similar component to get neighboring hexes
            (int q, int r, int s) = GridManager.XYtoQRS(this.transform.position.x, this.transform.position.y);
            var adjacentHexes = GridManager.GetAdjacentHexes(q, r, s); 

            foreach (var hex in adjacentHexes)
            {
                foreach (var enemy in hex.GetEnemiesInHex()) // Assuming a method to get enemies in a hex
                {
                    enemies.Add(enemy.transform);
                }
            }

            return enemies;
        }*/
    }
}
