using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using GridSystem;
using EnemyAndTowers;

//import GridManager.cs;
namespace EnemyAndTowers
{
      public class CaveGenerator : MonoBehaviour
  {
      [SerializeField] public GridManager GridManager;

      [SerializeField] public GameObject CavePrefab;
      [SerializeField] public int numCaves;
      [SerializeField] public int Radius;
      [SerializeField] public bool DEBUG;
      public List<Vector3> CavePositions = new List<Vector3>();

      void Awake()
      {
          GenerateCaves();
      }

      public void GenerateCaves()
      {
          // Check if the number of caves is valid
          if (numCaves > 6)
          {
              Debug.LogError("Number of caves cannot exceed 6");
              return;
          }
          HashSet<int> usedEdges = new HashSet<int>();  // Track used edges
          for (int i = 0; i < this.numCaves; i++)
          {
              (int q, int r, int s) = RandomTileInRadius(Radius, 5);
              int edge = DetermineEdge(q, r, s);  // Identify which edge this tile is on

              // If the edge has already been used, find a new tile
              while (usedEdges.Contains(edge) || isSpoke(q, r, s))
              {
                  (q, r, s) = RandomTileInRadius(Radius, 5);
                  edge = DetermineEdge(q, r, s);
              }

              usedEdges.Add(edge);  // Mark this edge as used
              CavePositions.Add(new Vector3(q, r, s));
          }
          
          if (DEBUG)
          {
              foreach (var Position in CavePositions)
              {
                  Debug.Log("Cave position: " + Position);
              }
          }
      }

      // Helper method to determine edge
      private int DetermineEdge(int q, int r, int s)
      {
          (int i, int j) = GridManager.QRStoIJ(q, r, s);
          if (i == 0) return 1;                // Top
          if (i == 2 * Radius) return 2;       // Bottom
          if (j == 0 && i <= Radius) return 3; // Top left
          if (j == 0) return 4;                // Bottom left
          if (j == 1 && i <= Radius) return 5; // Top right
          if (j == 1) return 6;                // Bottom right
          return 0; // Should never reach here
      }

      public bool isSpoke(int q, int r, int s)
      {
          return (Math.Abs(q) == Math.Abs(r) && s == 0) || (Math.Abs(q) == Math.Abs(s) && r == 0) || 
                (Math.Abs(r) == Math.Abs(s) && q == 0);
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
  }
}

