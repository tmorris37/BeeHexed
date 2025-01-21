using System;
using System.Collections;
using System.Collections.Generic;
using EnemyAndTowers;
using UnityEditor.Animations;
using UnityEngine;

public class UniversalDoom : UniversalSpell<Enemy>
{
    public int damage = 25;
    public GameObject thunderboltPrefab;
    public GameObject fireboltPrefab;
    public GameObject iceboltPrefab;
    public float bolt_offset = 2.5f;
    public override void ApplySpellEffect(Enemy entity, SpriteRenderer sprite) {
        if (SFXManager.Instance != null) {
            SFXManager.Instance.PlayThunderclap();
        }
        Vector3 strikePosition = new Vector3(entity.transform.position.x, entity.transform.position.y + bolt_offset, entity.transform.position.z); 
        Animate(strikePosition);
        entity.TakeDamage(damage);
    }

    private void Animate(Vector3 strikePosition) {
        System.Random randy = new();
        int whichBolt = randy.Next() % 3; // witch bolt?
        GameObject boltPrefab;
        string animation;
        if (whichBolt == 0) {
            boltPrefab = thunderboltPrefab;
            animation = "ThunderboltStrike";
        } else if (whichBolt == 1) {
            boltPrefab = fireboltPrefab;
            animation = "FireboltStrike";
        } else {
            boltPrefab = iceboltPrefab;
            animation = "IceboltStrike";
        }
        GameObject boltInstance = Instantiate(boltPrefab, strikePosition, Quaternion.identity);
        boltInstance.GetComponent<Animator>().Play(animation, -1);
    }



    public override string GetTargetTag() {
        return "Enemy";
    }
}
