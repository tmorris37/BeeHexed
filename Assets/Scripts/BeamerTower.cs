using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GridSystem;

namespace EnemyAndTowers
{
    public class BeamerTower : HexPosition
    {
        [SerializeField] public int TowerID;
        public TowerData Data;

        
        public float fireRate = 0.5f; // Pulse every 3 seconds

        public GameObject projectilePrefab;

        private List<Transform> targets;
        private float fireCountdown;
        
        private int HP;

        //[SerializeField] private Animator circleAnimator;

        
        //public Animator animator;

        [SerializeField] FloatingHealthBar healthBar;

        void Start()
        {
            var FileData = Resources.Load<TextAsset>("Towers/Tower_" + TowerID);
            this.targets = new List<Transform>();
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
            
            
            fireCountdown = fireRate; // Initialize the pulse countdown
            //animator = GetComponent<Animator>();
            
        }

        void Update()
        {
            

            // Pulse effect logic
            if (fireCountdown <= 0f)
            {
                Damage();
                fireCountdown = fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log("It detects a thing");
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("Enemy detected");
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
                    //circleAnimator.Play("newState");
                }
            }
        }



        

        private void Damage()
        {
           
           //possible bug were targets is null if firerate isn't set. Very strange
            foreach (Transform enemy in targets)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    
                    enemyScript.TakeDamage(1);
                    //if (circleAnimator != null)
                    //{
                
                        //circleAnimator.SetTrigger("PulseEffect");
                    //}
                }
                
            }

            
        }

       
    }
}
