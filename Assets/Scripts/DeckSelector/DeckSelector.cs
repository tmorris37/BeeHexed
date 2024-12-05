using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    [SerializeField] private Color themeColor;

    void Awake()
    {
        highlight.SetActive(false); 
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

    private void WriteDeckNameToFile(string name) {
        PlayerData playerData = new()
        {
            deckName = name,
            deck = null,
            themeColor = BasicColor.ConvertToBasicColor(themeColor)
        };
        string jsonData = JsonConvert.SerializeObject(playerData);
        if (DEBUG_MODE) Debug.Log("Written Deck Name: " + playerData.deckName);
        File.WriteAllText(savePath, jsonData);
    }

}
