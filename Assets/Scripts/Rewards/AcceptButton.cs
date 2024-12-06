
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
    [SerializeField] private string savePath = "Assets/Deck/Save.json";
    [SerializeField] private string mapScene = "Assets/STSMap_Gen_Package/Scenes/Overworld.unity";
    private PlayerData saveData;
    private void Start() {
      string JSONPlainText = File.ReadAllText(savePath);
      if (DEBUG_MODE) Debug.Log("Read JSON: " + JSONPlainText);
      // String (JSON) -> List
      saveData = JsonConvert.DeserializeObject<PlayerData>(JSONPlainText);
      if (DEBUG_MODE) Debug.Log("Read Deck: " + saveData.cardPaths);
    }
    private void LoadMapScene() {
      if (MusicManager.Instance != null)
      {
        MusicManager.Instance.PlayNewGameMusic();
      }
      if (DEBUG_MODE) Debug.Log("Loading map...");
      SceneManager.LoadScene(mapScene);
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
