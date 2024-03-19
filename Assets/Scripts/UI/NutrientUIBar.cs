using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NutrientUIBar : MonoBehaviour
{
    private NutrientHandler nutrientPlayer;
    private Slider slider;
    private void Start()
    {
        slider = GetComponent<Slider>();
        nutrientPlayer = FindObjectOfType<PlayerController>().gameObject.GetComponent<NutrientHandler>();
    }
    private void Update()
    {
        slider.value = nutrientPlayer.GetSliderValue();
    }
}
