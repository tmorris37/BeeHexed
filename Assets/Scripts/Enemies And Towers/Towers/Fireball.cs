using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAndTowers {
    public class Fireball : MonoBehaviour
    {
        public FireflySorceress tower;

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("EnemyBody"))
            {
                // Deal damage to the enemy
                tower.DealDamage();
                // Destroy the projectile after it hits the enemy
                Destroy(gameObject);
            }
        }

        private void OnBecameInvisible() {
            // Destroy the projectile if it goes out of camera view
            Destroy(gameObject);
        }
    }
}

