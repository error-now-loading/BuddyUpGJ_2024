using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private bool cursorRange = true;
    [SerializeField] private Outline outliner;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cursorRange && collision.GetComponent<CursorRangeTrigger>())
        {
            outliner.enabled = true;
        }
        else if (!cursorRange && collision.GetComponent<NearRangeTrigger>())
        {
            outliner.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (cursorRange && collision.GetComponent<CursorRangeTrigger>())
        {
            outliner.enabled = false;
        }
        else if (!cursorRange && collision.GetComponent<NearRangeTrigger>())
        {
            outliner.enabled = false;
        }
    }
}