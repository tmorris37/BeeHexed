using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;


namespace EnemyAndTowers{
    public class NectarBeeTower : Tower
    {

        public float nectarInterval;
        private float timer = 0f;

        private NectarManager nectarManager;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            this.nectarManager = FindObjectOfType<NectarManager>();
        }

        // Update is called once per frame
        protected override void Update()
        {
            if (this.health <= 0)
            {
                HexTile tile = gridManager.FetchTile(q,r,s);
                tile.LeaveTile(gameObject);
                Destroy(gameObject);
            }

            if (timer >= nectarInterval)
            {
                addNectar();
                timer = 0f;
            }
            timer += Time.deltaTime;

        }

        private void addNectar()
        {
            int currNectar = nectarManager.GetNectar();
            nectarManager.SetNectar(currNectar + 1);
        }
    }

}
