using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GridSystem;

namespace EnemyAndTowers
{
    public class CenterTower : Tower
    {
        PlayerHealthManager playerHealthManager;
        protected override void Start()
        {
            base.Start();
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
        }
    }
}
