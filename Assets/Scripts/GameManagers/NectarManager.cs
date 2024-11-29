using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NectarManager : MonoBehaviour
{

    public TextMeshProUGUI nectarText;
    private int currentNectar;
      
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetNectar(int value) {
      currentNectar = value;
    }

    public int GetNectar() {
      return currentNectar;
    }

    // Update is called once per frame
    void Update()
    {
        nectarText.text = currentNectar.ToString();
    }
}
