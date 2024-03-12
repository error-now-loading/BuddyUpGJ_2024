using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class MushroomSelectUIScript : MonoBehaviour
{
    //Proabably need to change to Image objects later
    [SerializeField]
    private List<RawImage> mushrooms = new List<RawImage>();

    [SerializeField]
    private RawImage mushroomSelectorIndicator;

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
                currentlySelectedMushroomIndex = (currentlySelectedMushroomIndex + 1) % mushrooms.Count;
            }
        }
        mushroomSelectorIndicator.transform.position = mushrooms[currentlySelectedMushroomIndex].transform.position;
    }
}
