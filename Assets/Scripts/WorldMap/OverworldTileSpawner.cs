using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridSystem;
using EnemyAndTowers;
using Node;

public class OverworldTileSpawner : MonoBehaviour {
    [SerializeField] public GridManager gridManager;

    // Corresponds to 2 Prefabs we might want to spawn
    [SerializeField] private List<GameObject> pathTiles;

    [SerializeField] private List<GameObject> normalTiles;
    private List<Vector3Int> tilePositions;
    public List<(int, int, int)> obstacleTracker;
    public List<(int, int, int)> pathTilePositions;
    public NodeGenerator nodeGenerator;

    // Assume you have a Dictionary to map QRS to children
    [SerializeField] private Dictionary<Vector3Int, SpriteRenderer> qrsToChildMap;

    void Awake() {
        nodeGenerator = GameObject.Find("NodeGenerator").GetComponent<NodeGenerator>();
    }

        void Start() {
        // Initialize the tilePositions list and the qrsToChildMap dictionary
        tilePositions = new List<Vector3Int>();
        qrsToChildMap = new Dictionary<Vector3Int, SpriteRenderer>();
        // Spawn the tiles
        GetPaths();
        SpawnTiles();
    }

    public void GetPaths() {
        PathingAlgo pathfinder = new PathingAlgo();
        pathTilePositions = pathfinder.GetPathTiles(this.gridManager);
    }


    // Checks if the Tile is in obstacleTracker
    public bool DijkstraCallback((int, int, int) QRSTuple) {
        return false;
    }

       // Spawns all the tiles in the grid
    public void SpawnTiles() {
        int q,r,s;
        int radius = gridManager.GridRadius;
        for (int i = 0; i < radius + 1; i++) {
            for (int j = 0; j < radius + i + 1; j++) {
                q = gridManager.IJtoQRS(i,j).Item1;
                r = gridManager.IJtoQRS(i,j).Item2;
                s = gridManager.IJtoQRS(i,j).Item3;
                if (this.pathTilePositions.Contains((q,r,s))) {
                    SpawnTile(q, r, s, "Path");
                } else {
                    SpawnTile(q, r, s);
                }
            }
        }
        for (int i = radius + 1; i < radius * 2 + 1; i++) {
            for (int j = 0; j < 2 * radius - (i - 1) % radius; j++) {
                q = gridManager.IJtoQRS(i,j).Item1;
                r = gridManager.IJtoQRS(i,j).Item2;
                s = gridManager.IJtoQRS(i,j).Item3;
                if (this.pathTilePositions.Contains((q,r,s))) {
                    SpawnTile(q, r, s, "Path");
                } else {
                    SpawnTile(q, r, s);
                }
            }
        }
    }

   // Spawns an Obstacle at (q, r, s). Checks to ensure spawnable, deletes otherwise
    public void SpawnTile(int q, int r, int s, string type = "Normal") {
        GameObject CurrentTile;
        if (type == "Path") {
            CurrentTile = Instantiate(RandomTileFromList(type));
        } else {
            CurrentTile = Instantiate(RandomTileFromList());
        }
        Vector3Int qrs = new Vector3Int(q, r, s);
        tilePositions.Add(qrs);
        qrsToChildMap[qrs] = CurrentTile.GetComponentInChildren<SpriteRenderer>();

        CurrentTile.transform.SetParent(gameObject.transform);

        CurrentTile.GetComponent<BaseTile>().gridManager = gridManager;
        CurrentTile.GetComponent<BaseTile>().GridRadius = gridManager.GridRadius;
        CurrentTile.GetComponent<BaseTile>().SetQRS(q, r, s);

        (float x, float y) = gridManager.QRStoXY(q, r, s);
        CurrentTile.transform.position = new Vector3(x, y, 0);
    }

    public bool HasTile(Vector3Int position) {
        foreach (Vector3Int tilePosition in tilePositions) {
            if (tilePosition == position) return true;
        }
        return false;
    }

    public void ColorTile(Vector3Int position, Color color) {
        // Convert XY to QRS
        (int q, int r, int s) = (position.x, position.y, position.z);
        Vector3Int qrsPosition = new Vector3Int(q, r, s);

        // Check if the tile exists in the map
        if (qrsToChildMap.TryGetValue(qrsPosition, out SpriteRenderer spriteRenderer)) {
            // Apply the color to the correct child
            spriteRenderer.color = color;
        } else {
            Debug.LogWarning($"Tile at QRS {qrsPosition} not found!");
        }
    }

    public GameObject RandomTileFromList(string type = "Normal")
    {
        if (type == "Path") {
            return pathTiles[Seed.Instance.GetRandomInt(0, pathTiles.Count)];
        } else {
            return normalTiles[Seed.Instance.GetRandomInt(0, normalTiles.Count)];
        }
    }

}
