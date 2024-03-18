using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlow : MonoBehaviour
{
    [SerializeField] private Material whiteGlow;
    private SpriteRenderer sprite;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    public void SetMat(Material mat)
    {
        sprite.material = mat;
    }
    public void ResetMat()
    {
        sprite.material = whiteGlow;
    }
}
