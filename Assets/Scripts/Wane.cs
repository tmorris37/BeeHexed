using System.Collections;
using System.Collections.Generic;
using EnemyAndTowers;
using UnityEngine;

public class Wane : MonoBehaviour
{
    // Start is called before the first frame update
    float duration = 6f;
    float currTime = 0f;
    GameObject[] enemies;
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Enemy e = enemy.GetComponent<Enemy>();
            SpriteRenderer s = enemy.GetComponent<SpriteRenderer>();
            if (e != null) {
                e.movementSpeed *= 0.1f;
            }
            if (s != null) {
                s.color = Color.yellow;
            }
        }
    }

    void Update() {
        if (currTime < duration)
        {
            currTime += Time.deltaTime;
        } else {
            for (int i = 0; i < enemies.Length; i++) {
                if (enemies[i] != null) {
                    Enemy e = enemies[i].GetComponent<Enemy>();
                    SpriteRenderer s = enemies[i].GetComponent<SpriteRenderer>();
                    if (e != null) {
                    e.movementSpeed *= 10f;
                    }
                    if (s != null) {
                    s.color = Color.white;
                    }
                }
            }
            Destroy(gameObject);
        }
    }

}
