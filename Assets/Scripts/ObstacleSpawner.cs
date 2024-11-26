using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridSystem;
using EnemyAndTowers;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] public GridManager gridManager;

    // Corresponds to 2 Prefabs we might want to spawn
    [SerializeField] public GameObject Obstacle1;
    [SerializeField] public GameObject Obstacle2;

    private List<GameObject> Obstacles;

    [SerializeField] public bool Percent;
    [SerializeField] public float Percentage;
    [SerializeField] public bool Constant;
    [SerializeField] public int ConstantQuantity;

    [SerializeField] public List<int> Inputs;

    void Start()
    {
        this.Obstacles = AddObstaclePrefabs();
        if (Percent)
        {
            SpawnXPercentObstaclesPerRing(Percentage);
        }
        else if (Constant)
        {
            SpawnNObstaclesPerRing(ConstantQuantity);
        }
        else
        {
            SpawnObstaclesPerRing(Inputs);
        }
    }

    // Creates a List from the Obstacle Prefabs attached to script
    public List<GameObject> AddObstaclePrefabs()
    {
        List<GameObject> Obstacles = new List<GameObject>();

        Obstacles.Add(Obstacle1);
        Obstacles.Add(Obstacle2);

        return Obstacles;
    }

    // Spawns N obstacles in each ring except for the Center (Hive) and outermost ring (Cave-zone)
    // If N < 0, Ring will be empty. If N > # of Spots, Ring will fill up fully
    public void SpawnNObstaclesPerRing(int N)
    {
        List<int> UniformObstacles = new List<int>();
        for (int i = 1; i < this.gridManager.GridRadius; i++)
        {
            UniformObstacles.Add(N);
        }
        SpawnObstaclesPerRing(UniformObstacles);
    }

    // Spawns X % obstacles in each ring except for the Center (Hive) and outermost ring (Cave-zone)
    // If X < 0, no obstacles. If X > 1, all obstacles
    public void SpawnXPercentObstaclesPerRing(float X)
    {
        List<int> PercentageObstacles = new List<int>();
        for (int i = 1; i < this.gridManager.GridRadius; i++)
        {
            PercentageObstacles.Add((int) (X * i * 6));
        }
        SpawnObstaclesPerRing(PercentageObstacles);
    }

    // Takes in a List of ints and spawns that many obstacles in the corresponding ring
    // If any int is negative, no obstacles will spawn.
    // If any int is greater than capacity, ring will fill with obstacles.
    // If List Length is too small, pads with empty rings.
    // If List Length is too big, truncates the extra rings.
    public void SpawnObstaclesPerRing(List<int> ObstacleQuantities)
    {
        // MaxRing Represents the largest ring we can spawn an obstacle in
        int MaxRing = (ObstacleQuantities.Count < gridManager.GridRadius) ?
                       ObstacleQuantities.Count : gridManager.GridRadius - 1;

        TileSelector ObstacleTileSelector = new TileSelector();

        // Loops through each Ring, spawning ObstacleQuantities[i] Obstacles
        for (int i = 0; i < MaxRing; i++)
        {
            int RingNumber = i + 1;

            List<(int, int, int)> RandomTiles = ObstacleTileSelector.SelectNRandomTiles(ObstacleQuantities[i], RingNumber);

            int q, r, s;
            for (int j = 0; j < RandomTiles.Count; j++)
            {
                (q, r, s) = RandomTiles[j];
                SpawnObstacle(q, r, s);
            }
        }
    }

    // Spawns an Obstacle at (q, r, s). Checks to ensure spawnable, deletes otherwise
    public void SpawnObstacle(int q, int r, int s)
    {
        GameObject CurrentObstacle = Instantiate(RandomObstacleFromList());
        CurrentObstacle.transform.SetParent(gameObject.transform);

        CurrentObstacle.GetComponent<Obstacle>().gridManager = this.gridManager;
        CurrentObstacle.GetComponent<Obstacle>().GridRadius = this.gridManager.GridRadius;
        CurrentObstacle.GetComponent<Obstacle>().SetQRS(q, r, s);

        if (CurrentObstacle.GetComponent<Obstacle>().SetPosition())
        {
            (float x, float y) = this.gridManager.QRStoXY(q, r, s);
            CurrentObstacle.transform.Translate(x, y, 0, Space.World);
        }
        else
        {
            Destroy(CurrentObstacle);
        }
    }

    public GameObject RandomObstacleFromList()
    {
        int index = UnityEngine.Random.Range(0, this.Obstacles.Count);
        return this.Obstacles[index];
    }
}
