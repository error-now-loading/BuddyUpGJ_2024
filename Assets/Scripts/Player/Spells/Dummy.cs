using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField] private float spellDuration;
    private static Dummy activeInstance;
    void Start()
    {
        if (activeInstance != null)
        {
            Destroy(activeInstance.gameObject);
        }
        activeInstance = this;
    }

    private void Update()
    {
        spellDuration -= Time.deltaTime;
        if (spellDuration < 0)
        {
            activeInstance = null;
            Destroy(gameObject);
        }
    }
}