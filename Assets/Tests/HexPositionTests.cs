using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

using EnemyAndTowers;
using GridSystem;

public class HexPositionTests
{
    private string hexTilePrefabPath = "Assets/Prefabs/HexTilePrefab.prefab";
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator HexPositionTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.

        // Sets up the GridManager Object
        GameObject Test = new GameObject();
        Test.AddComponent<GridManager>();
        GridManager TestGM = Test.GetComponent<GridManager>();

        TestGM.GridRadius = 10;
        yield return null;
        
        TestGM.HexTilePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(hexTilePrefabPath);

        TestGM.Awake();

        yield return null;

        // Sets up the HexPosition Object
        Debug.Log(TestGM.Grid);

        GameObject Test1 = new GameObject();
        Test1.AddComponent<HexPosition>();
        HexPosition TestHP = Test1.GetComponent<HexPosition>();

        TestHP.gridManager = TestGM;
        TestHP.GridRadius = TestGM.GridRadius;

        yield return null;

        // Tests A bad spawn location
        TestHP.SetQRS(1, 0, 0);

        Assert.AreEqual(TestHP.q, 1);
        Assert.AreEqual(TestHP.r, 0);
        Assert.AreEqual(TestHP.s, 0);

        Assert.IsFalse(TestHP.SetPosition());

        yield return null;

        // Tests a good spawn location
        TestHP.SetQRS(1, 0, -1);

        Assert.AreEqual(TestHP.q, 1);
        Assert.AreEqual(TestHP.r, 0);
        Assert.AreEqual(TestHP.s, -1);

        Assert.IsTrue(TestHP.SetPosition());

        yield return null;

        // Tests a good movement
        Assert.IsTrue(TestHP.UpdatePosition(0, -1, 1) == 1);

        Assert.AreEqual(TestHP.q, 1);
        Assert.AreEqual(TestHP.r, -1);
        Assert.AreEqual(TestHP.s, 0);

        yield return null;

        // Tests a bad movement
        Assert.IsTrue(TestHP.UpdatePosition(0, -10, 1) < 0);

        // Should not change if movement is bad
        Assert.AreEqual(TestHP.q, 1);
        Assert.AreEqual(TestHP.r, -1);
        Assert.AreEqual(TestHP.s, 0);
    }
}
