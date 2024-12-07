using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridSystem;
using EnemyAndTowers;

public class OverworldTileSpawner : MonoBehaviour
{
    [SerializeField] public GridManager gridManager;

    // Corresponds to 2 Prefabs we might want to spawn
    [SerializeField] private List<GameObject> pathTiles;

    [SerializeField] private List<GameObject> normalTiles;
    private List<Vector3Int> tilePositions;
    public List<(int, int, int)> obstacleTracker;
    public List<(int, int, int)> pathTilePositions;
    public NodeGenerator nodeGenerator;
    public MovementAlgorithms movement;

    // Assume you have a Dictionary to map QRS to children
    [SerializeField] private Dictionary<Vector3Int, SpriteRenderer> qrsToChildMap;
}
