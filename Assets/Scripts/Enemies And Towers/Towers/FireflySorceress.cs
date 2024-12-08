using System;
using System.Collections;
using System.Collections.Generic;
using EnemyAndTowers;
using UnityEngine;

public class FireflySorceress : Tower
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;

    protected override void Start() {
        base.Start();
        // TODO: Update everything to make the rate a rate and not a cooldown
        //rotatable = true;
        fireCountdown = fireRate;
    }

    protected override void Update()
    {
        base.Update();
        if (fireCountdown <= 0f && targets.Count > 0 && active)
            {
                Fire();
                fireCountdown = fireRate;
            }
            fireCountdown -= Time.deltaTime;
    }

    private void Fire() {
        if (projectilePrefab == null)
        {
            throw new NullReferenceException("Projectile prefab not assigned!");
        }

        // Use the tower's rotation to determine the direction
        Vector3 direction = transform.right; // Assuming the tower faces along its local right direction

        // Instantiate the projectile at the tower's position
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Fireball projectile = proj.GetComponent<Fireball>();
        projectile.tower = this;

        // Rotate the projectile to face the firing direction
        projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        // Set the velocity of the projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.velocity = -(direction * projectileSpeed);
        }
        else {
            throw new NullReferenceException("Projectile does not have a Rigidbody2D!");
        }
    }

    public void DealDamage() {
        Transform target = targets[0];
        Enemy enemy = target.GetComponent<Enemy>();
        enemy.TakeDamage(damage);
    }

    public override bool IsRotatable() {
        return true;
    }
}
