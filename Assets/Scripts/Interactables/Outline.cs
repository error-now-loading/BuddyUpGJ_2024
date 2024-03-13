using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    [SerializeField] private Material outlineMat;
    [SerializeField] private Material outlineHoverMat;

    private Material defaultMat;
    private SpriteRenderer spriteRenderer;

    private bool hover = false;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
        UpdateMat();
    }
    private void OnDisable()
    {
        spriteRenderer.material = defaultMat;
    }
    public void SetHover(bool hover)
    {
        this.hover = hover;
        UpdateMat();
    }

    private void UpdateMat()
    {
        if (enabled)
        {
            spriteRenderer.material = hover ? outlineHoverMat : outlineMat;
        }
    }
}