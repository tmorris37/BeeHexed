using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button viewButton;
    // [SerializeField] private string tempPath = "Assets/Deck/temp.json";
    private string selected;
    private string baseSelected = "None";
    private Dictionary<string, DeckSelector> decks = new();
    void Awake() {
        continueButton.gameObject.SetActive(false);
        viewButton.gameObject.SetActive(false);
        selected = baseSelected;
        DeckSelector[] decks = GetComponentsInChildren<DeckSelector>();
        foreach (DeckSelector deck in decks) {
            this.decks.Add(deck.gameObject.name, deck);
        }
    }
    public void Select(string name) {
        // deselect previously selected deck if it exists
        bool res = decks.TryGetValue(selected, out DeckSelector prevDeck);
        if (res) {
            prevDeck.SetSelected(false);
        }
        // if we selected a new deck
        if (selected != name) {
            selected = name;
            continueButton.gameObject.SetActive(true);
            viewButton.gameObject.SetActive(true);
            if (res) {
                prevDeck.RevertToNonHoverState();
            }
            res = decks.TryGetValue(selected, out DeckSelector currDeck);
            if (res) {
                currDeck.SetSelected(true);
                viewButton.image.color = currDeck.GetThemeColor();
            }
        } else {
            selected = baseSelected;
            continueButton.gameObject.SetActive(false);
            viewButton.gameObject.SetActive(false);
            prevDeck.RevertToHoverState();
        }
    }

    public void Reset() {
        selected = baseSelected;
    }

    // Writes the chosen deck to file and loads the map
    public void LoadMapPage() {
        bool res = decks.TryGetValue(selected, out DeckSelector currDeck);
        if (res) {
            WriteDeckToFile(currDeck.gameObject.name);
            
            SceneManager.LoadScene("OverworldToyBox");
        } else {
            throw new NullReferenceException("Selected deck does not exist");
        }
    }

    public void LoadDeckView() {
        DeckSelected.selectedDeck = selected;
        SceneManager.LoadScene("DeckSummary");
    }

    private void WriteDeckToFile(string deckName) {

        // A persistent location to store written data
        // On Windows: ..\AppData\LocalLow\defaultcompany\BeeHexed\DeckAssets\Save.json
        string filepath = Application.persistentDataPath + "/DeckAssets";

        string saveFilepath = filepath + "/Save.json";
        string jsonPlayerData = File.ReadAllText(saveFilepath);
        PlayerData saveData = JsonConvert.DeserializeObject<PlayerData>(jsonPlayerData);

        string deckFilepath = "DeckAssets/" + deckName;
        string jsonCardPaths = Resources.Load<TextAsset>(deckFilepath).text;
        IList<string> paths = JsonConvert.DeserializeObject<IList<string>>(jsonCardPaths);
        
        saveData.cardPaths = paths;
        jsonPlayerData = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(saveFilepath, jsonPlayerData);
    }
}
