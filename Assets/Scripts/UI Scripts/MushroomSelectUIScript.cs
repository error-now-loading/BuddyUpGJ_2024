using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class MushroomSelectUIScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> mushrooms = new List<GameObject>();

    [SerializeField]
    private GameObject mushroomSelectorIndicator;

    private int currentlySelectedMushroomIndex;

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            if (Input.mouseScrollDelta.y < 0)
            {
                currentlySelectedMushroomIndex--;
                if (currentlySelectedMushroomIndex < 0)
                {
                    currentlySelectedMushroomIndex = mushrooms.Count - 1;
                }
            }
            else
            {
                currentlySelectedMushroomIndex++;
                if (currentlySelectedMushroomIndex >= mushrooms.Count)
                {
                    currentlySelectedMushroomIndex = 0;
                }
            }
        }
        mushroomSelectorIndicator.transform.position = mushrooms[currentlySelectedMushroomIndex].transform.position;
    }
}
