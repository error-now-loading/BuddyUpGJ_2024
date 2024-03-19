using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpellSelector : MonoBehaviour
{
    [SerializeField] private List<SpellTypeSO> spellTypes;
    [SerializeField] private Image selectedSpell;
    [SerializeField] private Image next1Spell;
    [SerializeField] private Image next2Spell;
    [SerializeField] private Image prev1Spell;
    [SerializeField] private Image prev2Spell;
    private int selectedIndex = 0;
    private PlayerInput playerInput;
    private PlayerController playerController;
    private bool isInCooldown;
    private float cooldownTimer = 0.1f;

    private void OnEnable()
    {
        playerInput = new PlayerInput();
        playerController = FindObjectOfType<PlayerController>();
        playerInput.PlayerOverworld.Enable();
        playerInput.PlayerOverworld.SpellChangeScroll.performed += SpellChangeScroll_performed;
    }

    private void SpellChangeScroll_performed(InputAction.CallbackContext obj)
    {
        if (!isInCooldown)
        {
            isInCooldown = true;
            Invoke("CooldownEnd", cooldownTimer);

            if (obj.ReadValue<float>() > 0)
            {
                selectedIndex++;
                if (selectedIndex > spellTypes.Count - 1)
                {
                    selectedIndex = 0;
                }
            }
            else
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = spellTypes.Count - 1;
                }
            }

            int next1Index = (selectedIndex + 1) % spellTypes.Count;
            int next2Index = (selectedIndex + 2) % spellTypes.Count;
            int prev1Index = (selectedIndex - 1 + spellTypes.Count) % spellTypes.Count;
            int prev2Index = (selectedIndex - 2 + spellTypes.Count) % spellTypes.Count;

            selectedSpell.sprite = spellTypes[selectedIndex].icon;
            playerController.SetSelectedSpellType(spellTypes[selectedIndex]);
            next1Spell.sprite = spellTypes[next1Index].icon;
            next2Spell.sprite = spellTypes[next2Index].icon;
            prev1Spell.sprite = spellTypes[prev1Index].icon;
            prev2Spell.sprite = spellTypes[prev2Index].icon;
        }

    }
    private void CooldownEnd()
    {
        isInCooldown = false;
    }

    private void OnDisable()
    {
        playerInput.PlayerOverworld.SpellChangeScroll.performed -= SpellChangeScroll_performed;
        playerInput.Disable();
    }
}
