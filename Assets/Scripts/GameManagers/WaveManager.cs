using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private bool DEBUG_MODE = false;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Spawner spawner;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private int numWaves = 3;

    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private float timeBetweenSpawns = 1f;
    [SerializeField] private float timeToRewardLoad = 3f;

    private int currentWave = 0;
    private bool lastEnemySpawned = false;

    [SerializeField] private NectarManager nectarManager;
    private DrawPileManager drawPileManager;
    private HandManager handManager;
    private VictoryManager victoryManager;
    private List<int> enemyIDs = new List<int>();
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
        yield return new WaitForSeconds(2);
        while (currentWave < numWaves)
        {
            currentWave++;
            waveText.text = "Wave: " + currentWave + "/" + numWaves;
            nectarManager.SetNectar(nectarManager.GetNectar() + 5);
            for (int i = 0; i < 3; i++) {
                drawPileManager.DrawCard(handManager);
            }
            for (int i = 0; i <= enemiesPerWave; i++)
            {
                // TODO: Make this not hardcoded
                int enemyID = UnityEngine.Random.Range(0, 4);
                if (enemyID == 3) {
                    enemyID = 1;
                } else {
                    enemyID = 0;
                }
                spawner.SpawnFromCaves(enemyID);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
            if (currentWave == numWaves) {
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