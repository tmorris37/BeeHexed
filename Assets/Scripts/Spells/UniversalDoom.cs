using System.Collections;
using System.Collections.Generic;
using EnemyAndTowers;
using UnityEngine;

public class UniversalDoom : UniversalSpell<Enemy>
{
    public int damage = 25;
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
