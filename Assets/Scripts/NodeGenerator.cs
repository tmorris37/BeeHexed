using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridSystem;
using EnemyAndTowers;

namespace EnemyAndTowers
{
    public class NodeGenerator : MonoBehaviour
    {
        [SerializeField] public GridManager gridManager;

        [SerializeField] public GameObject nodePrefab;
        [SerializeField] public int numNodes;
        [SerializeField] public int radius;
        [SerializeField] public bool DEBUG;
        public List<Vector3> nodePositions = new List<Vector3>();

        HashSet<int> usedEdges;


    }
} 