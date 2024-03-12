using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellSelectUI : MonoBehaviour
{
    //Proabably need to change to Image objects later
    [SerializeField]
    private List<RawImage> spellIcons = new List<RawImage>();

    [SerializeField]
    private RawImage spellSelectorIndicator;

    private void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            spellSelectorIndicator.transform.position = spellIcons[0].transform.position;
        }
        else if (Input.GetKeyDown("2"))
        {
            spellSelectorIndicator.transform.position = spellIcons[1].transform.position;
        }
        else if (Input.GetKeyDown("3"))
        {
            spellSelectorIndicator.transform.position = spellIcons[2].transform.position;
        }
    }
}
