using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try {
            GameObject.Find("MapManager").GetComponent<MapManager>().DestroyMap();
        }
        catch {
            
        }
    }

}
