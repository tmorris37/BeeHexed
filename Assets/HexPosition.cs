using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Contains the x, y, z coordinates
    [SerializeField] private int q, r, s;

    // Represents the max distance out from the origin
    [SerializeField] private int GridRadius;

    // Controls console outputs
    [SerializeField] private bool DEBUG;

    // Used to initalize an object's position
    public bool SetPosition(int q, int r, int s)
    {
        // Checks for out-of-bounds positions
        if (isOutOfBounds(q, r, s))
            return false;
        
        // Checks for invalid positions
        if (isInvalidPosition(q, r, s))
            return false;

        // Checks if the new tile is blocked
        if (isBlocked(q, r, s))
            return false;

        this.q = q;
        this.r = r;
        this.s = s;

        return true;
    }
    /*
     * Provides a cleaner method to update coordinates when only moving 1 tile
     * input should be a direction ("northeast", ...)
     */
    public bool Move(string Direction)
    {
        string DirectionLower = Direction.ToLower();

        if (DirectionLower == "northwest")
        {
            return UpdatePosition( 0, -1, +1);
        }
        else if (DirectionLower == "northeast")
        {
            return UpdatePosition(+1, -1,  0);
        }
        else if (DirectionLower == "east")
        {
            return UpdatePosition(+1,  0, -1);
        }
        else if (DirectionLower == "southeast")
        {
            return UpdatePosition( 0, +1, -1);
        }
        else if (DirectionLower == "southwest")
        {
            return UpdatePosition(-1, +1,  0);
        }
        else if (DirectionLower == "west")
        {
            return UpdatePosition(-1,  0, +1);
        }
        else
        {
            return false;
        }
    }

    /*
     * A more direct method of changing position. 
     * Returns false if:
     *  - new position is out-of-bounds
     *  - new position is invalid (violates q + r + s = 0)
     *  - new position is blocked by another object
     */
    public bool UpdatePosition(int dq, int dr, int ds)
    {
        int qNew = this.q + dq;
        int rNew = this.r + dr;
        int sNew = this.s + ds;

        // Checks for out-of-bounds positions
        if (isOutOfBounds(qNew, rNew, sNew))
            return false;

        // Checks for invalid positions
        if (isInvalidPosition(qNew, rNew, sNew))
            return false;

        // Checks if the new tile is blocked
        if (isBlocked(qNew, rNew, sNew))
            return false;

        this.q = qNew;
        this.r = rNew;
        this.s = sNew;

        return true;
    }

    // Returns true if the inputted position is out of bounds
    private bool isOutOfBounds(int q, int r, int s)
    {
        if ((Math.Abs(q) > this.GridRadius) ||
            (Math.Abs(r) > this.GridRadius) ||
            (Math.Abs(s) > this.GridRadius))
        {
            if (DEBUG)
                Debug.Log("New Position is out of bounds. q = " + q
                          + ", r = " + r + ", s = " + s + ". Radius = "
                          + this.GridRadius);
            return true;
        }
        return false;
    }

    // Returns true if the inputted position is invalid
    private bool isInvalidPosition(int q, int r, int s)
    {
        if (q + r + s != 0)
        {
            if (DEBUG)
                Debug.Log("Invalid Movement. q + r + s = 0 must be true: q = "
                          + q + ", r = " + r + ", s = " + s);
            return true;
        }
        return false;
    }

    // Returns true if the inputted position is blocked by another object
    private bool isBlocked(int q, int r, int s)
    {
        // TODO: Implement a Block check
        return false;
    }
}
