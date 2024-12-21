using UnityEngine;

// Reset the random generator for the map based on the map name and the seed
public class SeededMapInit : MonoBehaviour {
    public string mapName;
    public void Awake() {
        mapName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        // Checks if Seed is null and if it is creates a new one
        // This is reallu only for use in the editor, as the seed should be set 
        // aleady if the game is run from the main menu
        if (Seed.Instance == null) {
            GameObject seedObject = new GameObject("SeedContainer");
            Seed seed = seedObject.AddComponent<Seed>();
            seed.SetSeedFromTime();
        }
        SetRandomGenerator();
    }

    // Set the random generator based on the map name
    public void SetRandomGenerator() {
        Seed.Instance.SetRandomGenerator(mapName.GetHashCode());
    }
}
