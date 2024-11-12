using System.Collections;
using UnityEngine;

public class SmoothMover : MonoBehaviour
{
    public float movementDuration = 2f; // Total time to move to the target
    private Vector3 targetPosition;

    // Set the target position you want to move toward
    public void MoveToPosition(Vector3 target)
    {
        targetPosition = target;
        StartCoroutine(TranslateOverTime(targetPosition, movementDuration));
    }

    private IEnumerator TranslateOverTime(Vector3 target, float duration)
    {
        Vector3 initialPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate how far along we are in the movement based on time
            float t = elapsedTime / duration;

            // Linearly interpolate between the start and target positions
            transform.position = Vector3.Lerp(initialPosition, target, t);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the final position is set to the target
        transform.position = target;
    }
}