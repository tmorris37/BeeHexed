using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    //[SerializeField] private Camera camera;
    //[SerializeField] private Transform target;
    //[SerializeField] private Vector3 offset;

    void Start()
    {
        //camera = camera.main;
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    // Update is called once per frame
    /*void Update()
    {
        //transform.rotation = GetComponent<Camera>().transform.rotation;
        //transform.position = target.position + offset;
    }*/
}
