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
    // All of the asserts are backwards, TODO: Change later

    // Unit test for DrawPileManager
    public IEnumerator DrawPileManagerTest()
    {
        GameObject Test = new GameObject();
        Test.AddComponent<DrawPileManager>();
        DrawPileManager TestDPM = Test.GetComponent<DrawPileManager>();
        Test.AddComponent<TextMeshProUGUI>();
        TestDPM.drawPileCounter = Test.GetComponent<TextMeshProUGUI>();
        TestDPM.Awake();


        yield return null;


        // After setup there should be 20 cards in the deck, and it should be displayed as such
        Assert.AreEqual(TestDPM.deck.Count, 20);
        Assert.AreEqual(TestDPM.drawPileCounter.text, "20");

        TestDPM.Shuffle();
        yield return null;
        // there is no way to confirm that cards have been shuffled randomly since every permutation is valid

        // confirm shuffling does not change card count
        Assert.AreEqual(TestDPM.deck.Count, 20);
        Assert.AreEqual(TestDPM.drawPileCounter.text, "20");

        TestDPM.generateDrawPile(TestDPM.deck);
        yield return null;
        // confirm generated pile has the same amount of cards as it was given
        Assert.AreEqual(TestDPM.deck.Count, 20);
        Assert.AreEqual(TestDPM.drawPileCounter.text, "20");

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
        Assert.AreEqual(TestHM.handSize, 0);
        Assert.AreEqual(TestHM.handSize, TestHM.cardsInHand.Count);


        yield return null;

        Card[] cardList = Resources.LoadAll<Card>("Cards");
        TestHM.AddToHand(cardList[0]);


        Assert.AreEqual(1, TestHM.handSize);
        Assert.AreEqual(TestHM.handSize, TestHM.cardsInHand.Count);

        yield return null;

        TestHM.AddToHand(cardList[5]);

        Assert.AreEqual(TestHM.handSize, 2);
        Assert.AreEqual(TestHM.handSize, TestHM.cardsInHand.Count);
        
        yield return null;

        for (int i = 0; i < 10; i++) {
          TestHM.AddToHand(cardList[i]);
        }

        Assert.AreEqual(TestHM.handSize, 12);
        Assert.AreEqual(TestHM.handSize, TestHM.cardsInHand.Count);


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
        Assert.AreEqual(TestDM.graveyardSize, 0);
        Assert.AreEqual(TestDM.graveyardSizeText.text, "0");

        Assert.IsFalse(TestDM.drawFromGraveyard(null));
        Assert.IsFalse(TestDM.drawFromGraveyard(cardList[0]));

        yield return null;


        TestDM.discard(null);

        // discarding null should not work
        Assert.AreEqual(TestDM.graveyardSize, 0);
        Assert.AreEqual(TestDM.graveyardSizeText.text, "0");
        Assert.IsFalse(TestDM.drawFromGraveyard(null));
        Assert.IsFalse(TestDM.drawFromGraveyard(cardList[0]));

        yield return null;

        
        TestDM.discard(cardList[0]);

        Assert.AreEqual(TestDM.graveyardSize, 1);
        Assert.AreEqual(TestDM.graveyardSizeText.text, "1");
        Assert.IsTrue(TestDM.graveyard.Contains(cardList[0]));

        Assert.IsTrue(TestDM.drawFromGraveyard(cardList[0]));

        Assert.AreEqual(TestDM.graveyardSize, 0);
        Assert.AreEqual(TestDM.graveyardSizeText.text, "0");
        Assert.IsFalse(TestDM.graveyard.Contains(cardList[0]));

        yield return null;

        TestDM.discard(cardList[6]);
        TestDM.discard(cardList[0]);
        

        Assert.AreEqual(TestDM.graveyardSize, 2);
        Assert.AreEqual(TestDM.graveyardSizeText.text, "2");
        Assert.IsTrue(TestDM.graveyard.Contains(cardList[0]));
        Assert.IsTrue(TestDM.graveyard.Contains(cardList[6]));

        Assert.IsTrue(TestDM.drawFromGraveyard(cardList[6]));

        Assert.AreEqual(TestDM.graveyardSize, 1);
        Assert.AreEqual(TestDM.graveyardSizeText.text, "1");
        Assert.IsFalse(TestDM.graveyard.Contains(cardList[6]));

        yield return null;

        for (int i = 0; i < 10; i++) {
          TestDM.discard(cardList[i]);
        }
        Assert.AreEqual(TestDM.graveyardSize, 11);
        Assert.AreEqual(TestDM.graveyardSizeText.text, "11");
        for (int i = 0; i < 10; i++) {
          Assert.IsTrue(TestDM.graveyard.Contains(cardList[i]));
        }

        List<Card> returnList = TestDM.drawAllGraveyard();
        Assert.AreEqual(TestDM.graveyardSize, 0);
        Assert.AreEqual(TestDM.graveyardSizeText.text, "0");
        for (int i = 0; i < 10; i++) {
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

        Assert.AreEqual(TestDPM.deck.Count, 20);
        Assert.AreEqual(TestHM.handSize, 0);
        Assert.AreEqual(TestDM.graveyardSize, 0);

        yield return null;

        TestDPM.DrawCard(TestHM);
        Assert.AreEqual(TestDPM.deck.Count, 19);
        Assert.AreEqual(TestHM.handSize, 1);
        Assert.AreEqual(TestDM.graveyardSize, 0);

        yield return null;
        
        TestHM.DiscardCard(TestHM.cardsInHand[0].GetComponent<CardDisplay>().cardData);
        Assert.AreEqual(TestDPM.deck.Count, 19);
        Assert.AreEqual(TestHM.handSize, 0);
        Assert.AreEqual(TestDM.graveyardSize, 1);

        yield return null;

        for (int i = 0; i < 9; i++) {
          TestDPM.DrawCard(TestHM);
        }
        // max hand size is 8, so no cards should be drawn after
        Assert.AreEqual(TestDPM.deck.Count, 11);
        Assert.AreEqual(TestHM.handSize, 8);
        Assert.AreEqual(TestDM.graveyardSize, 1);

        yield return null;

        for (int i = 0; i < 8; i++) {
          TestHM.DiscardCard(TestHM.cardsInHand[0].GetComponent<CardDisplay>().cardData);
        }
        Assert.AreEqual(TestDPM.deck.Count, 11);
        Assert.AreEqual(TestHM.handSize, 0);
        Assert.AreEqual(TestDM.graveyardSize, 9);

        yield return null;

        for (int i = 0; i < 8; i++) {
          TestDPM.DrawCard(TestHM);
          TestHM.DiscardCard(TestHM.cardsInHand[0].GetComponent<CardDisplay>().cardData);
        }
        Assert.AreEqual(TestDPM.deck.Count, 3);
        Assert.AreEqual(TestHM.handSize, 0);
        Assert.AreEqual(TestDM.graveyardSize, 17);
        for (int i = 0; i < 5; i++) {
            TestDPM.DrawCard(TestHM);
        }
        Assert.AreEqual(TestDPM.deck.Count, 15);
        Assert.AreEqual(TestHM.handSize, 5);
        Assert.AreEqual(TestDM.graveyardSize, 0);

    }
}

