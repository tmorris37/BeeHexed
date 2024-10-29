using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAlgorithms : MonoBehaviour
{
    public string RandomMovement(string[] ValidDirections)
    {
        int DirectionIndex = Random.Range(0, ValidDirections.Length);
        return ValidDirections[DirectionIndex];
    }

    
}
