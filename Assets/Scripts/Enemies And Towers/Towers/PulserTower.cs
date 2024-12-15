using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GridSystem;

namespace EnemyAndTowers
{
    public class PulserTower : Tower
    {

        public GameObject projectilePrefab;

        private float pulseCountdown; // Countdown for the pulse effect
        [SerializeField] private Animator circleAnimator;

        //public Animator animator;

        protected override void Start() {
            base.Start();
            pulseCountdown = fireRate; // Initialize the pulse countdown
        }

        protected override void Update() {
            base.Update();
            targets = detection.targets;
            // Pulse effect logic
            if (pulseCountdown <= 0f)
            {
                DealDamage();
                pulseCountdown = fireRate;
            }
            pulseCountdown -= Time.deltaTime;
        }

        private void DealDamage() {
            foreach (Transform enemy in targets)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null) {
                    enemyScript.TakeDamage(damage);
                    if (circleAnimator != null) circleAnimator.SetTrigger("PulseEffect");
                    if (SFXManager.Instance != null) SFXManager.Instance.PlayPulse();
                }
            }
        }
    }
}
