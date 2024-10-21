using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class HexTile : MonoBehaviour
    {
        [SerializeField] private bool Occupied;
        [SerializeField] private bool DPSEffect;

        void Start()
        {
            this.Occupied = false;
            this.DPSEffect = false;
        }

        public void UpdateOccupy()
        {
            this.Occupied = !this.Occupied;
        }

        public void UpdateDPSEffect()
        {
            this.DPSEffect = !this.DPSEffect;
        }


    }
}