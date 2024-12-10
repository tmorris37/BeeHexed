using System.Collections;
using System.Collections.Generic;
using EnemyAndTowers;
using UnityEngine;

public class Wax : UniversalDurationSpell<Enemy>
{
    [SerializeField] string setTag = "Enemy";
    [SerializeField] float duration = 5f;
    public override void ApplySpellEffect(Enemy entity, SpriteRenderer sprite)
    {
        entity.movementSpeed *= 0.1f;
        sprite.color = Color.yellow;
    }

    public override void EndEffect(Enemy entity, SpriteRenderer sprite)
    {
        entity.movementSpeed *= 10f;
        sprite.color = Color.white;
    }

    public override float GetDuration()
    {
        return duration;
    }

    public override string GetTargetTag()
    {
        return setTag;
    }
}