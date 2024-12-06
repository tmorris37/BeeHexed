using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridSystem;
using EnemyAndTowers;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] public GridManager gridManager;

    // Corresponds to 2 Prefabs we might want to spawn
    [SerializeField] public GameObject tile;
    [SerializeField] public GameObject pathTile;

    private List<GameObject> tiles;

    public List<(int, int, int)> obstacleTracker;
    public List<(int, int, int)> pathTiles;
    public CaveGenerator caveGenerator;
    public MovementAlgorithms movement;

    void Awake()
    {
        caveGenerator = GameObject.Find("CaveGenerator").GetComponent<CaveGenerator>();
        movement = GameObject.Find("MovementAlgorithms").GetComponent<MovementAlgorithms>();
    }
    
    void Start()
    {
        AddTilePrefabs();
        GetPaths();
        SpawnTiles();
    }

    public void GetPaths() {
        pathTiles = new List<(int, int, int)>();
        foreach (Vector3 cave in caveGenerator.cavePositions)
        {
            (int q, int r, int s) = ((int)cave.x, (int)cave.y, (int)cave.z);
            // I'm honestly not entirely sure how this works but it does
            // Trying to just call DijkstraSimple() was not working
            ShortestPath PathVerifier = new ShortestPath();
            Enemy tempEnemy = new();
            tempEnemy.SetQRS(q, r, s);
            List<(int, int, int)> path = movement.DijkstraInitialize(tempEnemy);
            if (path.Count > 0)
            {
                pathTiles.AddRange(path);
            }
            // Add the cave to the path
            pathTiles.Add((q, r, s));
        }
        
    }

    // Creates a List from the Tile Prefabs attached to script
    public void AddTilePrefabs()
    {
        List<GameObject> Tiles = new List<GameObject>();

        Tiles.Add(tile);

        Debug.Log("Tiles: " + Tiles.Count);
        tiles = Tiles;
    }

    // Checks if the Tile is in obstacleTracker
    public bool DijkstraCallback((int, int, int) QRSTuple)
    {
        return false;
    }

    // Spawns all the tiles in the grid
    public void SpawnTiles()
    {
        int q,r,s;
        int radius = gridManager.GridRadius;
        for (int i = 0; i < radius + 1; i++)
        {
            for (int j = 0; j < radius + i + 1; j++)
            {
                q = gridManager.IJtoQRS(i,j).Item1;
                r = gridManager.IJtoQRS(i,j).Item2;
                s = gridManager.IJtoQRS(i,j).Item3;
                if (pathTiles.Contains((q, r, s)))
                {
                    SpawnTile(q, r, s, "Path");
                } else {
                    SpawnTile(q, r, s);
                }
            }
        }
        for (int i = radius + 1; i < radius * 2 + 1; i++)
        {
            for (int j = 0; j < 2 * radius - (i - 1) % radius; j++)
            {
                q = gridManager.IJtoQRS(i,j).Item1;
                r = gridManager.IJtoQRS(i,j).Item2;
                s = gridManager.IJtoQRS(i,j).Item3;
                if (pathTiles.Contains((q, r, s)))
                {
                    SpawnTile(q, r, s, "Path");
                } else {
                    SpawnTile(q, r, s);
                }
            }
        }
    }

    // Spawns an Obstacle at (q, r, s). Checks to ensure spawnable, deletes otherwise
    public void SpawnTile(int q, int r, int s, string type = "Normal")
    {
        GameObject CurrentTile;
        if (type == "Path") {
            CurrentTile = Instantiate(pathTile);
        } else {
            CurrentTile = Instantiate(RandomTileFromList());
        }

        CurrentTile.transform.SetParent(gameObject.transform);

        CurrentTile.GetComponent<BaseTile>().gridManager = gridManager;
        CurrentTile.GetComponent<BaseTile>().GridRadius = gridManager.GridRadius;
        CurrentTile.GetComponent<BaseTile>().SetQRS(q, r, s);

        (float x, float y) = gridManager.QRStoXY(q, r, s);
        CurrentTile.transform.position = new Vector3(x, y, 0);
        // if (CurrentTile.GetComponent<BaseTile>().SetPosition())
        // {
        //     (float x, float y) = gridManager.QRStoXY(q, r, s);
        //     CurrentTile.transform.position = new Vector3(x, y, 0);
        // }
        // else
        // {
        //     Destroy(CurrentTile);
        // }
    }

    public GameObject RandomTileFromList()
    {
        int index = Random.Range(0, tiles.Count);
        return tiles[index];
    }
}
