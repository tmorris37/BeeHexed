using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

using GridSystem;
using EnemyAndTowers;

namespace Node {

    public class Node : HexPosition {
        
        [SerializeField] public string nodeType;         // Type of level associated to node

        [SerializeField] public string encounter;       // Name of encounter


    }


}