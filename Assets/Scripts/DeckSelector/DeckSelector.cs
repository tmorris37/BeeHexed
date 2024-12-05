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
    // assigned in prefab
    [SerializeField] private Button viewButton;
    private Color selectColor;
    private bool selected;
    private SelectionManager selectionManager;

    void Awake()
    {
        selectionManager = FindObjectOfType<SelectionManager>();
        highlight.SetActive(false); 
        selectColor = new Color(0, 255, 208, 255f);
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
            transform.localScale *= hoverScale;
            viewButton.gameObject.SetActive(true);
        } else {
            highlight.GetComponent<Image>().color = Color.white;
            highlight.SetActive(false);
            transform.localScale /= hoverScale;
            viewButton.gameObject.SetActive(false);
        }
    }

    public void RevertToHoverState() {
        highlight.SetActive(true);
    }

    public void RevertToNonHoverState() {
        transform.localScale /= hoverScale;
    }

    public void LoadDeckPage() {
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
