using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Map;

namespace Map {
public class QuestionDialogueScript : MonoBehaviour
{
    // Question Text in textbox
    [SerializeField] private TMP_Text textBox;

    // Buttons Yes and No
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

private void Awake() {
//    Transform textBoxTransform = transform.Find("TextBox");
    Transform yesButtonTransform = transform.Find("YesButton");
    Transform noButtonTransform = transform.Find("NoButton");

//    if (textBoxTransform == null) Debug.LogError("TextBox not found!");
    if (yesButtonTransform == null) Debug.LogError("YesButton not found!");
    if (noButtonTransform == null) Debug.LogError("NoButton not found!");

//    textBox = transform.Find("TextBox").GetComponent<TextMeshProUGUI>();
    yesButton = yesButtonTransform?.GetComponent<Button>();
    noButton = noButtonTransform?.GetComponent<Button>();

//    if (textBox == null) Debug.LogError("TMP_Text component not found");
    if (yesButton == null) Debug.LogError("Button component not found on YesButton!");
    if (noButton == null) Debug.LogError("Button component not found on NoButton!");

    ShowQuestion("Do you want to do this?", () => {
        Debug.Log("Yes");
    }, () => {
        Debug.Log("No");
    });
}

    // ShowQuestion will modify QuestionDialogue scene to show the questionText and 
    // modifies the onclick function of Yes and No buttons to yesAction and noAction respectively
    public void ShowQuestion(string questionText, Action yesAction, Action noAction) {
//        textBox.text = questionText;
        yesButton.onClick.AddListener(() => { 
            Hide();
            yesAction();
        });
        noButton.onClick.AddListener(() => { 
            Hide();
            noAction();
        });

    }

    // Hides 
    private void Hide() {
        gameObject.SetActive(false);
    }
}
}