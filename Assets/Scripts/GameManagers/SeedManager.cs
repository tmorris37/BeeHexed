using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;


public class SeedManager : MonoBehaviour {
    public Seed seed;
    public TMP_InputField inputField;

    void Start() {
        seed = Seed.Instance;
        inputField.text = seed.gameSeed;
    }
    
    public void SetSeedFromInputField() {
        string newSeed = inputField.text;
        seed.SetSeed(newSeed);
    }
}
