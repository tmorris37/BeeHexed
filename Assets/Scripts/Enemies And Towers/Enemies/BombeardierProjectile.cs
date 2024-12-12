using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EnemyAndTowers
{
    public class BombeardierProjectile : MonoBehaviour
    {
        private Transform target;
        public int bombDamage = 1;
        public float speed = 5f;
        private Vector3 startScale; // Initial scale of the bomb
        private float totalDistance; // Total distance to the target
        public GameObject explosionPrefab;

        // Set the target for the projectile
        public void Seek(Transform target)
        {
            this.target = target;
            startScale = transform.localScale; // Record initial scale
            if (target != null)
                totalDistance = Vector3.Distance(transform.position, target.position); // Calculate total travel distance
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject entity = other.transform.parent.gameObject;
            if (entity.CompareTag("Tower") && entity.transform == target)
            {
                entity.GetComponent<Tower>().TakeDamage(bombDamage);
                if (SFXManager.Instance != null)
                {
                    SFXManager.Instance.PlayExplosion();
                }
                

                GameObject explosion = Instantiate(explosionPrefab, target.position, Quaternion.identity);
                // BombeardierProjectile bombScript = ex.GetComponent<BombeardierProjectile>();
                // bombScript.bombDamage = attackDamage;
                // bombScript?.Seek(target.transform);
                // if (SFXManager.Instance != null)
            }
        }
        private void Update()
        {
            // if (target == null)
            // {
            //     Destroy(gameObject); // Destroy the projectile if there's no target
            //     return;
            // }
            if(transform == null || target == null)
            {
                return;
            }

            // Calculate direction towards the target and move the projectile
            Vector2 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);

            // Update the bomb's size based on progress to the target
            float currentDistance = Vector3.Distance(transform.position, target.position);
            float progress = 1 - (currentDistance / totalDistance); // Progress from 0 (start) to 1 (target)
            AnimateSize(progress);

            // Destroy projectile if it gets very close to the target (within 0.1 units)
            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                Destroy(gameObject);
                // Optionally, you can add an effect here to represent the hit
            }
        }

        private void AnimateSize(float progress)
        {
            // Size animation logic
            if (progress <= 0.5f)
            {
                // Increase size to double at halfway point
                transform.localScale = Vector3.Lerp(startScale, startScale * 2, progress * 2);
            }
            else
            {
                // Decrease size back to original size from halfway to the end
                transform.localScale = Vector3.Lerp(startScale * 2, startScale, (progress - 0.5f) * 2);
            }
        }
    }
}