using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

public class MainMenu : MonoBehaviour 
{ 

    [SerializeField] private string deckSavePath = "Assets/Deck/Deck.json";
    [SerializeField] private string tempPath = "Assets/Deck/temp.json";

   // Switches Scene to input Scene, sceneName
    public void GoToScene(string sceneName) 
    {
        // if (MusicManager.Instance != null)
        //     {
        //         if (sceneName == "Overworld")
        //         {
        //             MusicManager.Instance.PlayNewGameMusic();
        //         }
        //     }
        SceneManager.LoadScene(sceneName);
    }

    // Quits the Game
    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Application has quit");
    }

    public void WriteBaseDeckToFile() {
      // by default, we use two copies of every card in Resources as our starting deck
      List<Card> deck = new List<Card>(Resources.LoadAll<Card>("StartingCards/"));
      List<string> cardNames = new();
      foreach (Card card in deck){
        cardNames.Add("Cards/" + card.cardName);
        cardNames.Add("Cards/" + card.cardName);
      }
      string jsonDeck = JsonConvert.SerializeObject(cardNames);
      File.WriteAllText(deckSavePath, jsonDeck);
      string jsonTemp = JsonConvert.SerializeObject("None");
      File.WriteAllText(tempPath, jsonTemp);
    }
}
