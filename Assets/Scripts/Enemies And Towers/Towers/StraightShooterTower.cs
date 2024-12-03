using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GridSystem;

namespace EnemyAndTowers
{
    public class StraightShooterTower : Tower
    {
        //public GameObject beam;
        public GameObject projectilePrefab;
        public float projectileSpeed = 10f;

        

        protected override void Start()
        {
            base.Start();
            
            fireCountdown = fireRate;
            //animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            this.targets = detection.targets;
            if (this.health <= 0)
            {
                HexTile tile = gridManager.FetchTile(q,r,s);
                tile.LeaveTile(gameObject);
                Destroy(gameObject);
            }
            //Debug.Log("ACTIVE is: " + this.active);
            if (fireCountdown <= 0f && targets.Count > 0 && this.active)
            {
                //DealDamage();
                Fire();
                fireCountdown = fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }

        private void Fire()
        {
            if (projectilePrefab == null)
            {
                Debug.LogError("Projectile prefab not assigned!");
                return;
            }

    // Use the tower's rotation to determine the direction
    Vector3 direction = transform.right; // Assuming the tower faces along its local right direction

    // Instantiate the projectile at the tower's position
    GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    SSbullet projectile = proj.GetComponent<SSbullet>();
    projectile.tower = this;

    // Rotate the projectile to face the firing direction
    projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

    // Set the velocity of the projectile
    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.velocity = -1 * direction * projectileSpeed;
    }
    else
    {
        Debug.LogError("Projectile does not have a Rigidbody2D!");
    }
}

        public void DealDamage()
        {
            Transform target = targets[0];
            Enemy enemy = target.GetComponent<Enemy>();
            enemy.TakeDamage(1);
        }
    }
}
