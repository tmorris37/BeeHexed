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
    }

    public void MakeMapActive() {
        this.persistentMap.SetActive(true);
    }

    public void MakeMapInactive() {
        this.persistentMap.SetActive(false);
    }
}
