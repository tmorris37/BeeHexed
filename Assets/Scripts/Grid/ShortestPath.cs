using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace GridSystem
{
    public class ShortestPath
    {
        /* Node Index Representation
         *     _   _   
         *   _/0\_/1\_
         *  /2\_/3\_/4\
         *  \_/5\_/6\_/
         *    \_/ \_/
         */
        public int gridRadius;

        public GridManager gridManager;

        public bool DEBUG = false;

        // Implements the Dijksta Algorithm to find the shortest path between 2 nodes.
        // Because the graph is unweighted, the algoritm becomes simplified
        // Returns a list of the (q, r, s) coordinates that make the shortest path
        // If there is no path, returns an empty list
        public List<(int, int, int)> DijkstraSimple(GridManager gridManager, (int, int, int) start, (int, int, int) end, Predicate<(int, int, int)> ConditionCallback)
        {
            this.gridManager = gridManager;
            this.gridRadius = gridManager.GridRadius;

            int numNodes = 3*gridRadius*gridRadius + 3*gridRadius + 1;

            // Checks to make sure path doesn't start/end on an Obstacle
            (int q, int r, int s) = end;
            if (ConditionCallback((q, r, s)))
            {
                Debug.LogError("Path ends on Obstacle");
                return new List<(int, int, int)>();
            }

            (q, r, s) = start;
            if (ConditionCallback((q, r, s)))
            {
                Debug.LogError("Path starts on Obstacle");
                return new List<(int, int, int)>();
            }

            // Sets the cost to travel to each Node to -1 (treat like infinity)
            int[] costToTravel = new int[numNodes];
            for (int i = 0; i < numNodes; i++)
            {
                costToTravel[i] = -1;
            }
            // Changes the cost of start to 0 [(q, r, s) still holds start]
            costToTravel[QRStoNodeIndex(q, r, s)] = 0;

            // Creates the toVisit list for Dijkstra's Algorithm
            List<(int, int, int)> toVisit = new List<(int, int, int)>();

            toVisit.Add((q, r, s));
            while (toVisit.Count != 0)
            {
                // Pops off the current QRS
                (q, r, s) = toVisit[0];
                toVisit.RemoveAt(0);

                // We found the node early! Break now to save time
                if ((q, r, s) == end)
                {
                    break;
                }

                int currentIndex = QRStoNodeIndex(q, r, s);

                // Finds (valid) neighbors and updates their cost
                List<(int, int, int)> neighbors = NeighboringQRS(q, r, s, ConditionCallback);
                foreach ((int, int, int) neighbor in neighbors)
                {
                    (int qNew, int rNew, int sNew) = neighbor;
                    int neighborIndex = QRStoNodeIndex(qNew, rNew, sNew);
                    // Debug.Log(neighborIndex + ": " + neighbor);


                    if (costToTravel[neighborIndex] == -1 || costToTravel[neighborIndex] > costToTravel[currentIndex] + 1)
                    {
                        if (!toVisit.Contains(neighbor))
                            toVisit.Add(neighbor);
                        costToTravel[neighborIndex] = costToTravel[currentIndex] + 1;
                    }
                }
            }
            (q, r, s) = end;
            int endIndex = QRStoNodeIndex(q, r, s);

            if (DEBUG) {
                string debugString = "";
                for (int i = 0; i < numNodes; i++)
                {
                    if (i == 6 || i == 13 || i == 21 || i == 30 || i == 40 || i == 51 || i == 61 || i == 70 || i == 78 || i == 85 || i == 91)
                        debugString += '\n';
                    debugString += costToTravel[i] + " ";
                }
                Debug.Log(debugString);
            }

            // The end can be reached from start
            if (costToTravel[endIndex] != -1)
            {
                List<(int, int, int)> shortestPath = new List<(int, int, int)>();
                shortestPath.Add(end);

                // Used to track momentum of an enemy to prioritize straight lines when there's a choice
                (int, int, int) pos1 = (0,0,0),
                                pos2 = (0,0,0),
                                momentum = (0,0,0);

                // Iterates backwards from end to start to get the shortest path
                while ((q, r, s) != start)
                {
                    int count = shortestPath.Count;
                    if (count > 1)
                    {
                        pos1 = shortestPath[count - 1];
                        pos2 = shortestPath[count - 2];

                        momentum = (pos1.Item1 - pos2.Item1,
                                    pos1.Item2 - pos2.Item2,
                                    pos1.Item3 - pos2.Item3);
                    }

                    pos1 = (q, r ,s);
                    List<(int, int, int)> neighbors = NeighboringQRS(q, r, s, ConditionCallback);
                    int minCost = Int32.MaxValue;
                    (int, int, int) bestNeighbor = (q, r, s);

                    foreach ((int, int, int) neighbor in neighbors)
                    {
                        (int qNew, int rNew, int sNew) = neighbor;
                        int neighborIndex = QRStoNodeIndex(qNew, rNew, sNew);
                        int currentCost = costToTravel[neighborIndex];

                        // Checks for momentum
                        if (currentCost == minCost && currentCost > -1)
                        {
                            if ((pos1.Item1 + momentum.Item1,
                                 pos1.Item2 + momentum.Item2,
                                 pos1.Item3 + momentum.Item3) == neighbor) {
                                bestNeighbor = neighbor;
                            }
                        }
                        // Checks for better paths
                        else if (currentCost < minCost && currentCost > -1)
                        {
                            minCost = currentCost;
                            bestNeighbor = neighbor;
                        }
                    }
                    if ((q,r,s) == bestNeighbor)
                        Debug.LogError("Should be impossible");
                    (q, r, s) = bestNeighbor;

                    if (DEBUG)
                        Debug.Log(bestNeighbor);

                    shortestPath.Add((q, r, s));
                }

                // Stored the path backwards, so we have to reverse before return
                shortestPath.Reverse();

                if (DEBUG) {
                    string debugStr = "";
                    foreach (var item in shortestPath)
                    {
                        debugStr += item + ": " + ConditionCallback((item.Item1, item.Item2, item.Item3));
                    }
                    Debug.Log(debugStr);
                }

                return shortestPath;
            }
            // end could not be reached from start -> return empty list
            return new List<(int, int, int)>();
        }

        // Converts the QRS coordinates of a Tile into its Node Index
        public int QRStoNodeIndex(int q, int r, int s)
        {
            // Uses the IJ coordinates described in GridManager
            (int i, int j) = this.gridManager.QRStoIJ(q, r, s);

            // Debug.Log(i + ", " + j + " : " + q + ", " + r + ", " + s);

            int rowNum = 0;
            int baseValue = 0;

            // Iterates through each row and adds the number of nodes encountered
            if (this.gridRadius >= i)
            {
                // Only iterates through the top half of the Grid
                while (rowNum < i)
                {
                    baseValue += (gridRadius + rowNum + 1);
                    rowNum++;
                }
            }
            else
            {
                // Iterates through both halves of the Grid
                while (rowNum < this.gridRadius)
                {
                    baseValue += (gridRadius + rowNum + 1);
                    rowNum++;
                }
                while (rowNum < i)
                {
                    baseValue += (3*gridRadius + 1 - rowNum);
                    rowNum++;
                }
            }
            // Returns the nodes in previous rows + the offset in this row
            return (baseValue + j);
        }

        // Returns the q, r, s coordinates of the (valid) neighbors of the input
        public List<(int, int, int)> NeighboringQRS(int q, int r, int s, Predicate<(int, int, int)> ConditionCallback)
        {
            List<(int, int, int)> validNeighbors = new List<(int, int, int)>();

            (int, int, int)[] directions = new (int, int, int)[]
            {
                (q + 1, r - 1, s),
                (q - 1, r + 1, s),
                (q + 1, r, s - 1),
                (q - 1, r, s + 1),
                (q, r + 1, s - 1),
                (q, r - 1, s + 1)
            };
            // Iterates through each potential neighbor
            foreach ((int, int, int) QRS in directions)
            {
                (int currQ, int currR, int currS) = QRS;
                // If the Tile is occupied by an obstacle or out-of-bounds, reject it
                if (Math.Abs(currQ) <= this.gridRadius &&
                    Math.Abs(currR) <= this.gridRadius &&
                    Math.Abs(currS) <= this.gridRadius &&
                    !ConditionCallback((currQ, currR, currS))
                   )
                {
                    validNeighbors.Add(QRS);
                }
            }
            
            if (DEBUG) {
                string debug = "(" + q +", " + r + ", " + s + "): ";
                foreach (var neighbor in validNeighbors)
                {
                    debug += neighbor;
                }
                Debug.Log(debug);
            }

            return validNeighbors;
        }
    }

}
