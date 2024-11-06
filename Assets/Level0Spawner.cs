using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using GridSystem;
using EnemyAndTowers;

//import GridManager.cs;

    public class Level0Spawner : MonoBehaviour
{
    [SerializeField] public GameObject EnemyPrefab;

    [SerializeField] public GameObject ProjectilePrefab;

    [SerializeField] public GameObject TowerPrefab;

    [SerializeField] public GridManager GridManager;

    public Enemy EnemyComponent;

    public Tower TowerCompnent;

    private float timer = 0f;
    public float moveInterval;

    void Start()
    {
        System.Random rand = new System.Random();

        int sign = (rand.Next(1, 3) == 1) ? -1 : 1;
        int a = 4 * sign;
        int s = 0, q = 0, r = 0, b = 0, c = 0;
        int set = rand.Next(1,6);
        switch (set) {
            case 1:
                b = -4 * sign;
                c = 0;
                break;
            case 2:
                b = -3 * sign;
                c = -1 * sign;
                break;
            case 3:
                b = -2 * sign;
                c = -2 * sign;
                break;
            case 4:
                b = -1 * sign;
                c = -3 * sign;
                break;
            case 5:
                b = 0 * sign;
                c = -4 * sign;
                break;
        }

        switch (rand.Next(0,3)) {
            case 0:
                s = a;
                q = b;
                r = c;
                break;
            case 1:
                s = b;
                q = c;
                r = a;
                break;
            case 2:
                s = c;
                q = a;
                r = b;
                break;
        }

        //EnemyComponents = new List<Enemy>();
        print(s);
        print(q);
        print(r);
        Spawn(0,3,1,-4);
        //Spawn(0, 5, -5, 0);
    }

    public void Spawn(int EnemyID, int q, int r, int s)
    {
        // Spawn Unity Object with Enemy script (Prefab)
        GameObject NewEnemy = Instantiate(EnemyPrefab);
    
        this.EnemyComponent = NewEnemy.GetComponent<Enemy>();

        // Set the Enemy ID with desired ID
        this.EnemyComponent.EnemyID = EnemyID;

        // Set the QRS position to spawn it in
        this.EnemyComponent.SetQRS(q, r, s);

        // Sets the XY coordinates in the Unity coordinates
        (float x, float y) = this.GridManager.QRStoXY(q, r, s);
        this.EnemyComponent.transform.Translate(x, y, 0, Space.World);

        this.EnemyComponent.GridManager = this.GridManager;

        this.EnemyComponent.GridRadius = this.GridManager.GridRadius;

    }

    public void MoveNW()
    {
        if (this.EnemyComponent.Move("Northwest"))
            this.EnemyComponent.transform.Translate(-.5f, 1, 0, Space.Self);
    }
    public void MoveNE()
    {
        if (this.EnemyComponent.Move("Northeast"))
            this.EnemyComponent.transform.Translate(.5f, 1, 0, Space.Self);
    }
    public void MoveE()
    {
        if (this.EnemyComponent.Move("East"))
            this.EnemyComponent.transform.Translate(1, 0, 0, Space.Self);
    }
    public void MoveSE()
    {
        if (this.EnemyComponent.Move("Southeast"))
            this.EnemyComponent.transform.Translate(.5f, -1, 0, Space.Self);
    }
    public void MoveSW()
    {
        if (this.EnemyComponent.Move("Southwest"))
            this.EnemyComponent.transform.Translate(-.5f, -1, 0, Space.Self);
    }
    public void MoveW()
    {
        if (this.EnemyComponent.Move("West"))
            this.EnemyComponent.transform.Translate(-1, 0, 0, Space.Self);
    }

    public void Shoot()
    {
        Instantiate(ProjectilePrefab).transform.position = this.EnemyComponent.transform.position;
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= moveInterval)
        {
            MoveW();
            timer = 0f;
        }
    }

    private void MoveEnemy()
    {
        this.EnemyComponent.transform.Translate(-1, 0, 0, Space.Self);
        
    }
}


