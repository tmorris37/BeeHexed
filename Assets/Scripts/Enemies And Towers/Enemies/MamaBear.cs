using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAndTowers {
    public class MamaBear : Enemy
    {
        public int baseAttack;
        public float baseSpeed;
        public float scale = 1.0f;
        public float incement = 0.1f;

        protected override void Start()
        {
            base.Start();
            baseAttack = attackDamage;
            baseSpeed = movementSpeed;
        }

        private IEnumerator MoveToTargetAtFixedSpeed(Vector3 target, float speed)
        {
            Vector3 initialPosition = transform.position;
            float distanceToTarget = Vector3.Distance(initialPosition, target);

            // Calculate total time to travel based on speed
            float travelTime = distanceToTarget / speed;
            float elapsedTime = 0f;

            while (elapsedTime < travelTime)
            {
                // Calculate interpolation value based on elapsed time and speed
                float t = elapsedTime / travelTime;

                // Update position at a constant rate towards the target
                transform.position = Vector3.Lerp(initialPosition, target, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Set the final position exactly to the target
            transform.position = target;
        }

        public void IncreaseStats()
        {
            if (scale < 2.0f)
            {
                scale += incement;
                attackDamage = (int)(baseAttack * scale);
                // Temp fix for demo
                if (movementSpeed > 0.05) {
                    movementSpeed = baseSpeed * scale;
                }
            }
        }
    }
}

