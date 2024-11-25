using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

using GridSystem;

namespace EnemyAndTowers
{
    public class Enemy : HexPosition
    {
        public enum InertiaDirection        // Possible directions of inertia
        {
            cw,   // Clockwise
            ccw,  // Counter-clockwise
            cwi,  // Clockwise inward
            cwo,  // Clockwise outward
            ccwi, // Counter-clockwise inward
            ccwo, // Counter-clockwise outward
            i,    // Inward
            o     // Outward
        }
        // [SerializeField] protected FloatingHealthBar healthBar;
        public int EnemyID;                 // Determines the type of enemy
        public int health;                  // Current health of the enemy
        public float movementSpeed = 0.5f;  // Speed at which the enemy moves (tiles per second)
        public float moveTimeRemaining;     // Time remaining to move to the next tile
        public float attackRate = 1f;       // Time between attacks
        public float attackCooldown;        // Time remaining before the enemy can attack again
        public Vector3 targetPosition;      // Position the enemy is moving towards
        public MovementAlgorithms movement; // Movement algorithms for the enemy
        public EnemyData data;              // Data about the enemy
        public EnemyDetection detection;    // Detection script for the enemy
        public List<Transform> targets;     // List of targets in range
        public InertiaDirection inertia;    // Direction of inertia for the enemy

        protected virtual void Start()
        {
            // Get the hitbox of the enemy
            detection = GetComponentInChildren<EnemyDetection>();
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
                this.data = JsonConvert.DeserializeObject<EnemyData>(JSONPlainText);
                // We can then read the values from the Data class as needed
            }
            else
            {
                Debug.Log("Unable to load Enemy_" + EnemyID);
            }
            this.health = this.data.MaxHP;
            // healthBar = GetComponentInChildren<FloatingHealthBar>();
            // healthBar.UpdateHealthBar(this.health, this.data.MaxHP);
            attackCooldown = attackRate;

        }

        // Call this method to smoothly move the enemy to a target position
        public virtual void MoveToPosition(Vector3 target)
        {
            StopAllCoroutines();  // Stop any ongoing movement to avoid conflicts
            StartCoroutine(MoveToTargetAtFixedSpeed(target));
        }

        // Coroutine to translate the position at a constant speed
        private IEnumerator MoveToTargetAtFixedSpeed(Vector3 target)
        {
            // Store the initial position of the enemy
            Vector3 initialPosition = transform.position;

            // Calculate total time to travel based on speed
            float travelTime = 1 / movementSpeed;
            float elapsedTime = 0f;

            // Move the enemy towards the target position
            while (elapsedTime < travelTime)
            {
                // Calculate interpolation value based on elapsed time
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
            this.targets = detection.targets;
            
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
                if (movement.SimpleMove(this)) {
                    moveTimeRemaining = 1 / movementSpeed;
                }
            }
            // Update the timers
            moveTimeRemaining -= Time.deltaTime;
            attackCooldown -= Time.deltaTime;
        }

        protected virtual void Attack()
        {
            // Loop through all targets and deal damage
            foreach (Transform tower in targets)
            {
                Tower towerScript = tower.GetComponent<Tower>();
                if (towerScript != null)
                {
                    towerScript.TakeDamage(5);
                }
            }
        }

        // Method to deal damage to the enemy
        public virtual void TakeDamage(int damage)
        {
            StartCoroutine(UpdateHealthAfterDelay(damage, 0.04f));
        }

        // Coroutine to update the health after a delay
        private IEnumerator UpdateHealthAfterDelay(int damage, float delay)
        {
            yield return new WaitForSeconds(delay);
            this.health = this.health - damage;
            // healthBar.UpdateHealthBar(this.health, this.data.MaxHP);
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
