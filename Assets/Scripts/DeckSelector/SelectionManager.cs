using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    private string selected;
    private string baseSelected = "None";
    private Dictionary<string, DeckSelector> decks = new();
    void Awake()
    {
        continueButton.gameObject.SetActive(false);
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
            continueButton.gameObject.SetActive(true);
            if (res) {
                prevDeck.RevertToNonHoverState();
            }
            res = decks.TryGetValue(selected, out DeckSelector currDeck);
            if (res) {
                currDeck.SetSelected(true);
            }
        } else {
            selected = "None";
            continueButton.gameObject.SetActive(false);
            prevDeck.RevertToHoverState();
        }
        
    }


    public void LoadMapPage() {
        SceneManager.LoadScene("Overworld");
    }
    
}
