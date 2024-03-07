using UnityEngine;

[DisallowMultipleComponent]
public class NutrientHandler : MonoBehaviour
{
    [SerializeField] private int maxNutrients = 3500;
    [SerializeField] private int _nutrients = 100;
    public int nutrients => _nutrients;



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
}