using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Map;
using UnityEngine.UI;
public class MapScript : MonoBehaviour
{
    private string state = "Start"; // Tracks the current state

    // List of GameObjects (nodes) mapped to specific states
    public GameObject starting_node_1;
    public GameObject level_2_node_1_1;
    public GameObject level_2_node_1_2;
    public GameObject level_3_node_1_1;
    public GameObject level_3_node_1_2;
    public GameObject level_4_node_1_1;
    public GameObject level_4_node_1_2_1;
    public GameObject level_4_node_1_2_2;

    public GameObject starting_node_2;
    public GameObject level_2_node_2_1;
    public GameObject level_2_node_2_2;
    public GameObject level_3_node_2_1_1;
    public GameObject level_3_node_2_1_2;
    public GameObject level_3_node_2_2;
    public GameObject level_4_node_2_1;
    public GameObject level_4_node_2_1_2;
    public GameObject level_4_node_2_2;
    public GameObject final_node;

    private Dictionary<string, List<GameObject>> stateMap;
    // Switch the game state and update nodes
    public void SetState(string newState = "Start")
    {
        state = newState; // Update the state
        PlayerPrefs.SetString("MapState", state); // Save the state
        PlayerPrefs.Save(); // Write to disk
        Debug.Log($"State changed to: {state}");
        UpdateNodes();
    }

    // Switches Scene to input Scene
    public void GoToScene(string sceneName)
    {
        if (MusicManager.Instance != null)
        {
            if (sceneName == "MainMenu")
            {
                MusicManager.Instance.PlayMainMenuMusic();
            }
            else
            {
                MusicManager.Instance.PlayInGameMusic();
            }
        }
        SceneManager.LoadScene(sceneName);
    }
    private void Start()
    {
        state = PlayerPrefs.GetString("MapState", "Start");

        // Map states to their corresponding GameObjects
        // states based on previous button
        stateMap = new Dictionary<string, List<GameObject>>()
        {
            { "Start", new List<GameObject> { starting_node_1, starting_node_2 } },
            { "1", new List<GameObject> { level_2_node_1_1, level_2_node_1_2 }},
            { "2", new List<GameObject> { level_2_node_2_1, level_2_node_2_2 }},
            { "2.1.1", new List<GameObject> { level_3_node_1_1 } },
            { "2.1.2", new List<GameObject> { level_3_node_1_2 } },
            { "3.1.1", new List<GameObject> { level_4_node_1_1 } },
            { "3.1.2", new List<GameObject> { level_4_node_1_2_1, level_4_1_2_2 } },
            { "4.1.1", new List<GameObject> { final_node } },
            { "4.1.2.1", new List<GameObject> { final_node } },
            { "4.1.2.2", new List<GameObject> { final_node } },
            { "2.2.1", new List<GameObject> { level_3_node_2_1_1, level_3_node_2_1_2 } },
            { "2.2.2", new List<GameObject> { level_3_node_2_2 } },
            { "3.2.1.1", new List<GameObject> { level_4_node_2_1 } },
            { "3.2.1.2", new List<GameObject> { level_4_node_2_1_2, level_4_node_2_1 } },
            { "3.2.2", new List<GameObject> { level_4_node_2_2 } },
            { "4.2.1", new List<GameObject> { final_node } },
            { "4.2.1.2", new List<GameObject> { final_node } },
            { "4.2.2", new List<GameObject> { final_node }},
            { "Final", new List<GameObject> { starting_node_1, starting_node_2 }},
        };

        UpdateNodes();
    }

    private void UpdateNodes()
    {
        Debug.Log($"Current State: {state}");
        // Disable all nodes first
        foreach (var nodeList in stateMap.Values)
        {
            foreach (var node in nodeList)
            {
                Button button = node.GetComponentInChildren<Button>();
                if (button != null)
                    button.interactable = false; // Disable the button
            }
        }

        // Enable only the nodes corresponding to the current state
        if (stateMap.ContainsKey(state))
        {
            foreach (var node in stateMap[state])
            {
                Button button = node.GetComponentInChildren<Button>();
                if (button != null)
                    button.interactable = true; // Enable the button
            }
        }
    }
}