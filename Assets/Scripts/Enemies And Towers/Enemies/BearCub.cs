using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GridSystem;

namespace EnemyAndTowers
{
    public class BearCub : Enemy
    {
        protected override void Start()
        {
            base.Start();
        }

        // Coroutine to translate the position at a constant speed
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
    }
}
