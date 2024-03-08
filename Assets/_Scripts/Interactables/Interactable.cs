using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private bool cursorRange = true;
    [SerializeField] private Outline outliner;

    private bool interactable = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cursorRange && collision.GetComponent<CursorRangeTrigger>())
        {
            outliner.enabled = true;
            interactable = true;
        }
        else if (!cursorRange && collision.GetComponent<NearRangeTrigger>())
        {
            outliner.enabled = true;
            interactable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (cursorRange && collision.GetComponent<CursorRangeTrigger>())
        {
            outliner.enabled = false;
            interactable = false;
        }
        else if (!cursorRange && collision.GetComponent<NearRangeTrigger>())
        {
            outliner.enabled = false;
            interactable = false;
        }
    }
    private void OnMouseEnter()
    {
        if (cursorRange)
        {
            outliner.SetHover(true);
        }
    }
    private void OnMouseExit()
    {
        if (cursorRange)
        {
            outliner.SetHover(false);
        }
    }
    private void OnMouseDown()
    {
        if (cursorRange && interactable)
        {
            Debug.Log("Interacted, must inherit this class for behaviour when interacted");
        }
    }
    public void NearInteraction()
    {
        if (!cursorRange && interactable)
        {
            Debug.Log("Interacted near, must inherit this class for behaviour when interacted");
        }
    }
}