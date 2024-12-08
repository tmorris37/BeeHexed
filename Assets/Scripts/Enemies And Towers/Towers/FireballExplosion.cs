using System.Collections;
using System.Collections.Generic;
using EnemyAndTowers;
using UnityEngine;

public class FireballExplosion : MonoBehaviour
{
    public int damage = 0;
    private bool damaged = false;
    public void End() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("EnemyBody") && !damaged) {
            if (SFXManager.Instance != null) {
                SFXManager.Instance.PlayFireballExplode();
            }
            collision.gameObject.GetComponentInParent<Enemy>().TakeDamage(damage);
            // this object lingers for the animation, but we don't actually want it to damage if an enemy enters the explosion after instantiated
            GetComponent<CircleCollider2D>().enabled = false;
            StartCoroutine(EndAnimation());
        }
    }

    private IEnumerator EndAnimation() {
        yield return new WaitForSeconds(5);
        End();
    }

}
