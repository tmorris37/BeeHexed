using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using TMPro;

public class DrawDiscardHandTest
{
    [UnityTest]

    // Unit test for DrawPileManager
    public IEnumerator DrawPileManagerTest()
    {
        GameObject Test = new GameObject();
        Test.AddComponent<DrawPileManager>();
        DrawPileManager TestDPM = Test.GetComponent<DrawPileManager>();
        Test.AddComponent<TextMeshProUGUI>();
        TestDPM.drawPileCounter = Test.GetComponent<TextMeshProUGUI>();
        TestDPM.DEVELOPER_MODE = true;
        // set up a test deck, knowing each will be added twice
        List<Card> myDeck = new()
        {
          Resources.Load<Card>("Cards/Wax"),
          Resources.Load<Card>("Cards/Pulser"),
          Resources.Load<Card>("Cards/Blizzard"),
          Resources.Load<Card>("Cards/Beemer")
        };
        TestDPM.DEV_setCustomDeck(myDeck);
        TestDPM.Awake();


        yield return null;


        // After setup there should be 8 cards since our custom deck has 4 and each card is added twice
        Assert.AreEqual(8, TestDPM.deck.Count);
        Assert.AreEqual("8", TestDPM.drawPileCounter.text);

        TestDPM.Shuffle();
        yield return null;
        // there is no way to confirm that cards have been shuffled randomly since every permutation is valid

        // confirm shuffling does not change card count
        Assert.AreEqual(8, TestDPM.deck.Count);
        Assert.AreEqual("8", TestDPM.drawPileCounter.text);

        TestDPM.generateDrawPile(TestDPM.deck);
        yield return null;
        // confirm generated pile has the same amount of cards as it was given
        Assert.AreEqual(8, TestDPM.deck.Count);
        Assert.AreEqual("8", TestDPM.drawPileCounter.text);

        // There are a few methods we can't test without a hand, like drawing and discarding
    }


    [UnityTest]
    public IEnumerator HandTest()
    {
        GameObject Test = new GameObject();
        Test.AddComponent<HandManager>();
        HandManager TestHM = Test.GetComponent<HandManager>();
        TestHM.spellCardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SpellCard.prefab");
        TestHM.towerCardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TowerCardPrefab Variant.prefab");
        TestHM.handLocation = TestHM.transform;
  
        TestHM.Start();


        yield return null;


        // At start hand should be empty by default
        Assert.AreEqual(0, TestHM.handSize);
        Assert.AreEqual(TestHM.cardsInHand.Count, TestHM.handSize);


        yield return null;

        Card[] cardList = Resources.LoadAll<Card>("Cards");
        TestHM.AddToHand(cardList[0]);


        Assert.AreEqual(1, TestHM.handSize);
        Assert.AreEqual(TestHM.cardsInHand.Count, TestHM.handSize);

        yield return null;

        TestHM.AddToHand(cardList[4]);

        Assert.AreEqual(2, TestHM.handSize);
        Assert.AreEqual(TestHM.cardsInHand.Count, TestHM.handSize);
        
        yield return null;

        for (int i = 0; i < 5; i++) {
          TestHM.AddToHand(cardList[i]);
        }

        Assert.AreEqual(7, TestHM.handSize);
        Assert.AreEqual(TestHM.cardsInHand.Count, TestHM.handSize);


    }
    [UnityTest]
    public IEnumerator DiscardTest()
    {

        GameObject Test = new GameObject();
        Test.AddComponent<DiscardManager>();
        DiscardManager TestDM = Test.GetComponent<DiscardManager>();
        Test.AddComponent<TextMeshProUGUI>();
        TestDM.graveyardSizeText = Test.GetComponent<TextMeshProUGUI>();
        
  
        TestDM.Awake();
        Card[] cardList = Resources.LoadAll<Card>("Cards");

        yield return null;


        // At start discard should be empty by default
        Assert.AreEqual(0, TestDM.graveyardSize);
        Assert.AreEqual("0", TestDM.graveyardSizeText.text);

        Assert.IsFalse(TestDM.drawFromGraveyard(null));
        Assert.IsFalse(TestDM.drawFromGraveyard(cardList[0]));

        yield return null;


        TestDM.discard(null);

        // discarding null should not work
        Assert.AreEqual(0, TestDM.graveyardSize);
        Assert.AreEqual("0", TestDM.graveyardSizeText.text);
        Assert.IsFalse(TestDM.drawFromGraveyard(null));
        Assert.IsFalse(TestDM.drawFromGraveyard(cardList[0]));

        yield return null;

        
        TestDM.discard(cardList[0]);

        Assert.AreEqual(1, TestDM.graveyardSize);
        Assert.AreEqual("1", TestDM.graveyardSizeText.text);
        Assert.IsTrue(TestDM.graveyard.Contains(cardList[0]));

        Assert.IsTrue(TestDM.drawFromGraveyard(cardList[0]));

        Assert.AreEqual(0, TestDM.graveyardSize);
        Assert.AreEqual("0", TestDM.graveyardSizeText.text);
        Assert.IsFalse(TestDM.graveyard.Contains(cardList[0]));

        yield return null;

        TestDM.discard(cardList[4]);
        TestDM.discard(cardList[0]);
        

        Assert.AreEqual(2, TestDM.graveyardSize);
        Assert.AreEqual("2", TestDM.graveyardSizeText.text);
        Assert.IsTrue(TestDM.graveyard.Contains(cardList[0]));
        Assert.IsTrue(TestDM.graveyard.Contains(cardList[4]));

        Assert.IsTrue(TestDM.drawFromGraveyard(cardList[4]));

        Assert.AreEqual(1, TestDM.graveyardSize);
        Assert.AreEqual("1", TestDM.graveyardSizeText.text);
        Assert.IsFalse(TestDM.graveyard.Contains(cardList[4]));

        yield return null;

        for (int i = 0; i < 5; i++) {
          TestDM.discard(cardList[i]);
        }
        Assert.AreEqual(6, TestDM.graveyardSize);
        Assert.AreEqual("6", TestDM.graveyardSizeText.text);
        for (int i = 0; i < 5; i++) {
          Assert.IsTrue(TestDM.graveyard.Contains(cardList[i]));
        }

        List<Card> returnList = TestDM.drawAllGraveyard();
        Assert.AreEqual(0, TestDM.graveyardSize);
        Assert.AreEqual("0", TestDM.graveyardSizeText.text);
        for (int i = 0; i < 5; i++) {
          Assert.IsFalse(TestDM.graveyard.Contains(cardList[i]));
          Assert.IsTrue(returnList.Contains(cardList[i]));
        }

        yield return null;
    }
    [UnityTest]
    public IEnumerator DrawDiscardHandIntegrationTest()
    {
        GameObject Test = new GameObject();
        Test.AddComponent<HandManager>();
        Test.AddComponent<DrawPileManager>();
        Test.AddComponent<DiscardManager>();
        HandManager TestHM = Test.GetComponent<HandManager>();
        DiscardManager TestDM = Test.GetComponent<DiscardManager>();
        DrawPileManager TestDPM = Test.GetComponent<DrawPileManager>();
        TestDPM.DEVELOPER_MODE = true;
        // set up a test deck, knowing each will be added twice
        List<Card> myDeck = new()
        {
          Resources.Load<Card>("Cards/Wax"),
          Resources.Load<Card>("Cards/Pulser"),
          Resources.Load<Card>("Cards/Blizzard"),
          Resources.Load<Card>("Cards/Sacrificial Bargain"),
          Resources.Load<Card>("Cards/Beemer")
        };
        TestDPM.DEV_setCustomDeck(myDeck);
        TestHM.spellCardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SpellCard.prefab");
        TestHM.towerCardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TowerCardPrefab Variant.prefab");
        Test.AddComponent<TextMeshProUGUI>();
        TestDM.graveyardSizeText = Test.GetComponent<TextMeshProUGUI>();
        TestDPM.drawPileCounter = Test.GetComponent<TextMeshProUGUI>();
        TestHM.handLocation = TestHM.transform;
        TestDM.Awake();
        TestDPM.Awake();
        TestHM.Start();

        yield return null;

        Assert.AreEqual(10, TestDPM.deck.Count);
        Assert.AreEqual(0, TestHM.handSize);
        Assert.AreEqual(0, TestDM.graveyardSize);

        yield return null;

        TestDPM.DrawCard(TestHM);
        Assert.AreEqual(9, TestDPM.deck.Count);
        Assert.AreEqual(1, TestHM.handSize);
        Assert.AreEqual(0, TestDM.graveyardSize);

        yield return null;
        
        TestHM.DiscardCard(TestHM.cardsInHand[0].GetComponent<CardDisplay>().cardData);
        Assert.AreEqual(9, TestDPM.deck.Count);
        Assert.AreEqual(0, TestHM.handSize);
        Assert.AreEqual(1, TestDM.graveyardSize);

        yield return null;

        for (int i = 0; i < 8; i++) {
          TestDPM.DrawCard(TestHM);
        }
        // max hand size is 8, so no cards should be drawn after
        Assert.AreEqual(1, TestDPM.deck.Count);
        Assert.AreEqual(8, TestHM.handSize);
        Assert.AreEqual(1, TestDM.graveyardSize);

        yield return null;

        for (int i = 0; i < 8; i++) {
          TestHM.DiscardCard(TestHM.cardsInHand[0].GetComponent<CardDisplay>().cardData);
        }
        Assert.AreEqual(1, TestDPM.deck.Count);
        Assert.AreEqual(0, TestHM.handSize);
        Assert.AreEqual(9, TestDM.graveyardSize);

        yield return null;

        for (int i = 0; i < 8; i++) {
          TestDPM.DrawCard(TestHM);
          TestHM.DiscardCard(TestHM.cardsInHand[0].GetComponent<CardDisplay>().cardData);
        }
        Assert.AreEqual(3, TestDPM.deck.Count);
        Assert.AreEqual(0, TestHM.handSize);
        Assert.AreEqual(7, TestDM.graveyardSize);
        for (int i = 0; i < 3; i++) {
            TestDPM.DrawCard(TestHM);
        }
        Assert.AreEqual(0, TestDPM.deck.Count);
        Assert.AreEqual(3, TestHM.handSize);
        Assert.AreEqual(7, TestDM.graveyardSize);
    }
}

