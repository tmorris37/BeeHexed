using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UniversalSpell<T> : MonoBehaviour {
    // the targets the spell will effect
    GameObject[] targets;
    // the tag with which to find each target object
    string targetTag;
    // Start is called before the first frame update
    void Start() {
        targetTag = GetTargetTag();
        GetTargetsAndApply();
        Destroy(gameObject);
    }


    // Gets all targets of type T and their sprites, and then applies the spell effect to each
    void GetTargetsAndApply() {
        targets = GameObject.FindGameObjectsWithTag(targetTag);
        foreach (GameObject target in targets) {
            T entity = target.GetComponent<T>();
            SpriteRenderer sprite = target.GetComponent<SpriteRenderer>();
            ApplySpellEffect(entity, sprite);
        }
    }

    // Apply the spell's effect to each entity of type T, sprite is provided for visual modification
    public abstract void ApplySpellEffect(T entity, SpriteRenderer sprite);


    // Force subclass to define the tag with which to search for targets
    public abstract string GetTargetTag();
}
