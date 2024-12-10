using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public GameObject enemy;
    public List<Transform> targets = new List<Transform>();
    public bool DEBUG_MODE = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TowerBody"))
        {
            Transform towerTransform = other.transform.root;
            if (!targets.Contains(towerTransform))
            {
                targets.Add(towerTransform);
            }
            if (DEBUG_MODE) 
                Debug.Log(targets);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TowerBody"))
        {
            // Get the root GameObject of the tower
            Transform towerTransform = other.transform.root;

            targets.Remove(towerTransform);
        }
    }
}