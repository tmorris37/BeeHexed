using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private bool DEBUG_MODE = false;
    [SerializeField] private int STARTUP_TIME = 10;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Spawner spawner;

    [SerializeField] private float timeBetweenWaves;

    private bool lastEnemySpawned = false;

    [SerializeField] private NectarManager nectarManager;
    private DrawPileManager drawPileManager;
    private HandManager handManager;
    private VictoryManager victoryManager;
    [SerializeField] private List<Wave> waves;

    void Awake() {
        drawPileManager = FindObjectOfType<DrawPileManager>();
        handManager = FindObjectOfType<HandManager>();
    }
    void Start()
    {
        waveText.text = "Waves not started";
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        int currentWave = 0;
        yield return new WaitForSeconds(STARTUP_TIME);
        while (currentWave < waves.Count)
        {
            currentWave++;
            int minEnemies = waves[currentWave-1].avgEnemies - waves[currentWave-1].enemySpread;
            int maxEnemies = waves[currentWave-1].avgEnemies + waves[currentWave-1].enemySpread;
            // Get the number of enemies to spawn
            int numEnemies = UnityEngine.Random.Range(minEnemies, maxEnemies);
            waveText.text = "Wave: " + currentWave + "/" + waves.Count;
            // Reward the player with 2 nectar at the start of each wave
            nectarManager.SetNectar(nectarManager.GetNectar() + 2);
            // Draw 3 cards at the start of each wave
            for (int i = 0; i < 3; i++) {
                drawPileManager.DrawCard(handManager);
            }
            // Spawn enemies
            for (int i = 0; i <= numEnemies; i++)
            {
                // Generate a random float for time between spawns
                float avgTimeBetweenSpawns = waves[currentWave-1].avgTimeBetweenSpawns;
                float timeBetweenSpawns = UnityEngine.Random.Range(avgTimeBetweenSpawns - 0.5f, avgTimeBetweenSpawns + 0.5f);
                string enemyType = null;
                // Determine which enemy to spawn based on spawn frequencies
                for (int j = 0; j <= numEnemies; j++) {
                    // Generate a random float between 0 and 1
                    float randomValue = UnityEngine.Random.value;
                    float cumulativeProbability = 0f;

                    // Determine which enemy to spawn based on spawn frequencies
                    foreach (var enemyData in waves[currentWave-1].enemies)
                    {
                        cumulativeProbability += enemyData.spawnFrequency;
                        if (randomValue <= cumulativeProbability)
                        {
                            enemyType = enemyData.enemyType;
                            break;
                        }
                    }

                    // Ensure an enemy was selected
                    if (enemyType == null)
                    {
                        Debug.LogError("No enemy selected! Check if frequencies sum to 1 for the wave.");
                        continue;
                    }
                }
                // Call the spawner to spawn the enemy
                spawner.SpawnFromCaves(enemyType);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
            if (currentWave == waves.Count) {
                lastEnemySpawned = true;
            } else {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
        waveText.text = "Waves Complete!";
        if (DEBUG_MODE) Debug.Log("Waves are done");
    }

    public bool GetLastEnemySpawned() {
        return lastEnemySpawned;
    }

    public void Stop() {
        StopAllCoroutines();
    }
}

[Serializable]
public class EnemySpawnData
{
    public float spawnFrequency;
    public string enemyType;
}

[Serializable]
public class Wave
{
    public List<EnemySpawnData> enemies;
    public int avgEnemies;
    public int enemySpread;
    public float avgTimeBetweenSpawns;
}