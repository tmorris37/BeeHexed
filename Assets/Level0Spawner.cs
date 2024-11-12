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

    public Enemy EnemyComponent;

    public Tower TowerCompnent;

    private float timer = 0f;
    public float moveInterval;

    void Start()
    {
        (int q, int r, int s) = RandomTileInRadius(5, 4);

        //EnemyComponents = new List<Enemy>();
        print(s);
        print(q);
        print(r);
        Spawn(0,q,r,s);
        //Spawn(0, 5, -5, 0);
    }

    public void Spawn(int EnemyID, int q, int r, int s)
    {
        // Spawn Unity Object with Enemy script (Prefab)
        GameObject NewEnemy = Instantiate(EnemyPrefab);
    
        this.EnemyComponent = NewEnemy.GetComponent<Enemy>();

        // Set the Enemy ID with desired ID
        this.EnemyComponent.EnemyID = EnemyID;

        // Set the QRS position to spawn it in
        this.EnemyComponent.SetQRS(q, r, s);

        // Sets the XY coordinates in the Unity coordinates
        (float x, float y) = this.GridManager.QRStoXY(q, r, s);
        this.EnemyComponent.transform.Translate(x, y, 0, Space.World);

        this.EnemyComponent.GridManager = this.GridManager;

        this.EnemyComponent.GridRadius = this.GridManager.GridRadius;

    }

    public void MoveNW()
    {
        if (this.EnemyComponent.Move("Northwest"))
            this.EnemyComponent.transform.Translate(-.5f, 1, 0, Space.Self);
    }
    public void MoveNE()
    {
        if (this.EnemyComponent.Move("Northeast"))
            this.EnemyComponent.transform.Translate(.5f, 1, 0, Space.Self);
    }
    public void MoveE()
    {
        if (this.EnemyComponent.Move("East"))
            this.EnemyComponent.transform.Translate(1, 0, 0, Space.Self);
    }
    public void MoveSE()
    {
        if (this.EnemyComponent.Move("Southeast"))
            this.EnemyComponent.transform.Translate(.5f, -1, 0, Space.Self);
    }
    public void MoveSW()
    {
        if (this.EnemyComponent.Move("Southwest"))
            this.EnemyComponent.transform.Translate(-.5f, -1, 0, Space.Self);
    }
    public void MoveW()
    {
        if (this.EnemyComponent.Move("West"))
            this.EnemyComponent.transform.Translate(-1, 0, 0, Space.Self);
    }

    public void Shoot()
    {
        Instantiate(ProjectilePrefab).transform.position = this.EnemyComponent.transform.position;
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= moveInterval)
        {
            if (this.EnemyComponent != null)
            {
              SimpleMove();
              timer = 0f;
            }
            
        }

        if (this.EnemyComponent != null && this.EnemyComponent.health <= 0)
        {
          Destroy(this.EnemyComponent.gameObject);
        
        // Set EnemyComponent to null to clean up the reference
          this.EnemyComponent = null;
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

    
    public void SimpleMove()
    {
      // Get the current position of the enemy
      int q = EnemyComponent.q;
      int r = EnemyComponent.r;
      int s = EnemyComponent.s;

      // If enemy is at the origin, do nothing
      if (q == 0 && r == 0 && s == 0) {
        return;
      }      
      // Move the enemy along the spokes of the hex grid if possible
      if (Math.Abs(q) == Math.Abs(r) && s == 0) {
        if (q > r) {
          MoveSW();
        } else {
          MoveNE();
        }
      } else if (Math.Abs(q) == Math.Abs(s) && r == 0) {
        if (q > s) {
          MoveW();
        } else {
          MoveE();
        }
      } else if (Math.Abs(r) == Math.Abs(s) && q == 0) {
        if (r > s) {
          MoveNW();
        } else {
          MoveSE();
        }
      }
      // If the enemy is not on a spoke, move it to the nearest spoke along the edges of the grid
      else {
        if (q > r && q > s && Math.Abs(q) == GridManager.GridRadius) {
          MoveNW();
        } else if (r > q && r > s && Math.Abs(r) == GridManager.GridRadius) {
          MoveE();
        } else if (s > q && s > r && Math.Abs(s) == GridManager.GridRadius) {
          MoveSW();
        } else if (q < r && q < s && Math.Abs(q) == GridManager.GridRadius) {
          MoveSE();
        } else if (r < q && r < s && Math.Abs(r) == GridManager.GridRadius) {
          MoveW();
        } else if (s < q && s < r && Math.Abs(s) == GridManager.GridRadius) {
          MoveNE();
        }
      }
    }
}


