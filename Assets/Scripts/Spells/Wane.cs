using System.Collections;
using System.Collections.Generic;
using EnemyAndTowers;
using UnityEngine;

public class Wane : UniversalDurationSpell<Enemy> {
    [SerializeField] float duration = 5f;
    [SerializeField] int damageDivisor = 2;
    [SerializeField] string setTag = "Enemy";

    public override void ApplySpellEffect(Enemy entity, SpriteRenderer sprite) {
        entity.attackDamage /= damageDivisor;
        sprite.color = Color.gray;
    }

    public override void EndEffect(Enemy entity, SpriteRenderer sprite) {
        entity.attackDamage *= damageDivisor;
        sprite.color = Color.white;
    }

    public override float GetDuration() {
        return duration;
    }

    public override string GetTargetTag() {
        return setTag;
    }
}
