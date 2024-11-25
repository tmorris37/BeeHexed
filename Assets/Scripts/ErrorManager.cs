using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private int fadeTime = 10;
    void Start()
    {
        errorText.color = Color.red;
        errorText.text = "";
    }

    public void SetErrorMsg(string message) {
      errorText.alpha = 1f;
      errorText.text = message;
      StartCoroutine(Fade());
    }

    private IEnumerator Fade() {
      for (int i = 0; i < fadeTime; i++) {
        yield return new WaitForSeconds(1);
        errorText.alpha -= 1f / fadeTime;
      }
    }
}
