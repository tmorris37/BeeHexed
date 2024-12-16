using UnityEngine;

// Reset the random generator for the level based on the level name and the seed
public class SeededLevelInit : MonoBehaviour {
    public string levelName;
    public void Awake() {
        levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        // Checks if Seed is null and if it is creates a new one
        // This is really only for use in the editor, as the seed should be set
        // aleady if the game is run from the main menu
        if (Seed.Instance == null) {
            GameObject seedObject = new GameObject("SeedContainer");
            Seed seed = seedObject.AddComponent<Seed>();
            seed.SetSeedFromTime();
        }
        SetRandomGenerator();

        // Creates a GameSpeedManager the first time a level is loaded
        if (GameSpeedManager.Instance == null) {
            GameObject gameSpeedObject = new GameObject("GameSpeedManager");
            GameSpeedManager gameSpeedManager = gameSpeedObject.AddComponent<GameSpeedManager>();
            gameSpeedManager.SetGameSpeed(1.0f);
        }
    }

    // Set the random generator based on the level name
    public void SetRandomGenerator() {
        Seed.Instance.SetRandomGenerator(levelName.GetHashCode());
    }
}
