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
        
        public float fireRate = 2f; // Pulse every 3 seconds

        //public GameObject projectilePrefab;

        public GameObject beam;

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
            if (fireCountdown <= 0f && targets.Count > 0)
            {
                fire();
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



        

        private void fire()
        {
           
           if (beam == null)
    {
        Debug.LogError("Beam prefab not assigned!");
        return;
    }

    
    // Instantiate the beam at the tower's position with the same rotation as the tower
    Vector3 offset = -this.transform.right * 12.5f; // Assuming the beam shoots along the tower's right direction
GameObject instantiatedBeam = Instantiate(beam, this.transform.position + offset, this.transform.rotation);
    
    instantiatedBeam.transform.rotation = this.transform.rotation;


    
    Destroy(instantiatedBeam, 0.2f);
            
             
        }

       
    }
}
