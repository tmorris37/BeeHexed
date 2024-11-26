using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using EnemyAndTowers;

namespace GridSystem
{
    public class HexTile : MonoBehaviour
    {
        public bool DPSEffect;

        [SerializeField] public List<GameObject> Occupants;

        public HexTile()
        {
            this.Occupants = new List<GameObject>();
            this.DPSEffect = false;
        }

        // Removes the Occupant from this Tile (if possible)
        // If Occupant is successfully removed, return true
        public bool LeaveTile(GameObject obj)
        {
            if (this.Occupants.Count == 0)
            {
                Debug.Log("Tile is empty. No one can leave!");
                return false;
            }

            this.Occupants.Remove(obj);
            return true;
        }

        // Adds the New Occupant to this Tile (if possible)
        // If Occupant is successfully added, return true
        public bool EnterTile(GameObject newOccupant)
        {
            if (this.Occupants.Count == 0)
            {
                this.Occupants.Add(newOccupant);
                return true;
            }
            bool isOccedByEnemy = true;
            bool newIsEnemy = false;

            if (newOccupant.GetComponent<Enemy>() != null)
            {
                newIsEnemy = true;
            }
            foreach (GameObject occupant in Occupants)
            {
                if (occupant.GetComponent<Enemy>() == null)
                {
                    isOccedByEnemy = false;
                }
            }

            if (newIsEnemy && isOccedByEnemy)
            {
                Occupants.Add(newOccupant);
                return true;
            }
            return false;
        }

        public bool getOccupied()
        {
            if (this.Occupants.Count > 0)
            {
                return true;
            }
            return false;
            
        }

        public bool getOccupiedByTower()
        {
            foreach (GameObject occupant in Occupants)
            {
                if (occupant.GetComponent<Tower>() != null)
                {
                    return true;
                }
            }
            return false;
        }

        public bool getOccupiedByObstacle()
        {
            foreach (GameObject occupant in Occupants)
            {
                if (occupant.GetComponent<Obstacle>() != null)
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateDPSEffect()
        {
            this.DPSEffect = !this.DPSEffect;
        }

        /*public List<Transform> GetEnemiesInHex()
        {
            List<Transform> enemies = new List<Transform>();

            // Check if tile is occupied and if the occupant is an enemy
            if (Occupant != null && Occupant.CompareTag("Enemy"))
            {
                enemies.Add(Occupant.transform);
            }

            return enemies;
        }*/



    }
}