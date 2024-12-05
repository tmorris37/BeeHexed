using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    private string selected;
    private string baseSelected = "None";
    private Dictionary<string, DeckSelector> decks = new();
    void Awake()
    {
        selected = baseSelected;
        DeckSelector[] decks = GetComponentsInChildren<DeckSelector>();
        foreach (DeckSelector deck in decks) {
            this.decks.Add(deck.gameObject.name, deck);
        }
    }
    public void Select(string name)
    {
        bool res = decks.TryGetValue(selected, out DeckSelector prevDeck);
        if (res) {
            prevDeck.SetSelected(false);
        }
        if (selected != name) {
            selected = name;
            if (res) {
                prevDeck.RevertToNonHoverState();
            }
            res = decks.TryGetValue(selected, out DeckSelector currDeck);
            if (res) {
                currDeck.SetSelected(true);
            }
        } else {
            selected = "None";
            prevDeck.RevertToHoverState();
        }
        
    }

     public void LoadDeckPage() {
        SceneManager.LoadScene("DeckSummary");
    }

    public void LoadMapPage() {
        SceneManager.LoadScene("Map");
    }
    
}
