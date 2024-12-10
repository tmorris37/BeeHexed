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
    [SerializeField] private bool DEBUG_MODE = false;
    // assigned in prefab
    [SerializeField] private Color themeColor;
    private Color selectColor;
    private bool selected;
    private SelectionManager selectionManager;

    void Awake()
    {
        selectionManager = FindObjectOfType<SelectionManager>();
        highlight.SetActive(false); 
        // TODO: Not hard-code this
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
        StoreDeckNameColor(name);
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
        string selectText = DeckSelected.selectedDeck;
        if (selectText == gameObject.name) {
            // clears previous selection
            selectionManager.Reset();
            selectionManager.Select(selectText);
            transform.localScale *= hoverScale;
        }
    }

    private void StoreDeckNameColor(string name) {
        PlayerData.deckName = name;
        if (DEBUG_MODE) Debug.Log("Stored deck Name: " + PlayerData.deckName);
        PlayerData.themeColor = BasicColor.ConvertToBasicColor(themeColor);
    }


}
