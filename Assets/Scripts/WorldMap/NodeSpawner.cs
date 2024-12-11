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
    [Header("Levels")]
    [SerializeField] public GameObject level0;
    [SerializeField] public GameObject level1;
    [SerializeField] public GameObject level2;
    [SerializeField] public GameObject level3;
    [SerializeField] public GameObject level4;
    [SerializeField] public GameObject rewards;
    [Header("Radius")]
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
        SpawnLevel2Nodes();
        SpawnLevel3Nodes();
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

        AddRingPositions(9, possiblePositions);

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
        SpawnNodesFromHextant(topLeftPositions, UnityEngine.Random.Range(2, 4), spawnedNodes, 1);
        SpawnNodesFromHextant(bottomRightPositions, UnityEngine.Random.Range(2, 4), spawnedNodes, 1);

        Debug.Log("Spawned Level 1 nodes in both hextants.");
    }

    public void SpawnLevel2Nodes()
    {
        // List to store possible node positions in rings 6 and 7
        List<(int q, int r, int s)> possiblePositions = new List<(int, int, int)>();
        
        AddRingPositions(6, possiblePositions); // Reusing AddRingPositions


        // Filter positions for the two regions
        List<(int q, int r, int s)> region1Positions = FilterPositions(possiblePositions, (q, r, s) =>
            (s > 0 && q < 1 && q > -4) || (q < 0 && s < 4 && s > -1));

        List<(int q, int r, int s)> region2Positions = FilterPositions(possiblePositions, (q, r, s) =>
            (s < 0 && q < 4 && q > -1) || (q > 0 && s < 1 && s > -4));

        // Spawn 2–3 nodes in each region
        List<(int q, int r, int s)> spawnedNodes = new List<(int, int, int)>();
        SpawnNodesFromHextant(region1Positions, UnityEngine.Random.Range(3, 5), spawnedNodes, 2);
        SpawnNodesFromHextant(region2Positions, UnityEngine.Random.Range(3, 5), spawnedNodes, 2);

        Debug.Log("Spawned Level 2 nodes in rings 6 and 7, respecting hextant conditions.");
    }

    public void SpawnLevel3Nodes()
    {
        // List to store possible node positions in rings 3 and 4
        List<(int q, int r, int s)> possiblePositions = new List<(int, int, int)>();
        AddRingPositions(3, possiblePositions); // Generate positions for ring 3


        // Filter positions for each side
        List<(int q, int r, int s)> side1Positions = FilterPositions(possiblePositions, (q, r, s) =>
            ((q < 1 && r > 0) || (s > -1 && r < 0)));

        List<(int q, int r, int s)> side2Positions = FilterPositions(possiblePositions, (q, r, s) =>
            ((q > -1 && r < 0) || (r > 0 && s < 1)));

        // Spawn 1 node on each side
        List<(int q, int r, int s)> spawnedNodes = new List<(int, int, int)>();
        SpawnNodesFromHextant(side1Positions, 2, spawnedNodes, 3);
        SpawnNodesFromHextant(side2Positions, 2, spawnedNodes, 3);

        Debug.Log("Spawned 2 Level 3 nodes: one on each side in rings 3 or 4.");
    }


    // Helper method to filter positions based on a condition
    private List<(int q, int r, int s)> FilterPositions(List<(int q, int r, int s)> positions, 
        Func<int, int, int, bool> condition)
    {
        List<(int q, int r, int s)> filtered = new List<(int, int, int)>();
        foreach (var (q, r, s) in positions)
        {
            if (condition(q, r, s))
            {
                filtered.Add((q, r, s));
            }
        }
        return filtered;
    }


    // Helper method to randomly spawn nodes while ensuring uniqueness
    private void SpawnNodesFromHextant(List<(int q, int r, int s)> hextantPositions, int nodesToSpawn, List<(int q, int r, int s)> spawnedNodes, int nodeType)
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
                GameObject newNode;
                GameObject level;
                if (nodeType == 1) {
                    level = NodeRandomizer(level1, .1);
                    newNode = Instantiate(level, new Vector3(x, y, 0), Quaternion.identity, transform);
                    newNode.tag = "Node1";
                } else if (nodeType == 2) {
                    level = NodeRandomizer(level2, .3);
                    newNode = Instantiate(level2, new Vector3(x, y, 0), Quaternion.identity, transform);
                    newNode.tag = "Node2";
                } else {
                    level = NodeRandomizer(level3, .2);
                    newNode = Instantiate(level, new Vector3(x, y, 0), Quaternion.identity, transform);
                    newNode.tag = "Node3";
                }
                newNode.name = $"Level{nodeType}Node_{candidate.q}_{candidate.r}_{candidate.s}";
                spawnedNodes.Add(candidate);
                nodesToSpawn--;
            }

            hextantPositions.RemoveAt(randomIndex);
            attempts++;
        }
    }

    // Randomizes input node with percent chance of getting reward node
    private GameObject NodeRandomizer(GameObject originalNode, double chance) {
        GameObject result = originalNode;
        System.Random random = new System.Random();

        double randomDouble = random.NextDouble();
        if (randomDouble <= chance)
            result = rewards;
        return result;
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



