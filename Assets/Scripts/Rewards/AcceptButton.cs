
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AcceptButton : MonoBehaviour
{
    [SerializeField] private bool DEBUG_MODE;
    [SerializeField] private string deckSavePath = "Assets/Deck/Deck.json";
    [SerializeField] private string mapScene = "Overworld";
    private List<Card> deck = new();
    private void Start() {
      string JSONPlainText = File.ReadAllText(deckSavePath);
      if (DEBUG_MODE) Debug.Log("Read JSON: " + JSONPlainText);
      // String (JSON) -> List
      IList<string> cardPaths = JsonConvert.DeserializeObject<List<string>>(JSONPlainText);
      
      // add each card from file
      foreach (string path in cardPaths) {
        if (DEBUG_MODE) Debug.Log("Path: " + path);
        deck.Add(Resources.Load<Card>(path));
      }
      if (DEBUG_MODE) Debug.Log("Read Deck: " + deck);
    }
    private void LoadMapScene() {
      if (MusicManager.Instance != null)
      {
        MusicManager.Instance.PlayNewGameMusic();
      }
      if (DEBUG_MODE) Debug.Log("Loading map...");
      SceneManager.LoadScene(mapScene);
    }
    public void WriteDeckWithRewardAndLoad() {
      RewardManager rm = FindObjectOfType<RewardManager>();
      if (DEBUG_MODE) Debug.Log(rm);
      // two copies of the reward card
      deck.Add(rm.GetRewardCard());
      deck.Add(rm.GetRewardCard());
      List<string> cardNames = new();
      foreach (Card card in deck){
        cardNames.Add("Cards/" + card.cardName);
      }
      if (DEBUG_MODE) Debug.Log("Deck Size: " + cardNames.Count);
      string jsonDeck = JsonConvert.SerializeObject(cardNames);
      if (DEBUG_MODE) Debug.Log("Written Reward Deck: " + jsonDeck);
      File.WriteAllText(deckSavePath, jsonDeck);
      LoadMapScene();
    }
   
}
