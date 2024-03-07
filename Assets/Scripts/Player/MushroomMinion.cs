using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMinion : MonoBehaviour
{
    [SerializeField] private MushroomTypeSO mushroomType;
    private bool standing = true;
    void Update()
    {
        if (!standing)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {

    }
}