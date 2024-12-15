using UnityEngine;

namespace EnemyAndTowers
{
    public class ApprenticeStinger : MonoBehaviour
    {
        public int damage = 0; // Damage dealt to the enemy
        public Apprentice tower;

        public float velocity; // Bullet velocity

        // Reference to the Magic Effect Prefab
        public GameObject magicEffectPrefab;

        private GameObject magicEffect; // Store reference to the spawned magic effect
        public bool DEBUG_MODE = false;

        public void Start() {
            SpawnMagicEffect();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("EnemyBody")) {
                if (DEBUG_MODE) Debug.Log("Bullet hit the enemy and deals damage.");
                
                collision.gameObject.GetComponentInParent<Enemy>().TakeDamage(damage);

                // Stop the magic effect from moving
                if (magicEffect != null) {
                    Rigidbody2D magicEffectRb = magicEffect.GetComponent<Rigidbody2D>();
                    if (magicEffectRb != null) magicEffectRb.isKinematic = true;      // Make it stationary

                    // Optionally, stop particle emission if the effect uses particles
                    ParticleSystem particleSystem = magicEffect.GetComponent<ParticleSystem>();
                    if (particleSystem != null) {
                        var emission = particleSystem.emission;
                        emission.enabled = false;
                    }
                }
                // Destroy the stinger
                Destroy(gameObject);
            }
        }

        private void SpawnMagicEffect() {
            // Spawn the magic effect at the bullet's position and rotation
            magicEffect = Instantiate(
                magicEffectPrefab,
                transform.position,
                transform.rotation
            );

            // Set the initial velocity of the magic effect to match the bullet's
            Rigidbody2D rb = magicEffect.GetComponent<Rigidbody2D>();
            Rigidbody2D mine = GetComponent<Rigidbody2D>();
            if (rb != null && mine != null) {
                rb.velocity = mine.velocity;
            }
        }

        private void OnBecameInvisible() {
            // Destroy the bullet if it goes out of camera view
            Destroy(gameObject);
        }
    }
}



