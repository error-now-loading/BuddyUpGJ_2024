using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Base class for sliders that have text elements attached to them
/// </summary>
public class TextSlider : MonoBehaviour
{
    [SerializeField] protected TMP_Text label = null;
    [SerializeField] protected Slider slider = null;

    protected Action<float> OnSliderValueChangedAction = null;



    protected void Awake()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void AddListener(Action<float> listener)
    {
        OnSliderValueChangedAction += listener;
    }

    public void SetSliderValue(float argValue)
    {
        if (argValue <= slider.maxValue && argValue >= slider.minValue)
        {
            slider.value = argValue;
        }

        else
        {
            Debug.Log($"[TextSlider '{gameObject.name}' Error]: SetSliderValue argument '{argValue}' is not within the range [{slider.minValue}, {slider.maxValue}]");
        }
    }

    protected void OnSliderValueChanged(float argValue)
    {
        OnSliderValueChangedAction?.Invoke(argValue);
    }

    protected void OnDestroy()
    {
        slider.onValueChanged.RemoveAllListeners();
    }
}