using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridSystem; 

namespace EnemyAndTowers
{
    public class HexPosition : MonoBehaviour
    {
        /*
        * To handle the hex grid, we're using a Cubic coordinate system
        * https://www.redblobgames.com/grids/hexagons/ (Really cool read!)

        * Note: The origin (0,0,0) is the center of the hexagon.
        * Note: q + r + s = 0 for ALL tiles (can omit s since it's trivial to calculate)

        * Movement:          ( q,  r,  s)
        *     1 - Northwest: ( 0, -1, +1)
        *     2 - Northeast: (+1, -1,  0)
        *     3 - East:      (+1,  0, -1)
        *     4 - Southeast: ( 0, +1, -1)
        *     5 - Southwest: (-1, +1,  0)
        *     6 - West:      (-1,  0, +1)
        */

        // Controls console outputs
        [SerializeField] public bool DEBUG = false;
                // Contains the x, y, z coordinates
        [SerializeField] public int q, r, s;
        // Represents the max distance out from the origin
        [SerializeField] public int GridRadius;
        // The Grid System for the game
        [SerializeField] public GridManager gridManager;
        // Used to initalize an object's position
        public bool SetPosition() {
            if (DEBUG) Debug.Log("Start: Setting Position");
            
            // Checks for out-of-bounds positions, invalid positions, or if the new tile is blocked
            if (isOutOfBounds(this.q, this.r, this.s) ||
                isInvalidPosition(this.q, this.r, this.s) ||
                isBlocked(this.q, this.r, this.s)) {
                if (DEBUG) Debug.Log("Unable to Spawn Enemy/Tower");
                return false;
            }

            // If we can spawn Enemy/Tower, we will
            gridManager.FetchTile(this.q, this.r, this.s).EnterTile(gameObject);
            return true;
        }

        // Sets the QRS values for the Enemy/Tower
        public void SetQRS(int q, int r, int s) {
            this.q = q;
            this.r = r;
            this.s = s;
        }

        /*
        * Provides a cleaner method to update coordinates when only moving 1 tile
        * input should be a direction ("northeast", ...)
        * Returns a negative number if the move was unsuccessful
        * -1 means out of bounds
        * -2 means invalid position
        * -3 means blocked
        * -4 means invalid direction
        * 1 means successful
        */
        public virtual int Move(string direction) {
            string directionLower = direction.ToLower();

            if (directionLower == "northwest") {
                return UpdatePosition(0, -1, +1);
            } else if (directionLower == "northeast") {
                return UpdatePosition(+1, -1, 0);
            } else if (directionLower == "east") {
                return UpdatePosition(+1,  0, -1);
            } else if (directionLower == "southeast") {
                return UpdatePosition( 0, +1, -1);
            } else if (directionLower == "southwest") {
                return UpdatePosition(-1, +1,  0);
            } else if (directionLower == "west") {
                return UpdatePosition(-1,  0, +1);
            } else {
                return -4;
            }
        }

        /*
        * A more direct method of changing position. 
        * Returns negative number if:
        *  - new position is out-of-bounds
        *  - new position is invalid (violates q + r + s = 0)
        *  - new position is blocked by another object
        */
        public virtual int UpdatePosition(int dq, int dr, int ds) {
            int qNew = this.q + dq;
            int rNew = this.r + dr;
            int sNew = this.s + ds;

            // Checks for out-of-bounds positions
            if (isOutOfBounds(qNew, rNew, sNew)) return -1;

            // Checks for invalid positions
            if (isInvalidPosition(qNew, rNew, sNew)) return -2;

            // Checks if the new tile is blocked
            if (isBlocked(qNew, rNew, sNew)) return -3;

            HexTile CurrentTile = this.gridManager.FetchTile(q, r, s);
            HexTile NewTile = this.gridManager.FetchTile(qNew, rNew, sNew);
            //Debug.Log("changing tiles");
            NewTile.EnterTile(gameObject);
            CurrentTile.LeaveTile(gameObject);

            q = qNew;
            r = rNew;
            s = sNew;

            return 1;
        }

        // Returns true if the inputted position is out of bounds
        private bool isOutOfBounds(int q, int r, int s) {
            if ((Math.Abs(q) > GridRadius) ||
                (Math.Abs(r) > GridRadius) ||
                (Math.Abs(s) > GridRadius)) {
                if (DEBUG)
                    Debug.Log("New Position is out of bounds. q = " + q
                            + ", r = " + r + ", s = " + s + ". Radius = "
                            + this.GridRadius);
                return true;
            }
            return false;
        }

        // Returns true if the inputted position is invalid
        private bool isInvalidPosition(int q, int r, int s) {
            if (q + r + s != 0) {
                if (DEBUG)
                    Debug.Log("Invalid Movement. q + r + s = 0 must be true: q = "
                            + q + ", r = " + r + ", s = " + s);
                return true;
            }
            return false;
        }

        // Returns true if the inputted position is blocked by another object
        private bool isBlocked(int q, int r, int s) {
            HexTile candidateTile = this.gridManager.FetchTile(q, r, s);
            if (candidateTile.getOccupiedByTower() || candidateTile.getOccupiedByObstacle()) {
                if (DEBUG) Debug.Log("Tile is occupied by ID:" + candidateTile.Occupants);
                return true;
            }
            return false;
        }

        // Returns 0 if unblocked, 1 if blocked by tower, 2 if blocked by obstacle
        public int isBlockedBy(int q, int r, int s) {
            HexTile candidateTile = this.gridManager.FetchTile(q, r, s);
            if (candidateTile.getOccupiedByTower())  return 1;
            if (candidateTile.getOccupiedByObstacle()) return 2;
            return 0;
        }
    }
}
