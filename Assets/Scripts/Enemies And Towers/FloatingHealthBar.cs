using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour {
    [SerializeField] private Slider slider;
    //[SerializeField] private Camera camera;
    //[SerializeField] private Transform target;
    //[SerializeField] private Vector3 offset;

    void Start() {
    }

    public void UpdateHealthBar(float currentValue, float maxValue) {
        slider.value = currentValue / maxValue;
    }

    public void Damage(float value) {
        slider.value -= value;
    }

    public float GetValue() {
        return slider.value;
    }

    // Update is called once per frame
    void Update() {
        //transform.rotation = GetComponent<Camera>().transform.rotation;
        //transform.position = target.position + offset;
    }
}
