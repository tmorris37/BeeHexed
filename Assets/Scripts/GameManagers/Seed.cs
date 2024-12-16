using UnityEngine;
using System;


public class Seed : MonoBehaviour {
    public static Seed Instance { get; private set; }
    public string gameSeed;
    public int currentSeed = 0;

    private System.Random randomGenerator;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject); // Destroy duplicates
            return;
        }
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start() {
        SetSeedFromTime();
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetSeedFromTime() {
        gameSeed = DateTime.Now.ToString();
        SetSeed(gameSeed);
    }

    public void SetSeed(string newSeed) {
        gameSeed = newSeed;
        currentSeed = gameSeed.GetHashCode();
        randomGenerator = new System.Random(currentSeed);
        UnityEngine.Random.InitState(currentSeed);
    }

    public int GetRandomInt(int min, int max) {
        return randomGenerator.Next(min, max);
    }

    public float GetRandomFloat() {
        return (float)randomGenerator.NextDouble();
    }
}
