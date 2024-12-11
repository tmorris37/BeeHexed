using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridSystem;

namespace Node {
    public class PathingAlgo {
        public GridManager gridManager;

        public List<(int, int, int)> GetPathTiles (GridManager gridManager) {
            this.gridManager = gridManager;

            ShortestPath pathfinder = new ShortestPath();

            GameObject[] level0Nodes = GameObject.FindGameObjectsWithTag("Node0");
            GameObject[] level1Nodes = GameObject.FindGameObjectsWithTag("Node1");
            GameObject[] level2Nodes = GameObject.FindGameObjectsWithTag("Node2");
            GameObject[] level3Nodes = GameObject.FindGameObjectsWithTag("Node3");
            GameObject[] level4Nodes = GameObject.FindGameObjectsWithTag("Node4");

            List<(int, int, int)> pathTiles = ConnectNodes(level0Nodes, level1Nodes);
            pathTiles.AddRange(ConnectNodes(level1Nodes, level2Nodes));
            pathTiles.AddRange(ConnectNodes(level2Nodes, level3Nodes));
            pathTiles.AddRange(ConnectNodes(level3Nodes, level4Nodes));

            return pathTiles;
        }

        public List<(int, int, int)> ConnectNodes(GameObject[] start, GameObject[] end) {
            ShortestPath pathfinder = new ShortestPath();

            List<(int, int, int)> allPaths = new List<(int, int, int)>();

            foreach (GameObject startNode in start) {
                foreach (GameObject endNode in end) {
                    // Tries to "connect" the nodes
                    if (SignOf(startNode.transform.position.x) == SignOf(endNode.transform.position.x)
                        || Math.Round(2.0*endNode.transform.position.x) == 0) {
                        (int, int, int) startQRS =
                            this.gridManager.XYtoQRS(startNode.transform.position.x, startNode.transform.position.y);
                        (int, int, int) endQRS =
                            this.gridManager.XYtoQRS(endNode.transform.position.x, endNode.transform.position.y);

                        // Only lets nodes on the same up/down path connect (converges in middle)
                        if (startQRS.Item2 == 0 || endQRS.Item2 == 0 ||
                            (SignOf(startNode.transform.position.y) == SignOf(endNode.transform.position.y))) {
                            List<(int, int, int)> path =
                                pathfinder.DijkstraSimple(this.gridManager, startQRS, endQRS, DijkstraCallback);
                            allPaths.AddRange(path);
                        }
                    }
                }
            }
            return allPaths;
        }

        public int SignOf(float number) {
            return (number < 0f ? -1 : 1);
        }

        public bool DijkstraCallback((int, int, int) QRSTuple)
        {
            //(int q, int r, int s) = QRSTuple;

            //return this.gridManager.FetchTile(q, r, s).getOccupiedByObstacle();

            return false;
        }
    }
}
