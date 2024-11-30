using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using GridSystem;
using EnemyAndTowers;

public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject assassinBear;
    [SerializeField] public GameObject bearCub;
    [SerializeField] public GameObject bombeardier;
    [SerializeField] public GameObject slothBear;

    [SerializeField] public GridManager gridManager;
    [SerializeField] public CaveGenerator caveGenerator;


    [SerializeField] public GameObject cheerios;

    [SerializeField] public GameObject cavePrefab;
    [SerializeField] public GameObject centerTower;

    [SerializeField] public int radius;
    [SerializeField] public bool DEBUG;

    [SerializeField] private MovementAlgorithms movement;

    // List to store cave positions
    private List<Vector3> cavePositions = new List<Vector3>();

    // List to store all spawned enemies
    private List<Enemy> enemies = new List<Enemy>();

    void Start()
    {
        this.cavePositions = caveGenerator.cavePositions;
        SpawnCaves();
        SpawnCenterTower();
    }

    public void SpawnCenterTower()
    {
        GameObject centerTower = Instantiate(this.centerTower);
        Tower centerTowerComponent = centerTower.GetComponent<Tower>();
        HexTile centerTile = gridManager.FetchTile(0, 0, 0);
        centerTile.EnterTile(centerTower);
        centerTowerComponent.gridManager = this.gridManager;
    }

    private void SpawnCheerios()
    {
        GameObject che = Instantiate(cheerios);
        HexTile spot = this.gridManager.FetchTile(0, 0, 0);
        spot.EnterTile(che);
    }

    public void SpawnFromCaves(string enemyType)
    {
        GameObject newEnemy;
        Vector3 randomCavePosition = cavePositions[UnityEngine.Random.Range(0, cavePositions.Count)];
        int q = (int)randomCavePosition.x;
        int r = (int)randomCavePosition.y;
        int s = (int)randomCavePosition.z;
        // Spawn Unity Object with Enemy script (Prefab)
        // TODO: Make this not hardcoded
        switch (enemyType)
        {
            case "BearCub" :
                newEnemy = Instantiate(bearCub);
                break;
            case "Bombeardier" :
                newEnemy = Instantiate(bombeardier);
                break;
            case "SlothBear" :
                newEnemy = Instantiate(slothBear);
                break;
            case "AssassinBear" :
                newEnemy = Instantiate(assassinBear);
                break;
            default:
                Debug.LogError("Invalid Enemy ID");
                return;
                
        }
        
        Enemy newEnemyComponent = newEnemy.GetComponent<Enemy>();
    
        if (newEnemyComponent != null)
        {
            newEnemyComponent.enemyType = enemyType;
            newEnemyComponent.SetQRS(q, r, s);
            newEnemyComponent.movement = movement;
            (float x, float y) = this.gridManager.QRStoXY(q, r, s);
            newEnemyComponent.transform.position = new Vector3(x, y, 0);
            newEnemyComponent.gridManager = this.gridManager;
            newEnemyComponent.GridRadius = this.gridManager.GridRadius;
            
            // Add the new enemy to the list of enemies
            enemies.Add(newEnemyComponent);
        }
    }

    public void SpawnCaves()
    {
        foreach (Vector3 cavePosition in cavePositions)
        {
            (float x, float y) = gridManager.QRStoXY((int)cavePosition.x, (int)cavePosition.y, (int)cavePosition.z);
            GameObject newCave = Instantiate(cavePrefab);
            newCave.transform.position = new Vector3(x, y, 0);
        }
        if (DEBUG)
        {
            foreach (var position in cavePositions)
            {
                Debug.Log("Cave position: " + position);
            }
        }
    }

    public void Update()
    {
        // Remove and destroy any enemies with 0 or less health
        enemies.RemoveAll(enemy =>
        {
            if (enemy != null && enemy.health <= 0)
            {
                HexTile tile = gridManager.FetchTile(enemy.q, enemy.r, enemy.s);
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
            return gridManager.IJtoQRS(hexRadius - spawnRadius, spawnHex - spawnRadius + hexRadius);
        }
        // Random Tile is in the Last Row
        if (spawnHex >= 5*spawnRadius - 1)
        {
            return gridManager.IJtoQRS(2*hexRadius - (hexRadius - spawnRadius), spawnHex - 5*spawnRadius + 1 - spawnRadius + hexRadius);
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
        return gridManager.IJtoQRS(i, j);
    }
}


