using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpeedManager : MonoBehaviour
{
    public static GameSpeedManager Instance { get; private set; }
    public bool DEBUG_MODE = false;
    public float gameSpeed = 1.0f;

    public void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start() {}

    public void SetGameSpeed(float speed) {
        gameSpeed = speed;
        Time.timeScale = speed;
        if (DEBUG_MODE) Debug.Log("Game speed set to: " + speed);
    }
}
