using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace GridSystem
{
    public class GridManager : MonoBehaviour
    {
        /*
        * To handle the hex grid, we're using a Cubic coordinate system
        * https://www.redblobgames.com/grids/hexagons/ (Really cool read!)
        */

        // Number of Tiles = 3(radius)^2 + 3(radius) + 1
        
        // Represents the max distance out from the origin
        // q_max = r_max = s_max = GridRadius
        [SerializeField] public int GridRadius;

        [SerializeField] public GameObject HexTilePrefab;

        [SerializeField] public bool DEBUG = false;

        public GameObject[][] Grid;

        // Instantiates the Hex Grid based on GridRadius
        // Uses an array of arrays to store the grid optimally
        public void Awake() {
            if (DEBUG) Debug.Log("Creating Grid");
            Grid = new GameObject[2*GridRadius + 1][];

            // Creates an Object for organization in Unity
            GameObject GridObject = new GameObject();
            GridObject.name = "Grid";
            GridObject.transform.SetParent(this.transform);

            // Creates Top "Half" of the Grid
            for (int i = 0; i < GridRadius; i++) {
                Grid[i] = new GameObject[GridRadius + 1 + i];
                for (int j = 0; j < GridRadius + 1 + i; j++) {
                    Grid[i][j] = Instantiate(HexTilePrefab);
                    Grid[i][j].name = "Grid[" + i + "][" + j + "]";
                    Grid[i][j].transform.SetParent(GridObject.transform);
                }
            }

            // Creates Middle Row of the Grid
            Grid[GridRadius] = new GameObject[2*GridRadius + 1];
            for (int j = 0; j < 2*GridRadius + 1; j++) {
                Grid[GridRadius][j] = Instantiate(HexTilePrefab);
                Grid[GridRadius][j].name = "Grid[" + GridRadius + "][" + j + "]";
                Grid[GridRadius][j].transform.SetParent(GridObject.transform);
            }

            // Creates Bottom "Half" of the Grid
            for (int i = GridRadius - 1; i >= 0; i--) {
                Grid[2*GridRadius - i] = new GameObject[GridRadius + 1 + i];
                for (int j = 0; j < GridRadius + 1 + i; j++) {
                    Grid[2*GridRadius - i][j] = Instantiate(HexTilePrefab);
                    Grid[2*GridRadius - i][j].name = "Grid[" + (2*GridRadius - i) + "][" + j + "]";
                    Grid[2*GridRadius - i][j].transform.SetParent(GridObject.transform);
                }
            }
        }

        // Fetches the HexTile associated with the q, r, s coordinates
        public HexTile FetchTile(int q, int r, int s) {
            (int i, int j) = QRStoIJ(q, r, s);
            
            //Debug.Log(Grid[i][j]);
            if (i < Grid.Length && i >= 0 && j >= 0 && j < Grid[i].Length) {
                return Grid[i][j].GetComponent<HexTile>();
            }
            return null;
        }

        // Converts the q, r, s coordinates to indices for the HexTile array Grid[][]
        public (int, int) QRStoIJ(int q, int r, int s) {
            int i = r + GridRadius;
            int j = (i > GridRadius) ? (q + GridRadius) : (q + i);

            return (i, j);
        }

        // Converts the indices for the HexTile array Grid[][] to q, r, s coordinates
        public (int, int, int) IJtoQRS(int i, int j) {
            int r = i - GridRadius;
            int q = (i > GridRadius) ? (j - GridRadius) : (j - i);
            int s = 0 - q - r;

            return (q, r, s);
        }

        // Converts the q, r, s coordinates to x, y Unity cooridinates
        // Origin = Center of Hex Grid (0, 0, 0) [BOTH systems]
        // Useful for spawning/moving Unity objects
        public (float, float) QRStoXY(int q, int r, int s) {
            float x = (q - s) * 0.5f * 1.05f;
            float y = -r * 0.866f * 1.0385f;

            return (x, y);
        }

        // Converts the x, y Unity coordinates to q, r, s cooridinates
        public (int, int, int) XYtoQRS(float x, float y) {
            int r = (int) Math.Round(-y / (0.866f * 1.0385f));
            int q = (int) Math.Round((2*(x / 1.05f) - r)/2);
            int s = 0 - q - r;

            return (q, r, s);
        }

        // Converts the q, r, s coordinates to TileMap coordinates
        public (int, int) QRStoTileMapXY(int q, int r, int s) {
            int y = -r;
            int x = (r % 2 == 0) ? ( (q - s)/2 ) : ( (q - s - 1)/2 );

            return (x, y);
        }

        // Converts the TileMap coordinates to q, r, s coordinates
        public (int, int, int) TileMapXYtoQRS(int x, int y) {
            int r = -y;
            int q = (y % 2 == 0) ? ( (2*x + y)/2 ) : ( (2*x + y + 1)/2 );
            int s = 0 - q - r;

            return (q, r, s);
        }

        //returns neighboring Hextiles
        public List<HexTile> GetAdjacentHexes(int q, int r, int s) {
            List<HexTile> adjacentHexes = new List<HexTile>();

            // Define the six possible directions for adjacent hexes
            (int, int, int)[] directions = new (int, int, int)[]
            {
                (q + 1, r - 1, s),
                (q - 1, r + 1, s),
                (q + 1, r, s - 1),
                (q - 1, r, s + 1),
                (q, r + 1, s - 1),
                (q, r - 1, s + 1)
            };

            foreach (var (adjQ, adjR, adjS) in directions) {
                // Check if the adjacent tile is within grid bounds
                try {
                    HexTile adjacentHex = FetchTile(adjQ, adjR, adjS);
                    if (adjacentHex != null) {
                        adjacentHexes.Add(adjacentHex);
                    }
                } catch {
                    // Ignore any out-of-bounds tiles
                }
            }
            return adjacentHexes;
        }
    }
}