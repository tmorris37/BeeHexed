using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting.IonicZip;
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
    void Awake()
    {
        continueButton.gameObject.SetActive(false);
        viewButton.gameObject.SetActive(false);
        selected = baseSelected;
        DeckSelector[] decks = GetComponentsInChildren<DeckSelector>();
        foreach (DeckSelector deck in decks) {
            this.decks.Add(deck.gameObject.name, deck);
        }
    }
    public void Select(string name)
    {
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
        string jsonPlayerData = File.ReadAllText(Paths.savePath);
        PlayerData saveData = JsonConvert.DeserializeObject<PlayerData>(jsonPlayerData);
        string jsonCardPaths = File.ReadAllText(Paths.presetsPath + deckName + ".json");
        IList<string> paths = JsonConvert.DeserializeObject<IList<string>>(jsonCardPaths);
        saveData.cardPaths = paths;
        jsonPlayerData = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(Paths.savePath, jsonPlayerData);
    }
}
