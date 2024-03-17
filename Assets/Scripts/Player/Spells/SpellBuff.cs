using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBuff : MonoBehaviour
{
    [SerializeField] private SpellTypes buffType;
    [SerializeField] private Material spellGlow;
    [SerializeField] private float spellDuration;

    private static SpellBuff activeInstance;

    void Start()
    {
        if (activeInstance != null)
        {
            Destroy(activeInstance.gameObject);
        }
        activeInstance = this;
        transform.position = Vector3.zero;
        MushroomMinion.SetActiveBuff(buffType);
        FindObjectOfType<PlayerGlow>().SetMat(spellGlow);
    }

    private void Update()
    {
        spellDuration -= Time.deltaTime;
        if (spellDuration < 0)
        {
            MushroomMinion.SetActiveBuff(SpellTypes.NullBuff);
            activeInstance = null;
            FindObjectOfType<PlayerGlow>().ResetMat();
            Destroy(gameObject);

        }
    }
}
