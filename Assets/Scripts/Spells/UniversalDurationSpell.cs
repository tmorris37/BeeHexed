using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UniversalDurationSpell<T> : DurationSpell {
    // the targets the spell will effect
    GameObject[] targets;
    // the tag with which to find each target object
    string targetTag;

    // Gets all targets of type T and their sprites, and then applies the spell effect to each
    void GetTargetsAndApply() {
        targets = GameObject.FindGameObjectsWithTag(targetTag);
        foreach (GameObject target in targets) {
            T entity = target.GetComponent<T>();
            SpriteRenderer sprite = target.GetComponent<SpriteRenderer>();
            if (sprite == null) sprite = target.GetComponentInChildren<SpriteRenderer>();
            if (entity == null) {
                throw new NullReferenceException("GetTargets: Entity was null in" + entity +"for this spell");
            } else if (sprite == null) {
                throw new NullReferenceException("GetTargets: Sprite was null in"  + entity + "for this spell");
            }
            ApplySpellEffect(entity, sprite);
        }
    }

    public override void StartEffect() {
        targetTag = GetTargetTag();
        GetTargetsAndApply();
    }

    public override void EndEffect() {
        foreach (GameObject target in targets) {
            if (target != null) {
                T entity = target.GetComponent<T>();
                SpriteRenderer sprite = target.GetComponent<SpriteRenderer>();
                if (sprite == null) sprite = target.GetComponentInChildren<SpriteRenderer>();
                if (entity == null) {
                    throw new NullReferenceException("EndEffect: Entity was null in" + entity +"for this spell");
                } else if (sprite == null) {
                    throw new NullReferenceException("EndEffect: Sprite was null in"  + entity + "for this spell");
                }
                EndEffect(entity, sprite);
            } 
        }
    }
    public abstract void EndEffect(T entity, SpriteRenderer sprite);

    // Apply the spell's effect to each entity of type T, sprite is provided for visual modification
    public abstract void ApplySpellEffect(T entity, SpriteRenderer sprite);


    // force subclass to define the tag
    public abstract string GetTargetTag();

}
