using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public GameObject enemy;
    public List<Transform> targets = new List<Transform>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tower"))
            {
                Debug.Log("Tower Detected");
                if (!targets.Contains(other.transform)) 
                {
                    targets.Add(other.transform);
                }
                Debug.Log(targets);
            }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Tower"))
        {
            targets.Remove(other.transform);
        }
    }
}