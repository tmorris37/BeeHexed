using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GridSystem;

namespace EnemyAndTowers
{
    public class PulserTower : Tower
    {
        public float pulseRate = 0.5f;

        public GameObject projectilePrefab;

        private float pulseCountdown; // Countdown for the pulse effect
        [SerializeField] private Animator circleAnimator;

        //public Animator animator;

        protected override void Start()
        {
            base.Start();
            
            pulseCountdown = pulseRate; // Initialize the pulse countdown
        }

        protected override void Update()
        {
            this.targets = detection.targets;
            if (this.health <= 0)
            {
                HexTile tile = gridManager.FetchTile(q,r,s);
                tile.LeaveTile(gameObject);
                Destroy(gameObject);
            }
            // Pulse effect logic
            if (pulseCountdown <= 0f)
            {
                DealDamage();
                pulseCountdown = pulseRate;
            }
            pulseCountdown -= Time.deltaTime;
        }

        private void DealDamage()
        {
            foreach (Transform enemy in targets)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(1);
                    if (circleAnimator != null)
                    {
                        circleAnimator.SetTrigger("PulseEffect");
                    }
                    if (SFXManager.Instance != null)
                    {
                        SFXManager.Instance.PlayPulse();
                    }
                }
            }
        }
    }
}
