using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public GameObject enemy; // Reference to the parent enemy object

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tower"))
        {
            Debug.Log($"Detected: {collision.name}");
            // Trigger enemy attack logic
            //enemy.GetComponent<EnemyAttack>().StartAttack(collision.gameObject);
        }
    }
}