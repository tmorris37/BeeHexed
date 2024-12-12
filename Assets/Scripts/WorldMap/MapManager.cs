using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] public GameObject persistentMap;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayNewGameMusic();
        }
    }

    public void DestroyMap() {
        Destroy(this.persistentMap);
        Destroy(gameObject);
    }

    public void MakeMapActive() {
        this.persistentMap.SetActive(true);
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayNewGameMusic();
        }
    }

    public void MakeMapInactive() {
        this.persistentMap.SetActive(false);
    }
}
