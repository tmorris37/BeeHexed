using System.Collections;
using System.Collections.Generic;

namespace GridSystem
{
    public class HexTile
    {
        public bool Occupied;
        public bool DPSEffect;
        public int ID;

        public HexTile()
        {
            this.Occupied = false;
            this.DPSEffect = false;
            this.ID = 0;
        }

        public void UpdateOccupy(int ID = 0)
        {
            if (!this.Occupied && ID == 0)
            {
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