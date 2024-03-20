using System;
using UnityEngine;

[DisallowMultipleComponent]
public class NutrientHandler : MonoBehaviour
{
    [SerializeField] private int _maxNutrients = 3500;
    [SerializeField] private bool player = false;
    [SerializeField] private float nutrientsRegenSpeed = 1;
    public int maxNutrients => _maxNutrients;
    [SerializeField] private int _nutrients = 100;
    public int nutrients => _nutrients;
    public event Action<int> OnNutrientValueChange;
    private float nutrientFloat;


    private void Update()
    {
        if (player && nutrients < 50)
        {
            if(nutrientFloat > 1)
            {
                nutrientFloat = 0;
                _nutrients++;
                OnNutrientValueChange?.Invoke(nutrients);
            }
            else
            {
                nutrientFloat += Time.deltaTime * nutrientsRegenSpeed;
            }
        }
    }
    public void AddNutrients(int argValue)
    {
        _nutrients += argValue;

        if (_nutrients >= _maxNutrients)
        {
            _nutrients = _maxNutrients;
        }

        OnNutrientValueChange?.Invoke(nutrients);
    }

    public bool SpendNutrients(int argValue)
    {
        if (argValue > _nutrients)
        {
            return false;
        }

        _nutrients -= argValue;
        OnNutrientValueChange?.Invoke(nutrients);
        return true;
    }

    public int GetAvailableSpace()
    {
        return _maxNutrients - _nutrients;
    }

    public void TransferNutrients(NutrientHandler other)
    {
        int availableSpace = other.GetAvailableSpace();
        if (_nutrients <= availableSpace)
        {
            other.AddNutrients(_nutrients);
            SpendNutrients(_nutrients);
        }

        else if (_nutrients > availableSpace)
        {
            other.AddNutrients(availableSpace);
            SpendNutrients(availableSpace);
        }

        OnNutrientValueChange?.Invoke(nutrients);
    }
    public void PlayerNutrientRefillEvent()
    {
        OnNutrientValueChange?.Invoke(nutrients);
    }
    public float GetSliderValue()
    {
        return (float)nutrients/maxNutrients;
    }
}