using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAndTowers;

public class Blizzard : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] enemies;
    float duration = 6f;
    float currTime = 0f;
    void Start()
    {
      enemies = GameObject.FindGameObjectsWithTag("Enemy");
      foreach (GameObject enemy in enemies) {
        Enemy e = enemy.GetComponent<Enemy>();
        SpriteRenderer s = enemy.GetComponent<SpriteRenderer>();
        e.TakeDamage(5);
        s.color = Color.blue;
      }
    }

    // Update is called once per frame
    void Update()
    {
        if (currTime < duration) {
        currTime += Time.deltaTime;
      } else {
        for (int i = 0; i < enemies.Length; i++) {
          if (enemies[i] != null) {
            SpriteRenderer s = enemies[i].GetComponent<SpriteRenderer>();
            if (s != null) {
              s.color = Color.white;
            }
          }
        }
        Destroy(gameObject);
      }
    }
}
