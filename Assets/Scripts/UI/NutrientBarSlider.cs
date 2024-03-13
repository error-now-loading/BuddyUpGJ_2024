using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NutrientBarSlider : MonoBehaviour
{
    [SerializeField]
    private Slider nutrientBarSlider;

    public void SetNutrientBarSlider(int nutrientAmount)
    {
        nutrientBarSlider.value = nutrientAmount;
    }

    public void SetNutrientBarSlider(int nutrientAmount, int maxNutrients)
    {
        nutrientBarSlider.maxValue = maxNutrients;
        nutrientBarSlider.value = nutrientAmount;
    }
}
