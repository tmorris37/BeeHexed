using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using GridSystem;
using EnemyAndTowers;

    public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject EnemyPrefab;

    [SerializeField] public GridManager GridManager;
    [SerializeField] public CaveGenerator CaveGenerator;


    [SerializeField] public GameObject Cheerios;

    [SerializeField] public GameObject CavePrefab;

    [SerializeField] public int Radius;
    [SerializeField] public bool DEBUG;

    private MovementAlgorithms Movement;
    private float timer = 0f;
    public float moveInterval;

    // List to store cave positions
    private List<Vector3> CavePositions = new List<Vector3>();

    // List to store all spawned enemies
    private List<Enemy> enemies = new List<Enemy>();

    void Start()
    {
        this.Movement = new MovementAlgorithms(GridManager, DEBUG);
        this.CavePositions = CaveGenerator.CavePositions;
        SpawnCaves();
    }

    private void SpawnCheerios()
    {
      GameObject Che = Instantiate(this.Cheerios);
      HexTile spot = this.GridManager.FetchTile(0, 0, 0);
      spot.EnterTile(Che);
    }

    public void SpawnFromCaves(int EnemyID)
    {
        Vector3 randomCavePosition = CavePositions[UnityEngine.Random.Range(0, CavePositions.Count)];
        int q = (int)randomCavePosition.x;
        int r = (int)randomCavePosition.y;
        int s = (int)randomCavePosition.z;
        // Spawn Unity Object with Enemy script (Prefab)
        GameObject NewEnemy = Instantiate(EnemyPrefab);
        Enemy newEnemyComponent = NewEnemy.GetComponent<Enemy>();
    
        if (newEnemyComponent != null)
        {
            newEnemyComponent.EnemyID = EnemyID;
            newEnemyComponent.SetQRS(q, r, s);
            (float x, float y) = this.GridManager.QRStoXY(q, r, s);
            newEnemyComponent.transform.position = new Vector3(x, y, 0);
            newEnemyComponent.GridManager = this.GridManager;
            newEnemyComponent.GridRadius = this.GridManager.GridRadius;
            
            // Add the new enemy to the list of enemies
            enemies.Add(newEnemyComponent);
        }
    }

    public void SpawnCaves()
    {
        foreach (Vector3 CavePosition in CavePositions)
        {
            (float x, float y) = GridManager.QRStoXY((int)CavePosition.x, (int)CavePosition.y, (int)CavePosition.z);
            GameObject newCave = Instantiate(CavePrefab);
            newCave.transform.position = new Vector3(x, y, 0);
        }
        if (DEBUG)
        {
            foreach (var position in CavePositions)
            {
                Debug.Log("Cave position: " + position);
            }
        }
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= moveInterval)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy != null)
                {
                    Movement.SimpleMove(enemy);
                }
            }
            timer = 0f;

        }
        // Remove and destroy any enemies with 0 or less health
        enemies.RemoveAll(enemy =>
        {
            if (enemy != null && enemy.health <= 0)
            {                
                HexTile tile = GridManager.FetchTile(enemy.q, enemy.r, enemy.s);
                tile.LeaveTile(enemy.gameObject);
                Destroy(enemy.gameObject);
                return true;
            }
            return false;
        });
    }
  
    // Generates a random Edge Tile on the current Hex Grid based on GridRadius
    // The logic I wrote to make this work is a little ridiculous, but it works
    // Could be better in q, r, s, but I just did it in i, j
    public (int q, int r, int s) RandomTileInRadius(int hexRadius, int spawnRadius)
    {
        if (spawnRadius > hexRadius)
        {
            Debug.LogError("Spawn Radius cannot be greater than Hex Radius");
            return (0, 0, 0);
        }
        // For any arbitrary Grid, there are exactly 6*Radius edge tiles
        // Generates a random int in the range [0,6*Radius)
        int spawnHex = UnityEngine.Random.Range(0, 6*spawnRadius);
        Debug.Log(spawnHex);
        // Random Tile is in the First Row
        if (spawnHex <= spawnRadius)
        {
            return GridManager.IJtoQRS(hexRadius - spawnRadius, spawnHex - spawnRadius + hexRadius);
        }
        // Random Tile is in the Last Row
        if (spawnHex >= 5*spawnRadius - 1)
        {
            return GridManager.IJtoQRS(2*hexRadius - (hexRadius - spawnRadius), spawnHex - 5*spawnRadius + 1 - spawnRadius + hexRadius);
        }
        // Random Tile is on Left/Right Edge
        int adjustedSpawnHex = spawnHex - spawnRadius - 1;
        int i, j;

        if (adjustedSpawnHex % 2 == 0)
        {
            // Tile is on Left Side
            i = 1 + (adjustedSpawnHex / 2) + hexRadius - spawnRadius; 
            j = hexRadius - spawnRadius;
        }
        else // (adjustedSpawnHex % 2 == 1)
        {
            // Tile is on Right Side
            i = 1 + ((adjustedSpawnHex - 1) / 2) + hexRadius - spawnRadius;
            j = (i > hexRadius) ? (2*hexRadius - (i - spawnRadius)) : (spawnRadius + i);
        }
        return GridManager.IJtoQRS(i, j);
    }
}


