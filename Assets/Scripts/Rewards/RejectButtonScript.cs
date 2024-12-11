using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RejectButtonScript : MonoBehaviour {
    [SerializeField] private bool DEBUG_MODE;
    public void LoadMapScene() {
      if (MusicManager.Instance != null)
      {
        MusicManager.Instance.PlayNewGameMusic();
      }
      if (DEBUG_MODE) Debug.Log("Loading map...");

      GameObject.Find("MapManager").GetComponent<MapManager>().MakeMapActive();
      SceneManager.LoadScene("OverworldToyBox");
    }
}
