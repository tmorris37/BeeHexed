using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClicker : MonoBehaviour
{
    public void ButtonClickSFX() {
        if (SFXManager.Instance != null) {
            SFXManager.Instance.PlayButtonClick();
        }
    }
}
