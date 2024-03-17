using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MushroomSelectUIScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> mushroomIcons = new List<GameObject>();
    [SerializeField]
    private List<GameObject> keyBinds = new List<GameObject>();

    private void Update()
    {
        // Code for testing updatingg input
        /*if (Input.GetKeyDown("4"))
        {
            UpdateTroopNumbersUI(0, 1);
        }
        if (Input.GetKeyDown("5"))
        {
            UpdateTroopNumbersUI(1, 1);
        }
        if (Input.GetKeyDown("6"))
        {
            UpdateTroopNumbersUI(2, 1);
        }
        if (Input.GetKeyDown("7"))
        {
            UpdateTroopNumbersUI(0, 0);
        }
        if (Input.GetKeyDown("8"))
        {
            UpdateTroopNumbersUI(1, 0);
        }
        if (Input.GetKeyDown("9"))
        {
            UpdateTroopNumbersUI(2, 0);
        }*/
    }

    public void UpdateTroopNumbersUI(int mushroomIndex, int mushroomCount)
    {
        mushroomIcons[mushroomIndex].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().SetText(mushroomCount.ToString());
        if (mushroomCount > 0)
        {
            mushroomIcons[mushroomIndex].SetActive(true);
        }
        else
        {
            mushroomIcons[mushroomIndex].SetActive(false);
        }
    }
}
