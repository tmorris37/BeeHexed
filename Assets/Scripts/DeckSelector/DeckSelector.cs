using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckSelector : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject highlight;
    [SerializeField] private float hoverScale = 1.1f;


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
        WriteDeckToFile(name);
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

    private void WriteDeckToFile(string deckName) {
        
    }
}
