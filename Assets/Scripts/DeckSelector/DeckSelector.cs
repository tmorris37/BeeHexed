using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckSelector : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject highlight;
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private string savePath = "Assets/Deck/Save.json";
    [SerializeField] private bool DEBUG_MODE = false;

    void Awake()
    {
        highlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData) {
        highlight.SetActive(true);
        transform.localScale *= hoverScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        WriteDeckNameToFile(name);
        LoadDeckPage();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlight.SetActive(false);
        transform.localScale /= hoverScale;
    }

    private void LoadDeckPage() {
        SceneManager.LoadScene("DeckSummary");
    }

    private void WriteDeckNameToFile(string deckName) {
        string jsonDeckName = JsonConvert.SerializeObject(deckName);
        if (DEBUG_MODE) Debug.Log("Written Deck Name: " + jsonDeckName);
        File.WriteAllText(savePath, jsonDeckName);
    }
}
