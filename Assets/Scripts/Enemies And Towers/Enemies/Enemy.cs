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
            i,    // Inward
            o,     // Outward
            nullDir // No direction
        }
        // [SerializeField] protected FloatingHealthBar healthBar;
    public string enemyType;                // Type of enemy
        public int health;                  // Current health of the enemy
        public float movementSpeed = 0.5f;  // Speed at which the enemy moves (tiles per second)
        public float moveSpeedLast;         // Speed of the enemy last update
        public float moveTimeRemaining;     // Time remaining to move to the next tile
        public float attackRate = 1f;       // Time between attacks
        public float attackCooldown;        // Time remaining before the enemy can attack again
        public int attackDamage;            // Damage dealt by the enemy
        public bool isPaused = false;       // Whether the enemy is paused
        public Vector3 targetPositionXY;      // Position the enemy is moving towards
        public MovementAlgorithms movement; // Movement algorithms for the enemy
        public EnemyData data;              // Data about the enemy
        public EnemyDetection detection;    // Detection script for the enemy
        public List<Transform> targets;     // List of targets in range
        public InertiaDirection inertiaIO;    // Direction of inertia for the enemy (in/out)
        public InertiaDirection inertiaDir;    // Direction of inertia for the enemy (cw/ccw)
        public InertiaDirection desiredInertiaDir;    // Desired direction of inertia for the enemy (cw/ccw)
        public InertiaDirection desiredInertiaIO;    // Desired direction of inertia for the enemy (in/out)

        public List<(int, int, int)> DijkstraMoves;  // A List of the moves to be taken, calculated by Dijkstra

        protected virtual void Start()
        {
            // Figure out the movement algorithm for the enemy
            this.DijkstraMoves = movement.DijkstraInitialize(this);
            
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
            var FileData = Resources.Load<TextAsset>("Enemies/" + enemyType);

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
                Debug.Log("Unable to load Enemy:" + enemyType);
            }
            this.health = this.data.MaxHealth;
            this.movementSpeed = this.data.Speed;
            this.attackDamage = this.data.Attacks[0].DamageAmount;
            this.attackRate =  1 / this.data.Attacks[0].AttackRate;
            targetPositionXY = transform.position;
            attackCooldown = attackRate;

            inertiaDir = InertiaDirection.nullDir;
            inertiaIO = InertiaDirection.nullDir;
            desiredInertiaDir = InertiaDirection.nullDir;
            desiredInertiaIO = InertiaDirection.nullDir;
            
            // healthBar = GetComponentInChildren<FloatingHealthBar>();
            // healthBar.UpdateHealthBar(this.health, this.data.MaxHP);

        }

        // Call this method to smoothly move the enemy to a target position
        public virtual void MoveToPosition()
        {
            StopAllCoroutines();  // Stop any ongoing movement to avoid conflicts
            StartCoroutine(MoveToTargetAtFixedSpeed());
        }

        // Coroutine to translate the position at a constant speed
        private IEnumerator MoveToTargetAtFixedSpeed()
        {
            // Store the initial position of the enemy
            Vector3 initialPosition = transform.position;

            // Calculate the distance to the target position
            float distance = Vector3.Distance(initialPosition, targetPositionXY);

            // Calculate total time to travel based on speed
            float travelTime = distance / movementSpeed;
            float elapsedTime = 0f;
            moveTimeRemaining = travelTime;


            // Move the enemy towards the target position
            while (elapsedTime < travelTime)
            {
                // Calculate interpolation value based on elapsed time
                float t = elapsedTime / travelTime;

                // Update position at a constant rate towards the target
                transform.position = Vector3.Lerp(initialPosition, targetPositionXY, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Set the final position exactly to the target
            transform.position = targetPositionXY;
        }

        protected virtual void Update()
        {
            if (isPaused)
            {
                return;
            }

            // Check if the movement speed has changed and update the movement
            if (moveSpeedLast != movementSpeed)
            {
                MoveToPosition();
            }
            
            // Update the list of targets from the detection script
            targets = detection.targets;
            
            // Check if there are any targets in range and attack if off cooldown
            if (attackCooldown <= 0f && targets.Count > 0)
            {
                if (DEBUG) {
                    Debug.Log("Targets identified, time to attack");
                }
                Attack();
                attackCooldown = attackRate;
            }
            // Try and move towards the next tile if off cooldown and at the target position
            if (targetPositionXY == transform.position && moveTimeRemaining <= 0f)
            {
                if (DEBUG) {
                    Debug.Log("No targets identified, time to move");
                }
                // Only reset the moveTimeRemaining if the enemy actually started moving
                if (movement.DijkstraMove(this, DijkstraMoves)) {
                }
            }
            // Update the timers
            moveTimeRemaining -= Time.deltaTime;
            attackCooldown -= Time.deltaTime;
            // Store the last movement speed
            moveSpeedLast = movementSpeed;
        }

        protected virtual void Attack()
        {
            // Loop through all targets and deal damage
            foreach (Transform tower in targets)
            {
                Tower towerScript = tower.GetComponent<Tower>();
                if (towerScript != null)
                {
                    towerScript.TakeDamage(attackDamage);
                    if (towerScript.GetComponent<BearricadeTower>() != null)
                    {
                        this.TakeDamage(1);
                    }
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

        public virtual void Stop() {
            StopAllCoroutines();
            movementSpeed = 0;
            attackRate = 0;
        }
    }

    #region JSON Data Structures

    [System.Serializable]
    public class EnemyData
    {
        public int MaxHealth { get; set; }
        public float Speed { get; set; }
        public IList<Attack> Attacks { get; set; }
        public string MovementType { get; set; }
        // More fields based on what we need to store about an enemy
    }

    [System.Serializable]
    public class Attack
    {
        public string DamageType { get; set; }
        public int DamageAmount { get; set; }
        public float AttackRate { get; set; }
        // More fields based on attack type
    }
    #endregion
}
