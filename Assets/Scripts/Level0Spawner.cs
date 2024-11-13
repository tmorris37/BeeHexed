using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using GridSystem;
using EnemyAndTowers;

//import GridManager.cs;

    public class Level0Spawner : MonoBehaviour
{
    [SerializeField] public GameObject EnemyPrefab;

    [SerializeField] public GameObject ProjectilePrefab;

    [SerializeField] public GameObject TowerPrefab;

    [SerializeField] public GridManager GridManager;

    [SerializeField] public GameObject Cheerios;

    [SerializeField] public GameObject CavePrefab;

    [SerializeField] public int Radius;
    [SerializeField] public bool DEBUG;

    // public Enemy EnemyComponent;

    // public Tower TowerCompnent;

    private float timer = 0f;
    public float moveInterval;

    // List to store cave positions
    private List<Vector3> cavePositions = new List<Vector3>();

    // List to store all spawned enemies
    private List<Enemy> enemies = new List<Enemy>();

    void Start()
    {
        // (int q, int r, int s) = RandomTileInRadius(Radius, 5);

        SpawnCaves();
        //SpawnCheerios();
        // Spawn(0,q,r,s);
    }

    private void SpawnCheerios()
    {
      GameObject Che = Instantiate(this.Cheerios);
      HexTile spot = this.GridManager.FetchTile(0, 0, 0);
      spot.EnterTile(Che);
    }

    private IEnumerator SpawnEnemiesFromCaves()
    {
        while (true)
        {
            // Choose a random cave position from the list
            Vector3 randomCavePosition = cavePositions[UnityEngine.Random.Range(0, 3)];
            // Spawn an enemy at the selected position
            //Spawn(0,(int)randomCavePosition.x,(int)randomCavePosition.y,(int)randomCavePosition.z);
            Spawn(0, 5, -2, -3);
            //Spawn(0, 0, 5, -5);
            // Wait for 5 seconds before spawning the next enemy
            yield return new WaitForSeconds(3f);
        }
    }

    public void Spawn(int EnemyID, int q, int r, int s)
    {
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
        HashSet<int> usedEdges = new HashSet<int>();  // Track used edges
        for (int i = 0; i < 3; i++)
        {
            (int q, int r, int s) = RandomTileInRadius(Radius, 5);
            int edge = DetermineEdge(q, r, s);  // Identify which edge this tile is on

            // If the edge has already been used, find a new tile
            while (usedEdges.Contains(edge))
            {
                (q, r, s) = RandomTileInRadius(Radius, 5);
                edge = DetermineEdge(q, r, s);
            }

            usedEdges.Add(edge);  // Mark this edge as used
            GameObject newCave = Instantiate(CavePrefab);
            (float x, float y) = GridManager.QRStoXY(q, r, s);
            newCave.transform.position = new Vector3(x, y, 0);
            cavePositions.Add(new Vector3(q, r, s));

            // Should check if occupied before spawning
            //GridManager.FetchTile(q, r, s).EnterTile(newCave);
        }
        
        if (DEBUG)
        {
            foreach (var position in cavePositions)
            {
                Debug.Log("Cave position: " + position);
            }
        }
        
        StartCoroutine(SpawnEnemiesFromCaves());
    }

    // Helper method to determine edge
    private int DetermineEdge(int q, int r, int s)
    {
        (int i, int j) = GridManager.QRStoIJ(q, r, s);
        // Example logic based on q, r, s values
        if (i == 0) return 1;            // Top
        if (i == 2 * Radius) return 2;   // Bottom
        return 3;                   // Left or Right
    }

    public void moveFailure()
    {
        if (DEBUG)
            Debug.Log("Move failed");
    }

    public void MoveNW(Enemy enemy, int q, int r, int s)
    {
        if (enemy.Move("Northwest"))
        {
            (float a, float b) = GridManager.QRStoXY(q,r-1,s+1);
            enemy.MoveToPosition(new Vector3(a, b, 0));
        }
        else
          moveFailure();
    }
    public void MoveNE(Enemy enemy, int q, int r, int s)
    {
        if (enemy.Move("Northeast"))
        {
            (float a, float b) = GridManager.QRStoXY(q+1,r-1,s);
            enemy.MoveToPosition(new Vector3(a, b, 0));
        }
        else
          moveFailure();
    }
    public void MoveE(Enemy enemy, int q, int r, int s)
    {
        if (enemy.Move("East"))
        {
            (float a, float b) = GridManager.QRStoXY(q+1,r,s-1);
            enemy.MoveToPosition(new Vector3(a, b, 0));
        }
        else
          moveFailure();
    }
    public void MoveSE(Enemy enemy, int q, int r, int s)
    {
        if (enemy.Move("Southeast"))
        {
            (float a, float b) = GridManager.QRStoXY(q,r+1,s-1);
            enemy.MoveToPosition(new Vector3(a, b, 0));
        }
        else
          moveFailure();
    }
    public void MoveSW(Enemy enemy, int q, int r, int s)
    {
        if (enemy.Move("Southwest"))
        {
            (float a, float b) = GridManager.QRStoXY(q-1,r+1,s);
            enemy.MoveToPosition(new Vector3(a, b, 0));
        }
        else
          moveFailure();
    }
    public void MoveW(Enemy enemy, int q, int r, int s)
    {
        if (enemy.Move("West"))
        {
            (float a, float b) = GridManager.QRStoXY(q-1,r,s+1);
            enemy.MoveToPosition(new Vector3(a, b, 0));
        }
        else
          moveFailure();
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
                    SimpleMove(enemy);
                }
            }
            timer = 0f;

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

    
    public void SimpleMove(Enemy enemy)
    {
      // Get the current position of the enemy
      int q = enemy.q;
      int r = enemy.r;
      int s = enemy.s;
      
      // If enemy is at the origin, do nothing
      if (q == 0 && r == 0 && s == 0) {
        return;
      }

      // Move the enemy along the spokes of the hex grid if possible
      if (Math.Abs(q) == Math.Abs(r) && s == 0) {
        if (q > r) {
          MoveSW(enemy,q,r,s);
        } else {
          MoveNE(enemy,q,r,s);
        }
      } else if (Math.Abs(q) == Math.Abs(s) && r == 0) {
        if (q > s) {
          MoveW(enemy,q,r,s);
        } else {
          MoveE(enemy,q,r,s);
        }
      } else if (Math.Abs(r) == Math.Abs(s) && q == 0) {
        if (r > s) {
          MoveNW(enemy,q,r,s);
        } else {
          MoveSE(enemy,q,r,s);
        }
      }
      // If the enemy is not on a spoke, move it to the nearest spoke
      else {
        int absq = Math.Abs(q);
        int absr = Math.Abs(r);
        int abss = Math.Abs(s);

        if (q > r && q > s && absq > absr && absq > abss) {
          MoveNW(enemy,q,r,s);
          if (DEBUG) {
            Debug.Log("Moving NW");
          }
        } else if (r > q && r > s && absr > absq && absr > abss) {
          MoveE(enemy,q,r,s);
        } else if (s > q && s > r && abss > absq && abss > absr) {
          MoveSW(enemy,q,r,s);
        } else if (q < r && q < s && absq > absr && absq > abss) {
          MoveSE(enemy,q,r,s);
        } else if (r < q && r < s && absr > absq && absr > abss) {
          MoveW(enemy,q,r,s);
        } else if (s < q && s < r && abss > absq && abss > absr) {
          MoveNE(enemy,q,r,s);
        }
      }
    }
}


