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

    [SerializeField] public GameObject level0;
    [SerializeField] public GameObject level1;
    [SerializeField] public GameObject level2;
    [SerializeField] public GameObject level3;
    [SerializeField] public GameObject level4;
    [SerializeField] public GameObject rewards;
    [SerializeField] public int radius;
    [SerializeField] public bool DEBUG;

    // List to store node positions
    private List<Vector3> nodePositions = new List<Vector3>();

    private List<GameObject> nodes = new List<GameObject>();
    void Start()
    {
        nodePositions = nodeGenerator.nodePositions;
        SpawnSpecificNodes();
        SpawnLevel1Nodes();
        //SpawnNodes();
    }
    
    public void SpawnSpecificNodes()
    {
        // Leftmost Hex (q = -radius, r = radius, s = 0)
        (float leftX, float leftY) = gridManager.QRStoXY(-radius, 0, radius);
        GameObject leftNode = Instantiate(level0, new Vector3(leftX, leftY, 0), Quaternion.identity, transform);
        leftNode.name = "LeftmostNode";

        // Rightmost Hex (q = radius, r = -radius, s = 0)
        (float rightX, float rightY) = gridManager.QRStoXY(radius, 0, -radius);
        GameObject rightNode = Instantiate(level0, new Vector3(rightX, rightY, 0), Quaternion.identity, transform);
        rightNode.name = "RightmostNode";

        // Center Hex (q = 0, r = 0, s = 0)
        (float centerX, float centerY) = gridManager.QRStoXY(0, 0, 0);
        GameObject centerNode = Instantiate(level4, new Vector3(centerX, centerY, 0), Quaternion.identity, transform);
        centerNode.name = "CenterNode";

        Debug.Log("Leftmost, Rightmost, and Center nodes have been spawned.");
        }

        public void SpawnLevel1Nodes()
        {
        // List to store possible node positions in rings 7 and 8
        List<(int q, int r, int s)> possiblePositions = new List<(int, int, int)>();

        // Generate positions for rings 7 and 8
        AddRingPositions(7, possiblePositions);
        AddRingPositions(8, possiblePositions);

        // Filter positions for each hextant
        List<(int q, int r, int s)> topLeftPositions = new List<(int, int, int)>();
        List<(int q, int r, int s)> bottomRightPositions = new List<(int, int, int)>();

        foreach (var (q, r, s) in possiblePositions)
        {
            if (q < 0 && s > 0 && r < 4 && r > -4) // left most
                topLeftPositions.Add((q, r, s));
            else if (q > 0 && s < 0 && r < 4 && r > -4) // right most
                bottomRightPositions.Add((q, r, s));
        }

        // Spawn 2 or 3 nodes in each hextant
        List<(int q, int r, int s)> spawnedNodes = new List<(int, int, int)>();
        SpawnNodesFromHextant(topLeftPositions, UnityEngine.Random.Range(2, 4), spawnedNodes);
        SpawnNodesFromHextant(bottomRightPositions, UnityEngine.Random.Range(2, 4), spawnedNodes);

        Debug.Log("Spawned Level 1 nodes in both hextants.");
    }

    // Helper method to randomly spawn nodes while ensuring uniqueness
    private void SpawnNodesFromHextant(List<(int q, int r, int s)> hextantPositions, int nodesToSpawn, List<(int q, int r, int s)> spawnedNodes)
    {
        int attempts = 0;

        while (nodesToSpawn > 0 && hextantPositions.Count > 0 && attempts < 1000)
        {
            int randomIndex = UnityEngine.Random.Range(0, hextantPositions.Count);
            var candidate = hextantPositions[randomIndex];

            // Ensure the candidate node is not already spawned
            bool valid = true;
            foreach (var existing in spawnedNodes)
            {
                int distance = HexDistance(candidate, existing);
                if (distance < 3) // Check minimum distance
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                (float x, float y) = gridManager.QRStoXY(candidate.q, candidate.r, candidate.s);
                GameObject newNode = Instantiate(level1, new Vector3(x, y, 0), Quaternion.identity, transform);
                newNode.name = $"Level1Node_{candidate.q}_{candidate.r}_{candidate.s}";
                spawnedNodes.Add(candidate);
                nodesToSpawn--;
            }

            hextantPositions.RemoveAt(randomIndex);
            attempts++;
        }
    }

    // Helper method to calculate hex distance
    private int HexDistance((int q, int r, int s) a, (int q, int r, int s) b)
    {
        return (Mathf.Abs(a.q - b.q) + Mathf.Abs(a.r - b.r) + Mathf.Abs(a.s - b.s)) / 2;
    }

    // Helper method to generate positions for a given ring radius
    private void AddRingPositions(int ringRadius, List<(int, int, int)> positions)
    {
        int q = ringRadius;
        int r = -ringRadius;
        int s = 0;

        (int dq, int dr, int ds)[] directions = new (int, int, int)[]
        {
            (0, 1, -1), (-1, 1, 0), (-1, 0, 1), (0, -1, 1), (1, -1, 0), (1, 0, -1)
        };

        for (int i = 0; i < 6; i++) // 6 sides
        {
            for (int j = 0; j < ringRadius; j++)
            {
                positions.Add((q, r, s));
                q += directions[i].dq;
                r += directions[i].dr;
                s += directions[i].ds;
            }
        }
    }
    public void SpawnNodes() {
        foreach (Vector3 nodePosition in nodePositions) {
            (float x, float y) = gridManager.QRStoXY((int)nodePosition.x, (int)nodePosition.y, (int)nodePosition.z);
            GameObject newNode = Instantiate(level0);
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



