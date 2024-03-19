using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinionCount : MonoBehaviour
{
    private TextMeshProUGUI m_TextMeshProUGUI;
    void Start()
    {
        MushroomMinion.onMinionCountChange += MushroomMinion_onMinionCountChange;
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void MushroomMinion_onMinionCountChange()
    {
        m_TextMeshProUGUI.text = MushroomMinion.minionCount.ToString();
    }
}
