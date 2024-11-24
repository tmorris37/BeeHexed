using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GridSystem;

namespace EnemyAndTowers
{
    public class BeamerTower : Tower
    {
        public GameObject beam;

        public bool active;

        protected override void Start()
        {
            base.Start();
            
            fireCountdown = fireRate;
            //animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            this.targets = Detection.targets;
            if (this.health <= 0)
            {
                HexTile tile = GridManager.FetchTile(q,r,s);
                tile.LeaveTile(gameObject);
                Destroy(gameObject);
            }
            if (fireCountdown <= 0f && targets.Count > 0 && this.active)
            {
                DealDamage();
                Fire();
                fireCountdown = fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }

        private void Fire()
        {
            if (beam == null)
            {
                Debug.LogError("Beam prefab not assigned!");
                return;
            }

            // Instantiate the beam at the tower's position with the same rotation as the tower
            Vector3 offset = -this.transform.right * 12.5f; // Assuming the beam shoots along the tower's right direction
            GameObject instantiatedBeam = Instantiate(beam, this.transform.position + offset, this.transform.rotation);
            instantiatedBeam.transform.rotation = this.transform.rotation;
            Destroy(instantiatedBeam, 0.2f);
        }

        private void DealDamage()
        {
            foreach (Transform enemy in targets)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(1);
                    if (SFXManager.Instance != null)
                    {
                        SFXManager.Instance.PlayBeem();
                    }
                }
            }
        }
    }
}
