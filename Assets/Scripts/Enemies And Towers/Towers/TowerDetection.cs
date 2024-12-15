using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDetection : MonoBehaviour {
    public GameObject tower;
    public List<Transform> targets = new List<Transform>();
    public bool DEBUG_MODE = false;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("EnemyBody")) {
            Transform enemyTransform = other.transform.root;
            if (!targets.Contains(enemyTransform)) targets.Add(enemyTransform);
            if (DEBUG_MODE) Debug.Log(targets.Count);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("EnemyBody")) {
            // Get the root GameObject of the tower
            Transform enemyTransform = other.transform.root;

            targets.Remove(enemyTransform);
        }
    }
}