using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GridSystem;

namespace EnemyAndTowers
{
    public class CenterTower : Tower
    {
        public float nectarInterval;
        public GameObject nectarPrefab;
        private float timer = 0f;
        PlayerHealthManager playerHealthManager;
        NectarManager nectarManager;
        protected override void Start()
        {
            base.Start();
            nectarManager = FindObjectOfType<NectarManager>();
            playerHealthManager = FindObjectOfType<PlayerHealthManager>();
            if (playerHealthManager == null)
            {
                Debug.LogError("PlayerHealthManager not found in the scene!");
            }
        }

        public override void TakeDamage(int damage)
        {
            playerHealthManager.TakeDamage(damage);
        }

        protected override void Update()
        {
            if (this.health <= 0)
            {
                HexTile tile = gridManager.FetchTile(q,r,s);
                tile.LeaveTile(gameObject);
                Destroy(gameObject);
            }
            if (timer >= nectarInterval)
            
            {
                addNectar();
                timer = 0f;
            }
            timer += Time.deltaTime;
        }

        private void addNectar()
        {
            Instantiate(nectarPrefab, transform.position, Quaternion.identity);
            int currNectar = nectarManager.GetNectar();
            nectarManager.SetNectar(currNectar + 1);
        }
    }
}
