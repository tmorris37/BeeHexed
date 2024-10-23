using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] public int GridRadius = 3;

        public HexTile[][] Grid;

        // Instantiates the Hex Grid based on GridRadius
        // Uses an array of arrays to store the grid optimally
        void Awake()
        {
            Grid = new HexTile[2*GridRadius + 1][];

            for (int i = 0; i < GridRadius; i++)
            {
                Grid[i] = new HexTile[GridRadius + 1 + i];
                Grid[2*GridRadius - i] = new HexTile[GridRadius + 1 + i];
                for (int j = 0; j < GridRadius + 1 + i; j++)
                {
                    Grid[i][j] = new HexTile();
                    Grid[2*GridRadius - i][j] = new HexTile();
                }
            }
            Grid[GridRadius] = new HexTile[2*GridRadius + 1];

            for (int j = 0; j < 2*GridRadius + 1; j++)
            {
                Grid[GridRadius][j] = new HexTile();
            }

            // Debug.Log(Grid);
        }

        // Fetches the HexTile associated with the q, r, s coordinates
        public HexTile FetchTile(int q, int r, int s)
        {
            (int i, int j) = QRStoIJ(q, r, s);
            
            //Debug.Log(Grid[i][j]);
            
            return Grid[i][j];
        }

        // Converts the q, r, s coordinates to indices for the HexTile array Grid[][]
        public (int, int) QRStoIJ(int q, int r, int s)
        {
            int i = r + GridRadius;
            int j = (i > GridRadius) ? (q + GridRadius) : (q + i);
            Debug.Log("q: "+ q + " r: " + r + " s: " + s + "\n" + "i: " + i + " j: " + j);

            return (i, j);
        }

        // Converts the q, r, s coordinates to x, y Unity cooridinates
        // Origin = Center of Hex Grid (0, 0, 0) [BOTH systems]
        // Useful for spawning/moving Unity objects
        public (float, float) QRStoXY(int q, int r, int s)
        {
            float x = (q - s)/2;
            float y = -r;
            return (x, y);
        }
    }
}
