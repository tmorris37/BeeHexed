using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAndTowers;

public class Blizzard : UniversalDurationSpell<Enemy>
{
  [SerializeField] float duration = 1f;
  [SerializeField] string setTag = "Enemy";
  [SerializeField] int damage = 5;

  public override void ApplySpellEffect(Enemy entity, SpriteRenderer sprite)
  {
    entity.TakeDamage(5);
    sprite.color = Color.blue;
  }

  public override void EndEffect(Enemy entity, SpriteRenderer sprite)
  {
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
