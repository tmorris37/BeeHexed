using UnityEngine;
using System;


public class SeededLevelInit : MonoBehaviour {
    public string levelName;
    public void Awake() {
        levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        // Checks if Seed is null and if it is creates a new one
        if (Seed.Instance == null) {
            GameObject seedObject = new GameObject("SeedContainer");
            Seed seed = seedObject.AddComponent<Seed>();
            seed.SetSeedFromTime();
        }
        SetRandomGenerator();
    }

    public void SetRandomGenerator() {
        Seed.Instance.SetRandomGenerator(levelName.GetHashCode());
    }
}
