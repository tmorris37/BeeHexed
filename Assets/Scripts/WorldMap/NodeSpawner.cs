using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using GridSystem;
using EnemyAndTowers;
using Node;

public class NodeSpawner : MonoBehaviour
{
    [SerializeField] public GridManager gridManager;
    [SerializeField] public NodeGenerator nodeGenerator;

    [SerializeField] public GameObject defaultNode;

    [SerializeField] public GameObject centerNode;

    [SerializeField] public int radius;
    [SerializeField] public bool DEBUG;

    // List to store node positions
    private List<Vector3> nodePositions = new List<Vector3>();

    private List<GameObject> nodes = new List<GameObject>();
    void Start()
    {
        nodePositions = nodeGenerator.nodePositions;
        SpawnNodes();
    }
    

    public void SpawnNodes() {
        foreach (Vector3 nodePosition in nodePositions) {
            (float x, float y) = gridManager.QRStoXY((int)nodePosition.x, (int)nodePosition.y, (int)nodePosition.z);
            GameObject newNode = Instantiate(defaultNode);
            newNode.transform.position = new Vector3(x, y, 0);
            newNode.tag="Node";
        }
        if (DEBUG) {
            foreach (var position in nodePositions) {
                Debug.Log("Node position: " + position);
            }
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



