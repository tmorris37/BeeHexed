using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EnemyAndTowers;

namespace GridSystem
{
    public class HexTile : MonoBehaviour
    {
        public bool DPSEffect;

        [SerializeField] public GameObject Occupant;

        public HexTile()
        {
            this.Occupant = null;
            this.DPSEffect = false;
        }

        // Removes the Occupant from this Tile (if possible)
        // If Occupant is successfully removed, return true
        public bool LeaveTile()
        {
            if (this.Occupant == null)
            {
                Debug.Log("Tile is empty. No one can leave!");
                return false;
            }

            this.Occupant = null;
            return true;
        }

        // Adds the New Occupant to this Tile (if possible)
        // If Occupant is successfully added, return true
        public bool EnterTile(GameObject newOccupant)
        {
            if (this.Occupant != null)
            {
                Debug.Log("Tile is full. No room for another occupant!");
                return false;
            }

            this.Occupant = newOccupant;
            return true;
        }

        public bool getOccupied()
        {
            return this.Occupant != null;
        }

        public void UpdateDPSEffect()
        {
            this.DPSEffect = !this.DPSEffect;
        }


    }
}