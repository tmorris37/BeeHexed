using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Spawner spawner;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private int numWaves = 3;

    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private float timeBetweenSpawns = 1f;
    [SerializeField] private float timeToRewardLoad = 3f;

    private int currentWave = 0;
    private bool waveInProgress = false;

    [SerializeField] private NectarManager nectarManager;
    private DrawPileManager drawPileManager;
    private HandManager handManager;
    private VictoryManager victoryManager;
    private List<int> enemyIDs = new List<int>();
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

            waveInProgress = false;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
        yield return new WaitForSeconds(10f);
        Debug.Log("Waves are done");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) {
            victoryManager.Win();
            yield return new WaitForSeconds(timeToRewardLoad);
            SceneManager.LoadScene("Rewards");
        } else {
            victoryManager.Lose();
            yield return new WaitForSeconds(timeToRewardLoad);
            SceneManager.LoadScene("MainMenu");
        }
        
    }
}