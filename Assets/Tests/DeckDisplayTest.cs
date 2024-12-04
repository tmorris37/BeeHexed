using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using TMPro;

public class DeckDisplayTest
{
    [UnityTest]

    // Unit test for CalculatePositions
    public IEnumerator CalcPositionsTest()
    {
        GameObject Test = new GameObject();
        Test.AddComponent<DeckSummary>();
        Test.AddComponent<TextMeshProUGUI>();
        DeckSummary TestDS = Test.GetComponent<DeckSummary>();
        TestDS.DEVELOPER_MODE = true;
        TestDS.DEBUG_MODE = true;
        TestDS.testWidth = 10;
        List<Card> myDeck = new()
        {
          Resources.Load<Card>("Cards/Wax"),
        };
        yield return null;
        TestDS.TEST_SetDeck(myDeck);
        Vector3[] results = TestDS.CalculatePositions();
        Assert.AreEqual(0, results[0].x);
        yield return null;
        myDeck.Add(Resources.Load<Card>("Cards/Wax"));
        TestDS.TEST_SetDeck(myDeck);
        results = TestDS.CalculatePositions();
        Assert.AreEqual(10/3, results[0].x);
        Assert.AreEqual(20/3, results[1].x);
    }
}
