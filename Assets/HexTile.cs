using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class HexTile : MonoBehaviour
    {
        [SerializeField] public bool Occupied;
        [SerializeField] public bool DPSEffect;
        public int ID;

        void Start()
        {
            this.Occupied = false;
            this.DPSEffect = false;
            this.ID = 0;
        }

        public void UpdateOccupy(int ID = 0)
        {
            if (!this.Occupied && ID == 0)
            {
                Debug.Log("Cannot occupy a tile with ID 0");
                return;
            }
            this.ID = (this.Occupied) ? 0 : ID;

            this.Occupied = !this.Occupied;
        }

        public void UpdateDPSEffect()
        {
            this.DPSEffect = !this.DPSEffect;
        }


    }
}