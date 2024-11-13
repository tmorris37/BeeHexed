using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Level0Spawner spawner;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private int numWaves = 3;

    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private float timeBetweenSpawns = 1f;

    private int currentWave = 0;
    private bool waveInProgress = false;

    void Start()
    {
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        yield return new WaitForSeconds(2);
        while (currentWave < numWaves)
        {
            currentWave++;
            waveInProgress = true;

            for (int i = 0; i <= enemiesPerWave; i++)
            {
                // Spawn an enemy using the Level0Spawner
                spawner.SpawnFromCaves(0);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            waveInProgress = false;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }
}