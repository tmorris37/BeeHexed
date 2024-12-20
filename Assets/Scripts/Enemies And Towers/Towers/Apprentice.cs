using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GridSystem;

namespace EnemyAndTowers
{
    public class Apprentice : Tower
    {
        public GameObject projectilePrefab;
        public float projectileSpeed = 10f;

        protected override void Start() {
            base.Start();
            fireCountdown = fireRate;
        }

        protected override void Update() {
            targets = detection.targets;
            base.Update();
            if (fireCountdown <= 0f && targets.Count > 0 && active) {
                Fire();
                fireCountdown = fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }

        private void Fire() {
            if (projectilePrefab == null) {
                Debug.LogError("Projectile prefab not assigned!");
                return;
            }

            // Use the tower's rotation to determine the direction
            Vector3 direction = transform.right; // Assuming the tower faces along its local right direction

            // Instantiate the projectile at the tower's position
            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            ApprenticeStinger projectile = proj.GetComponent<ApprenticeStinger>();
            projectile.damage = damage;
            projectile.velocity = projectileSpeed;

            // Rotate the projectile to face the firing direction
            projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

            // Set the velocity of the projectile
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null) {
                rb.velocity = -1 * direction * projectileSpeed;
            } else {
                Debug.LogError("Projectile does not have a Rigidbody2D!");
            }
        }

        public override bool IsRotatable() {
            return true;
        }
    }
}
