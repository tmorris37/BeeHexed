/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridSystem;

public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject TowerPrefab;

    [SerializeField] public GameObject ProjectilePrefab;

    [SerializeField] public GridManager GridManager;

    public Tower TowerComponent;

    /*public void ButtonSpawn()
    {
        Spawn(5, 1, 1, 1);
    }

    void Start()
    {
        Spawn(1, 1, 1, 1);
    }

    public void Spawn(int TowerID, int q, int r, int s)
    {
        // Spawn Unity Object with Enemy script (Prefab)
        GameObject NewTower = Instantiate(TowerPrefab);
        this.TowerComponent = NewTower.GetComponent<Tower>();

        // Set the Enemy ID with desired ID
        this.TowerComponent.TowerID = TowerID;
        // Set the QRS position to spawn it in
        this.TowerComponent.SetPosition(1, 0, -1);

        this.TowerComponent.GridManager = this.GridManager;

        this.TowerComponent.GridRadius = this.GridManager.GridRadius;

    }

    

    /*public void MoveNW()
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
    }*/

    /*public void Shoot()
    {
        Instantiate(ProjectilePrefab).transform.position = this.TowerComponent.transform.position;
    }*/

//}
