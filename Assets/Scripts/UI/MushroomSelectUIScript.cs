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
    [SerializeField]
    private List<MushroomTypeSO> mushromSOs = new List<MushroomTypeSO>();
    [SerializeField]
    private Vector2 keyBindPosition;
    private List<MushroomTypeSO> activeMushroomIcons = new List<MushroomTypeSO>();
    private PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        //I'll uncommit this once I get rid of the test code below it
        /*if (Input.GetKeyDown("1") && activeMushroomIcons.Count >= 1)
        {
            playerController.SetSelectedMushroomType(activeMushroomIcons[0]);
        }
        if (Input.GetKeyDown("2") && activeMushroomIcons.Count >= 2)
        {
            playerController.SetSelectedMushroomType(activeMushroomIcons[1]);
        }
        if (Input.GetKeyDown("3") && activeMushroomIcons.Count >= 3)
        {
            playerController.SetSelectedMushroomType(activeMushroomIcons[2]);
        }
        if (Input.GetKeyDown("4") && activeMushroomIcons.Count >= 4)
        {
            playerController.SetSelectedMushroomType(activeMushroomIcons[3]);
        }*/

        // Code for testing updating input
        if (Input.GetKeyDown("1"))
        {
            UpdateTroopNumbersUI(0, 1);
        }
        if (Input.GetKeyDown("2"))
        {
            UpdateTroopNumbersUI(1, 1);
        }
        if (Input.GetKeyDown("3"))
        {
            UpdateTroopNumbersUI(2, 1);
        }
        if (Input.GetKeyDown("4"))
        {
            UpdateTroopNumbersUI(3, 1);
        }
        if (Input.GetKeyDown("5"))
        {
            UpdateTroopNumbersUI(0, 0);
        }
        if (Input.GetKeyDown("6"))
        {
            UpdateTroopNumbersUI(1, 0);
        }
        if (Input.GetKeyDown("7"))
        {
            UpdateTroopNumbersUI(2, 0);
        }
        if (Input.GetKeyDown("8"))
        {
            UpdateTroopNumbersUI(3, 0);
        }
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
        RearrangeKeyBinds();
    }
    
    private void RearrangeKeyBinds()
    {
        activeMushroomIcons.Clear();
        foreach (GameObject i in keyBinds)
        {
            i.SetActive(false);
        }
        int k = 0;
        foreach (GameObject i in keyBinds)
        {
            while (k < mushroomIcons.Count)
            {
                if (mushroomIcons[k].activeSelf)
                {
                    i.SetActive(true);
                    i.transform.SetParent(mushroomIcons[k].transform);
                    i.transform.localPosition = keyBindPosition;
                    activeMushroomIcons.Add(mushromSOs[k]);
                    k++;
                    break;
                }
                k++;
            }
            if (k > mushroomIcons.Count)
            {
                return;
            }
        }
    }
}
