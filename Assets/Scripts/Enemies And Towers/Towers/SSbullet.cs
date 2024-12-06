using UnityEngine;

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
}
