using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAndTowers {
    public class Fireball : MonoBehaviour
    {
        public int damage = 0;
        [SerializeField] GameObject explosion;

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("EnemyBody")) {
                GameObject expl = Instantiate(explosion, transform.localPosition, Quaternion.identity);
                expl.GetComponent<FireballExplosion>().damage = damage;
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

