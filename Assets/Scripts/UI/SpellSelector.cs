using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpellSelector : MonoBehaviour
{
    [SerializeField] private float spinnyWeeTimer = 0.2f;
    [SerializeField] private List<SpellTypeSO> spellTypes;
    [SerializeField] private Image selectedSpell;
    [SerializeField] private Image next1Spell;
    [SerializeField] private Image next2Spell;
    [SerializeField] private Image prev1Spell;
    [SerializeField] private Image prev2Spell;
    private int selectedIndex = 0;
    private Animator animator;
    private PlayerInput playerInput;
    private PlayerController playerController;
    private bool isInCooldown;
    private int next1Index;
    private int next2Index;
    private int prev1Index;
    private int prev2Index;

    private void OnEnable()
    {
        playerInput = new PlayerInput();
        animator = selectedSpell.GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
        playerInput.PlayerOverworld.Enable();
        playerInput.PlayerOverworld.SpellChangeScroll.performed += SpellChangeScroll_performed;
    }

    private void SpellChangeScroll_performed(InputAction.CallbackContext obj)
    {
        if (!isInCooldown)
        {
            isInCooldown = true;
            animator.SetTrigger("SpinnyWee");
            StartCoroutine(SpinnyWee());

            if (obj.ReadValue<float>() > 0)
            {
                selectedIndex++;
                if (selectedIndex > spellTypes.Count - 1)
                {
                    selectedIndex = 0;
                }
                animator.SetFloat("Speed",1);
            }
            else
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = spellTypes.Count - 1;
                }
                animator.SetFloat("Speed", -1);
            }
            playerController.SetSelectedSpellType(spellTypes[selectedIndex]);
        }
    }

    IEnumerator SpinnyWee()
    {
        yield return new WaitForSeconds(spinnyWeeTimer/2);

        next1Index = (selectedIndex + 1) % spellTypes.Count;
        next2Index = (selectedIndex + 2) % spellTypes.Count;
        prev1Index = (selectedIndex - 1 + spellTypes.Count) % spellTypes.Count;
        prev2Index = (selectedIndex - 2 + spellTypes.Count) % spellTypes.Count;

        selectedSpell.sprite = spellTypes[selectedIndex].icon;
        next1Spell.sprite = spellTypes[next1Index].icon;
        next2Spell.sprite = spellTypes[next2Index].icon;
        prev1Spell.sprite = spellTypes[prev1Index].icon;
        prev2Spell.sprite = spellTypes[prev2Index].icon;

        yield return new WaitForSeconds(spinnyWeeTimer/2);

        isInCooldown = false;
    }

    private void OnDisable()
    {
        playerInput.PlayerOverworld.SpellChangeScroll.performed -= SpellChangeScroll_performed;
        playerInput.Disable();
    }
}
