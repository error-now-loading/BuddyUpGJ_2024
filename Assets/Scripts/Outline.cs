using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    [SerializeField] private Material outlineMat;

    private Material defaultMat;
    private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
        spriteRenderer.material = outlineMat;
    }
    private void OnDisable()
    {
        spriteRenderer.material = defaultMat;
    }
}