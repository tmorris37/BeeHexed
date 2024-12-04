using System.Collections;
using System.Collections.Generic;
using EnemyAndTowers;
using UnityEngine;

public class Providence : UniversalDurationSpell<Tower>
{
  // length of spell effect in seconds
  [SerializeField] float duration = 10f;
  // how long the effect has been active in seconds
  string targetingTag = "Tower";
  [SerializeField] float fireRateIncrease = 2f;

  public override void ApplySpellEffect(Tower entity, SpriteRenderer sprite)
  {
    // fireRate appears to work logically backwards right now
    entity.fireRate *= 1 / fireRateIncrease;
    entity.health = entity.data.MaxHealth;
    sprite.color = Color.yellow;
  }

  public override void EndEffect(Tower entity, SpriteRenderer sprite)
  {
    entity.fireRate *= fireRateIncrease;
    sprite.color = Color.white;
  }

  public override float GetDuration()
  {
    return duration;
  }

  public override string GetTargetTag()
  {
    return targetingTag;
  }
}
