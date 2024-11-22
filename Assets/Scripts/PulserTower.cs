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
            if (this.health <= 0)
            {
                HexTile tile = GridManager.FetchTile(q,r,s);
                tile.LeaveTile(gameObject);
                Destroy(gameObject);
            }
            // Pulse effect logic
            if (pulseCountdown <= 0f)
            {
                PulseDamage();
                pulseCountdown = pulseRate;
            }
            pulseCountdown -= Time.deltaTime;
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                targets.Remove(other.transform);
                if (targets.Count == 0)
                {
                    circleAnimator.Play("newState");
                }
            }
        }

        private void PulseDamage()
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
