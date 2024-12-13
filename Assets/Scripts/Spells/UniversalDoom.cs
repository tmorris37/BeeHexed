using System.Collections;
using System.Collections.Generic;
using EnemyAndTowers;
using UnityEngine;

public class UniversalDoom : UniversalSpell<Enemy>
{
    public int damage = 50;
    public override void ApplySpellEffect(Enemy entity, SpriteRenderer sprite) {
        if (SFXManager.Instance != null) {
            SFXManager.Instance.PlayThunderclap();
        }
        entity.TakeDamage(damage);
    }

    public override string GetTargetTag() {
        return "Enemy";
    }
}
