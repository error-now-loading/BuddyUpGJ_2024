using System;
using UnityEngine;

[DisallowMultipleComponent]
public class NutrientHandler : MonoBehaviour
{
    [SerializeField] private int maxNutrients = 3500;
    [SerializeField] private int _nutrients = 100;
    public int nutrients => _nutrients;
    public event Action<int> OnNutrientValueChange;

    public void AddNutrients(int argValue)
    {
        _nutrients += argValue;

        if (_nutrients >= maxNutrients)
        {
            _nutrients = maxNutrients;
        }
    }

    public bool SpendNutrients(int argValue)
    {
        if (argValue > _nutrients)
        {
            return false;
        }

        _nutrients -= argValue;
        return true;
    }

    public int GetAvailableSpace()
    {
        return maxNutrients - _nutrients;
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
    }
    public void PlayerNutrientRefillEvent()
    {
        OnNutrientValueChange?.Invoke(nutrients);
    }
}