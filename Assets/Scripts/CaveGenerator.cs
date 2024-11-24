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

      HashSet<int> usedEdges;

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
          usedEdges = new HashSet<int>();  // Track used edges

          TileSelector CaveTileSelector = new TileSelector();
          List<(int, int, int)> CaveLocations = CaveTileSelector.SelectNRandomTiles(numCaves, Radius, TileSelectorCallback);

          int q, r, s;
          for (int i = 0; i < CaveLocations.Count; i++)
          {
            (q, r, s) = CaveLocations[i];
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

      public bool TileSelectorCallback((int, int, int) QRSTuple)
      {
        (int q, int r, int s) = QRSTuple;
        int edge = DetermineEdge(q, r, s);
        if (usedEdges.Contains(edge) || isSpoke(q, r, s))
            return false;
        usedEdges.Add(edge);
        return true;
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
  }
}