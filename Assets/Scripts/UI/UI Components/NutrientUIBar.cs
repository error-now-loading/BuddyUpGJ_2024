using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NutrientUIBar : MonoBehaviour
{
    [SerializeField] private Slider slider = null;
    [SerializeField] private TMP_Text nutrientText = null;
    private NutrientHandler nutrientPlayer;



    private void Start()
    {
        nutrientPlayer = FindObjectOfType<PlayerController>().gameObject.GetComponent<NutrientHandler>();
        nutrientPlayer.OnNutrientValueChange += NutrientPlayer_OnNutrientValueChange;
        nutrientText.text = $"{nutrientPlayer.nutrients} / {nutrientPlayer.maxNutrients}";
    }

    private void NutrientPlayer_OnNutrientValueChange(int obj)
    {
        slider.value = nutrientPlayer.GetSliderValue();
        nutrientText.text = $"{nutrientPlayer.nutrients} / {nutrientPlayer.maxNutrients}";
    }
}
