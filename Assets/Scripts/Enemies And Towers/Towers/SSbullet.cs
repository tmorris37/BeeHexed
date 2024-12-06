/*using UnityEngine;

namespace EnemyAndTowers
{
    public class SSbullet : MonoBehaviour
    {
        public int damage = 10; // Damage dealt to the enemy
        public StraightShooterTower tower;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("EnemyBody"))
            {
                //Enemy enemy = collision.GetComponent<Enemy>();
                // Deal damage to the enemy
                //enemy.TakeDamage(damage);
                tower.DealDamage();
                ParticleSystem particleEffect = this.GetComponentInChildren<ParticleSystem>();
                particleEffect.transform.parent = null;
                //particleEffect.Stop();
                var emission = particleEffect.emission;
                emission.enabled = false;
                Destroy(particleEffect.gameObject, particleEffect.main.duration);
                // Destroy the projectile after it hits the enemy
                Destroy(gameObject);
                
            }
            // Check if the projectile collided with an enemy
            //Enemy enemy = collision.GetComponent<Enemy>();
            
        }

        private void OnBecameInvisible()
        {
            // Destroy the projectile if it goes out of camera view
            Destroy(gameObject);
        }
    }
}*/
using UnityEngine;

namespace EnemyAndTowers
{
    public class SSbullet : MonoBehaviour
    {
        public int damage = 10; // Damage dealt to the enemy
        public StraightShooterTower tower;

        public float velocity; // Bullet velocity

        // Reference to the Magic Effect Prefab
        public GameObject magicEffectPrefab;

        private GameObject magicEffect; // Store reference to the spawned magic effect

        public void Start()
        {
            SpawnMagicEffect();
        }

        

        /*private void Update()
        {
            MoveMagicEffect();
        }*/

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("EnemyBody"))
            {
                // Deal damage to the enemy
                tower.DealDamage();

                // Detach and stop spawning new particles
                if (magicEffect != null)
                {
                    ParticleSystem particleSystem = magicEffect.GetComponent<ParticleSystem>();
                    if (particleSystem != null)
                    {
                        var emission = particleSystem.emission;
                        emission.enabled = false;
                        Destroy(magicEffect, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
                    }
                    else
                    {
                        Destroy(magicEffect, 2f);
                    }
                }
                Rigidbody2D rb = magicEffect.GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;
                // Destroy the projectile after it hits the enemy
                Destroy(gameObject);
            }
        }

        private void SpawnMagicEffect()
{
    // Spawn the effect at the bullet's position and rotation
    GameObject magicEffect = Instantiate(
        magicEffectPrefab, 
        transform.position, 
        transform.rotation // Use the bullet's rotation
    );
    Rigidbody2D rb = magicEffect.GetComponent<Rigidbody2D>();
    Rigidbody2D mine = this.GetComponent<Rigidbody2D>();
    rb.velocity = mine.velocity;
}
/*private void MoveMagicEffect()
{
    magicEffect.transform.position = transform.position;
}*/

   

    


        

        private void OnBecameInvisible()
        {
            // Destroy the projectile if it goes out of camera view
            Destroy(gameObject);
        }
    }

}



