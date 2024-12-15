using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridSystem;
using EnemyAndTowers;
using Node;

namespace Node {
    public class NodeGenerator : MonoBehaviour {
        [SerializeField] public GridManager gridManager;

        [SerializeField] public GameObject level0;
        [SerializeField] public GameObject level1;
        [SerializeField] public GameObject level2;
        [SerializeField] public GameObject level3;
        [SerializeField] public GameObject level4;
        [SerializeField] public GameObject rewards;

        [SerializeField] private Transform parentTransform; 
        [SerializeField] public int numNodes;
        [SerializeField] public int radius;
        [SerializeField] public bool DEBUG;
        public List<Vector3> nodePositions = new List<Vector3>();

        HashSet<int> usedEdges;

        void Awake() {
            GenerateNodes();
        }

        public void GenerateNodes() {
            usedEdges = new HashSet<int>();  // Track used edges

            TileSelector nodeTileSelector = new TileSelector();
            List<(int, int, int)> nodeLocations = nodeTileSelector.SelectNRandomTiles(numNodes, radius, TileSelectorCallback);

            int q, r, s;
            for (int i = 0; i < nodeLocations.Count; i++) {
                (q, r, s) = nodeLocations[i];
                nodePositions.Add(new Vector3(q, r, s));
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