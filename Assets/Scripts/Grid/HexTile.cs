using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using EnemyAndTowers;

namespace GridSystem
{
    public class HexTile : HexPosition
    {
        public bool DPSEffect;

        [SerializeField] public List<GameObject> Occupants;

        public HexTile() {
            Occupants = new List<GameObject>();
            DPSEffect = false;
        }

        // Removes the Occupant from this Tile (if possible)
        // If Occupant is successfully removed, return true
        public bool LeaveTile(GameObject obj) {
            if (Occupants.Count == 0) {
                Debug.Log("Tile is empty. No one can leave!");
                return false;
            }

            Occupants.Remove(obj);
            return true;
        }

        // Adds the New Occupant to this Tile (if possible)
        // If Occupant is successfully added, return true
        public bool EnterTile(GameObject newOccupant) {
            if (Occupants.Count == 0) {
                Occupants.Add(newOccupant);
                return true;
            }
            bool isOccedByEnemy = true;
            bool newIsEnemy = false;

            if (newOccupant.GetComponent<Enemy>() != null) newIsEnemy = true;
            foreach (GameObject occupant in Occupants) {
                if (occupant.GetComponent<Enemy>() == null) isOccedByEnemy = false;
            }

            if (newIsEnemy && isOccedByEnemy) {
                Occupants.Add(newOccupant);
                return true;
            }
            return false;
        }

        public bool getOccupied() {
            if (Occupants.Count == 0 || (Occupants.Count == 1 && Occupants[0].GetComponent<BaseTile>() != null)) {
                return false;
            }
            return true;
            
        }

        public bool getOccupiedByTower() {
            foreach (GameObject occupant in Occupants) {
                if (occupant.GetComponent<Tower>() != null) return true;
            }
            return false;
        }

        public bool getOccupiedByObstacle() {
            foreach (GameObject occupant in Occupants) {
                if (occupant.GetComponent<Obstacle>() != null) return true;
            }
            return false;
        }

        public void UpdateDPSEffect() {
            DPSEffect = !DPSEffect;
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