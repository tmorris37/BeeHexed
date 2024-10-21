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
        [SerializeField] private int GridRadius = 3;

        private HexTile[][] Grid;

        // Instantiates the Hex Grid based on GridRadius
        // Uses an array of arrays to store the grid optimally
        void Start()
        {
            Grid = new HexTile[2*GridRadius + 1][];

            for (int i = 0; i < GridRadius; i++)
            {
                Grid[i] = new HexTile[GridRadius + 1 + i];
                Grid[2*GridRadius - i] = new HexTile[GridRadius + 1 + i];
            }
            Grid[GridRadius] = new HexTile[2*GridRadius + 1];

        }

        // Fetches the HexTile associated with the q, r, s coordinates
        public HexTile FetchTile(int q, int r, int s)
        {
            (int i, int j) = QRStoIJ(q, r, s);
            return Grid[i][j];
        }

        // Converts the q, r, s coordinates to indices for the HexTile array Grid[][]
        private (int, int) QRStoIJ(int q, int r, int s)
        {
            int i = r + GridRadius;
            int j = (i > GridRadius) ? (q + GridRadius) : (q + i);

            return (i, j);
        }
    }
}
