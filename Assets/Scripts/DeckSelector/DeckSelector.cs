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
    [SerializeField] private string tempPath = "Assets/Deck/temp.json";
    [SerializeField] private bool DEBUG_MODE = false;
    [SerializeField] private Color themeColor;
    // assigned in prefab
    private Color selectColor;
    private bool selected;
    private SelectionManager selectionManager;

    void Awake()
    {
        selectionManager = FindObjectOfType<SelectionManager>();
        highlight.SetActive(false); 
        selectColor = new Color(0, 255, 208, 255f);
    }

    void Start() {
        CheckSelected();
    }


    public void OnPointerEnter(PointerEventData eventData) {
        if (!selected) {
            highlight.SetActive(true);
            transform.localScale *= hoverScale;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        WriteDeckNameToFile(name);
        selectionManager.Select(name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!selected) {
            highlight.SetActive(false);
            transform.localScale /= hoverScale;
        }
    }

    public void SetSelected(bool whether) {
        selected = whether;
        if (selected) {
            highlight.GetComponent<Image>().color = selectColor;
            highlight.SetActive(true);
            transform.localScale *= hoverScale;
        } else {
            highlight.GetComponent<Image>().color = Color.white;
            highlight.SetActive(false);
            transform.localScale /= hoverScale;
        }
    }

    public void RevertToHoverState() {
        highlight.SetActive(true);
    }

    public void RevertToNonHoverState() {
        transform.localScale /= hoverScale;
    }

    public Color GetThemeColor() {
        return themeColor;
    }

    private void CheckSelected() {
        string jsonSelectText = File.ReadAllText(tempPath);
        string selectText = JsonConvert.DeserializeObject<string>(jsonSelectText);
        if (selectText == gameObject.name) {
            // clears previous selection
            selectionManager.Reset();
            selectionManager.Select(selectText);
            transform.localScale *= hoverScale;
        }
    }

    private void WriteDeckNameToFile(string name) {
        PlayerData playerData = new()
        {
            deckName = name,
            cardPaths = null,
            themeColor = BasicColor.ConvertToBasicColor(themeColor)
        };
        string jsonData = JsonConvert.SerializeObject(playerData);
        if (DEBUG_MODE) Debug.Log("Written Deck Name: " + playerData.deckName);
        File.WriteAllText(savePath, jsonData);
    }


}
