using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

using GridSystem;

namespace EnemyAndTowers
{
    public class Enemy : HexPosition
    {
        // [SerializeField] protected FloatingHealthBar healthBar;
        public int EnemyID;
        public int health;
        public float movementSpeed = 0.5f;
        public float moveTimeRemaining;
        public bool isMoving = false;
        public float attackRate = 1f;
        public float attackCooldown;
        protected Vector3 targetPosition;
        public MovementAlgorithms Movement;
        public EnemyData Data;
        public EnemyDetection Detection;
        public List<Transform> targets;

        protected virtual void Start()
        {
            this.DEBUG = true;
            Detection = GetComponentInChildren<EnemyDetection>();
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
            // healthBar = GetComponentInChildren<FloatingHealthBar>();
            // healthBar.UpdateHealthBar(this.health, this.Data.MaxHP);
            attackCooldown = attackRate;

        }

        // Call this method to smoothly move the enemy to a specified position
        public virtual void MoveToPosition(Vector3 target)
        {
            Debug.Log("Target: " + target);
            targetPosition = target;

            StopAllCoroutines();  // Stop any ongoing movement to avoid conflicts
            StartCoroutine(MoveToTargetAtFixedSpeed(targetPosition));
        }

        // Coroutine to translate the position at a constant speed
        private IEnumerator MoveToTargetAtFixedSpeed(Vector3 target)
        {
            Vector3 initialPosition = transform.position;
            float distanceToTarget = Vector3.Distance(initialPosition, target);

            // Calculate total time to travel based on speed
            float travelTime = 1 / movementSpeed;
            float elapsedTime = 0f;

            while (elapsedTime < travelTime)
            {
                // Calculate interpolation value based on elapsed time and speed
                float t = elapsedTime / travelTime;

                // Update position at a constant rate towards the target
                transform.position = Vector3.Lerp(initialPosition, target, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Set the final position exactly to the target
            transform.position = target;
        }
        protected virtual void Update()
        {
            // Update the list of targets from the detection script
            this.targets = Detection.targets;
            
            // Check if there are any targets in range and attack if off cooldown
            if (attackCooldown <= 0f && targets.Count > 0)
            {
                Debug.Log("Targets identified, time to attack");
                Attack();
                attackCooldown = attackRate;
            }
            // Try and move towards the next tile if off cooldown
            if (moveTimeRemaining <= 0f)
            {
                Debug.Log("No targets identified, time to move");
                // Only reset the moveTimeRemaining if the enemy actually started moving
                if (Movement.SimpleMove(this)) {
                    moveTimeRemaining = 1 / movementSpeed;
                }
            }
            // Update the timers
            moveTimeRemaining -= Time.deltaTime;
            attackCooldown -= Time.deltaTime;
        }

        protected virtual void Attack()
        {
            Debug.Log("Towers: " + targets.Count);
            foreach (Transform tower in targets)
            {
                Debug.Log("Attacking Tower: " + tower.name);
                Tower towerScript = tower.GetComponent<Tower>();
                if (towerScript != null)
                {
                    Debug.Log("Tower Script Found");
                    towerScript.TakeDamage(5);
                }
            }
        }

        public virtual void TakeDamage(int damage)
        {
            StartCoroutine(UpdateHealthAfterDelay(damage, 0.04f));
        }

        private IEnumerator UpdateHealthAfterDelay(int damage, float delay)
        {
            yield return new WaitForSeconds(delay);
            this.health = this.health - damage;
            // healthBar.UpdateHealthBar(this.health, this.Data.MaxHP);
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
