
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
    [SerializeField] private string mapScene = "Assets/STSMap_Gen_Package/Scenes/Overworld.unity";
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
        PlayerData.cardPaths.Add("Cards/" + rm.GetRewardCard().cardName);
        if (DEBUG_MODE) Debug.Log("Deck Size: " + PlayerData.cardPaths.Count);
        LoadMapScene();
    }
   
}
