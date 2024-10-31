using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridSystem;
using EnemyAndTowers;

public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject EnemyPrefab;

    [SerializeField] public GameObject ProjectilePrefab;

    [SerializeField] public GameObject TowerPrefab;

    [SerializeField] public GridManager GridManager;

    public Enemy EnemyComponent;

    public Tower TowerCompnent;

    public void ButtonSpawn()
    {
        Spawn(5, 0, 0, 0);
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

}
