using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridSystem;
using EnemyAndTowers;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] public GridManager GridManager;

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
        for (int i = 1; i < this.GridManager.GridRadius; i++)
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
        for (int i = 1; i < this.GridManager.GridRadius; i++)
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
        // Loops for each Ring in Grid
        int MaxRing = (ObstacleQuantities.Count >= GridManager.GridRadius) ?
                      GridManager.GridRadius - 1 : ObstacleQuantities.Count;
        for (int i = 0; i < MaxRing; i++)
        {
            int RingNumber = i + 1;
            // Generates the set of tiles within the Ring
            List<(int, int, int)> AvailableTiles = TilesInRing(RingNumber);

            int MaxQuantity = 6 * RingNumber;

            // Ensures the Quantity of Obstacles is not an impossible amount
            int Quantity = ObstacleQuantities[i];
            Quantity = (Quantity < 0) ? 0 : Quantity;
            Quantity = (Quantity > MaxQuantity) ? MaxQuantity : Quantity;

            int q, r, s;
            for (int j = 0; j < Quantity; j++)
            {
                // Selects a random tile from the currently available ones
                int TileIndex = UnityEngine.Random.Range(0, AvailableTiles.Count);

                // Grabs the QRS of the random tile then removes it from the list
                (q, r, s) = AvailableTiles[TileIndex];
                AvailableTiles.RemoveAt(TileIndex);

                SpawnObstacle(q, r, s);
            }
        }
    }

    // Creates a List of all (q, r, s) values within the Radius without repeats
    public List<(int, int, int)> TilesInRing(int Radius)
    {
        List<(int, int, int)> NewQRS = new List<(int, int, int)>();

        List<(int, int, int)> ABC;

        int a, b, c;

        // Runs twice, for both halves of the Hex Grid
        for (int i = -1; i < 2; i+=2)
        {
            ABC = QRSGenerator(i*Radius);
            for (int j = 0; j < ABC.Count; j++)
            {
                (a, b, c) = ABC[j];

                NewQRS.Add((a, b, c));
                NewQRS.Add((b, c, a));
                NewQRS.Add((c, a, b));
            }
        }
        return NewQRS;
    }

    // Generates a List of QRS coordinates that represent 1 side of the Hex
    public List<(int, int, int)> QRSGenerator(int Radius)
    {
        List<(int, int, int)> ABC = new List<(int, int, int)>();
        int a, b, c;

        a = Radius;
        // Covers (Radius, 0, -Radius) to (Radius, 1-Radius, -1)
        for (b = 0; b < SignOf(a) * a; b++)
        {
            int bAdj = -b * SignOf(a);
            c = -a - bAdj;
            ABC.Add((a, bAdj, c));
        }
        string DebugString = "";
        foreach ( (int, int, int) tuple in ABC)
        {
            DebugString += tuple;
        }
        Debug.Log(DebugString);
        return ABC;
    }

    // If number is negative, return -1. 1 otherwise
    public int SignOf(int number)
    {
        return (number < 0) ? -1 : 1;
    }

    // Spawns an Obstacle at (q, r, s). Checks to ensure spawnable, deletes otherwise
    public void SpawnObstacle(int q, int r, int s)
    {
        GameObject CurrentObstacle = Instantiate(RandomObstacleFromList());
        CurrentObstacle.transform.SetParent(gameObject.transform);

        CurrentObstacle.GetComponent<Obstacle>().GridManager = this.GridManager;
        CurrentObstacle.GetComponent<Obstacle>().GridRadius = this.GridManager.GridRadius;
        CurrentObstacle.GetComponent<Obstacle>().SetQRS(q, r, s);

        if (CurrentObstacle.GetComponent<Obstacle>().SetPosition())
        {
            (float x, float y) = this.GridManager.QRStoXY(q, r, s);
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

