using UnityEngine;
using System;

// This is the seed manager, it is used to generate a seed for the game
// and to generate random numbers based on that seed.
// Currently is is used for both enemy spawns and level generation.
// could be split into two separate classes if needed.
public class Seed : MonoBehaviour {
    public static Seed Instance { get; private set; } // Singleton
    public string gameSeed;                           // Seed for the game
    public int hashedSeed = 0;                        // Hashed seed for the game
    private System.Random randomGenerator;            // Random number generator

    void Awake() {
        // Singleton pattern, ensure only one instance of this class
        if (Instance != null && Instance != this) {
            Destroy(gameObject); // Destroy duplicates
            return;
        }
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start() {
        // Set the seed from the current time and ensure persistence
        SetSeedFromTime();
        DontDestroyOnLoad(this.gameObject);
    }

    // Set the seed from the current time
    public void SetSeedFromTime() {
        gameSeed = DateTime.Now.ToString();
        SetSeed(gameSeed);
    }

    // Set the base seed from a string
    public void SetSeed(string newSeed) {
        gameSeed = newSeed;
        hashedSeed = gameSeed.GetHashCode();
        SetRandomGenerator(0);
    }

    // Set the random generator with the base seed and a bonus hash for extra randomness
    public void SetRandomGenerator(int bonusHash) {
        randomGenerator = new System.Random(hashedSeed + bonusHash);
    }

    // Get a random integer between min and max
    public int GetRandomInt(int min, int max) {
        return randomGenerator.Next(min, max);
    }

    // Get a random float between 0 and 1
    public float GetRandomFloat() {
        return (float)randomGenerator.NextDouble();
    }
}
