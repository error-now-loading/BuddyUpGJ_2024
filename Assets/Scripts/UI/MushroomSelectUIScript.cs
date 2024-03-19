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
    private int selectedIndex;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerController.onTroopUpdate += PlayerController_onTroopUpdate;
    }

    private void PlayerController_onTroopUpdate()
    {
        bool emptyTroop = false;
        if (activeMushroomIcons.Count == 0)
        {
            emptyTroop = true;
        }
        for (int i = 0; i < mushromSOs.Count; i++)
        {
            UpdateTroopNumbersUI(i, playerController.GetMinionTypeCount(i));
        }
        if (emptyTroop)
        {
            if (activeMushroomIcons.Count > 0)
            {
                playerController.SetSelectedMushroomType(activeMushroomIcons[0]);
            }
            else
            {
                playerController.SetSelectedMushroomType(mushromSOs[0]); //"Select" Troopy when no one is in the troop, doesnt really matter.
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("1") && activeMushroomIcons.Count >= 1)
        {
            playerController.SetSelectedMushroomType(activeMushroomIcons[0]);
            selectedIndex = 0;
        }
        if (Input.GetKeyDown("2") && activeMushroomIcons.Count >= 2)
        {
            playerController.SetSelectedMushroomType(activeMushroomIcons[1]);
            selectedIndex = 1;
        }
        if (Input.GetKeyDown("3") && activeMushroomIcons.Count >= 3)
        {
            playerController.SetSelectedMushroomType(activeMushroomIcons[2]);
            selectedIndex = 2;
        }
        if (Input.GetKeyDown("4") && activeMushroomIcons.Count >= 4)
        {
            playerController.SetSelectedMushroomType(activeMushroomIcons[3]);
            selectedIndex = 3;
        }
    }

    public void UpdateTroopNumbersUI(int mushroomIndex, int mushroomCount)
    {
        mushroomIcons[mushroomIndex].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().SetText(mushroomCount.ToString());
        if (mushroomCount > 0)
        {
            mushroomIcons[mushroomIndex].SetActive(true);
            RearrangeKeyBinds();
        }
        else
        {
            mushroomIcons[mushroomIndex].SetActive(false);
            RearrangeKeyBinds();
            if (activeMushroomIcons.Count > selectedIndex) {
                playerController.SetSelectedMushroomType(activeMushroomIcons[selectedIndex]);
            }
            else if (activeMushroomIcons.Count > 0)
            {
                playerController.SetSelectedMushroomType(activeMushroomIcons[activeMushroomIcons.Count-1]);
            }
        }
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
