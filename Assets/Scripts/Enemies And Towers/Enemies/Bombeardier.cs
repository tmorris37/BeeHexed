using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GridSystem;

namespace EnemyAndTowers
{
    public class Bombeardier : Enemy
    {
        public GameObject bombPrefab;
        
        protected override void Start() {
            base.Start();
        }

        // Deal damage to first target in range
        // Assumes that the attackCooldown is off and resets it
        protected override void Attack() {
            Transform target = targets.First();

            GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            BombeardierProjectile bombScript = bomb.GetComponent<BombeardierProjectile>();
            bombScript.bombDamage = attackDamage;
            bombScript?.Seek(target.transform);
            if (SFXManager.Instance != null) {
                SFXManager.Instance.PlayToss();
            }
            attackCooldown = attackRate;
        }
    }
}
