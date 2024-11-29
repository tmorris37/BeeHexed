using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EnemyAndTowers
{
    public class TowerProjectile : MonoBehaviour
    {
        private Transform target;
        public float speed = 5f;

        // Set the target for the projectile
        public void Seek(Transform target)
        {
            this.target = target;
        }

        private void Update()
        {
            if (target == null)
            {
                Destroy(gameObject); // Destroy the projectile if there's no target
                return;
            }

            // Calculate direction towards the target and move the projectile
            Vector2 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);

            // Destroy projectile if it gets very close to the target (within 0.1 units)
            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                Destroy(gameObject);
                // Optionally, you can add an effect here to represent the hit
            }
        }
    }
}