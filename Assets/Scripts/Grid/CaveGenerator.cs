using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using GridSystem;
using EnemyAndTowers;

namespace EnemyAndTowers
{
    public class CaveGenerator : MonoBehaviour
    {
        [SerializeField] public GridManager gridManager;

        [SerializeField] public GameObject cavePrefab;
        [SerializeField] public int numCaves;
        [SerializeField] public int radius;
        [SerializeField] public bool DEBUG;
        public List<Vector3> cavePositions = new List<Vector3>();

        HashSet<int> usedEdges;
        [SerializeField] private Seed seedManager;

        void Awake() {
            GenerateCaves();
        }

        public void GenerateCaves() {
            // Check if the number of caves is valid
            if (numCaves > 6) {
                Debug.LogError("Number of caves cannot exceed 6");
                return;
            }
            usedEdges = new HashSet<int>();  // Track used edges

            TileSelector caveTileSelector = new TileSelector();
            List<(int, int, int)> caveLocations = caveTileSelector.SelectNRandomTiles(numCaves, radius, TileSelectorCallback);

            int q, r, s;
            for (int i = 0; i < caveLocations.Count; i++) {
                (q, r, s) = caveLocations[i];
                cavePositions.Add(new Vector3(q, r, s));
            }
            
            if (DEBUG) {
                foreach (var position in cavePositions) {
                    Debug.Log("Cave position: " + position);
                }
            }
        }

        public bool TileSelectorCallback((int, int, int) QRSTuple) {
        (int q, int r, int s) = QRSTuple;
        int edge = DetermineEdge(q, r, s);
        if (usedEdges.Contains(edge) || isSpoke(q, r, s)) return false;
        usedEdges.Add(edge);
        return true;
        }

        // Helper method to determine edge
        private int DetermineEdge(int q, int r, int s) {
            (int i, int j) = gridManager.QRStoIJ(q, r, s);
            if (i == 0) return 1;                // Top
            if (i == 2 * radius) return 2;       // Bottom
            if (j == 0 && i <= radius) return 3; // Top left
            if (j == 0) return 4;                // Bottom left
            if (j == 1 && i <= radius) return 5; // Top right
            if (j == 1) return 6;                // Bottom right
            return 0; // Should never reach here
        }

        public bool isSpoke(int q, int r, int s) {
            return (Math.Abs(q) == Math.Abs(r) && s == 0) || (Math.Abs(q) == Math.Abs(s) && r == 0) || 
                (Math.Abs(r) == Math.Abs(s) && q == 0);
        }
    }
}