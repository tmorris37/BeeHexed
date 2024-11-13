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

    [SerializeField] private NectarManager nectarManager;
    private DrawPileManager drawPileManager;
    private HandManager handManager;
    private VictoryManager victoryManager;
    void Awake() {
      drawPileManager = FindObjectOfType<DrawPileManager>();
      handManager = FindObjectOfType<HandManager>();
      victoryManager = FindObjectOfType<VictoryManager>();
    }
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
            nectarManager.SetNectar(nectarManager.GetNectar() + 5);
            for (int i = 0; i < 3; i++) {
              drawPileManager.DrawCard(handManager);
            }
            for (int i = 0; i <= enemiesPerWave; i++)
            {
                // Spawn an enemy using the Level0Spawner
                spawner.SpawnFromCaves(0);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            waveInProgress = false;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
        yield return new WaitForSeconds(10f);
        Debug.Log("Waves are done");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) {
          victoryManager.Win();
        } else {
          victoryManager.Lose();
        }
        
    }
}