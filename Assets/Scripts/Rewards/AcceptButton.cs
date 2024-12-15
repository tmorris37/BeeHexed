
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AcceptButton : MonoBehaviour
{
    [SerializeField] private bool DEBUG_MODE;
    [SerializeField] private string savePath;
    [SerializeField] private string mapScene = "Assets/STSMap_Gen_Package/Scenes/Overworld.unity";
    [SerializeField] private string emptyMapScene = "EmptyMap";

    private PlayerData saveData;
    private void Start() {
        // A persistent location to store written data
        // On Windows: ..\AppData\LocalLow\defaultcompany\BeeHexed\DeckAssets\Save.json
        string filepath = Application.persistentDataPath + "/DeckAssets";
        savePath = filepath + "/Save.json";

        System.IO.Directory.CreateDirectory(filepath);

        string JSONPlainText = File.ReadAllText(savePath);
        if (DEBUG_MODE) Debug.Log("Read JSON: " + JSONPlainText);
        // String (JSON) -> List
        saveData = JsonConvert.DeserializeObject<PlayerData>(JSONPlainText);
        if (DEBUG_MODE) Debug.Log("Read Deck: " + saveData.cardPaths);
    }
    private void LoadMapScene() {
        if (MusicManager.Instance != null) MusicManager.Instance.PlayNewGameMusic();
        if (DEBUG_MODE) Debug.Log("Loading map...");

        GameObject.Find("MapManager").GetComponent<MapManager>().MakeMapActive();
        SceneManager.LoadScene(emptyMapScene);
    }
    public void WriteDeckWithRewardAndLoad() {
        RewardManager rm = FindObjectOfType<RewardManager>();
        if (DEBUG_MODE) Debug.Log(rm);
        // one copy of the reward card
        saveData.cardPaths.Add("Cards/" + rm.GetRewardCard().cardName);
        if (DEBUG_MODE) Debug.Log("Deck Size: " + saveData.cardPaths.Count);
        string jsonSave = JsonConvert.SerializeObject(saveData);
        if (DEBUG_MODE) Debug.Log("Written Reward Deck: " + jsonSave);
        File.WriteAllText(savePath, jsonSave);
        LoadMapScene();
    }
}
