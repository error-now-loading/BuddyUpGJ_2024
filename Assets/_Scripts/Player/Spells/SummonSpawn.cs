using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSpawn : MonoBehaviour
{
    [SerializeField] float destroyTimer = 5f;
    [SerializeField] GameObject child;
    private void OnEnable()
    {
        child.transform.parent = null;
        StartCoroutine(DestroyTimer(destroyTimer));
    }
    IEnumerator DestroyTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject.transform.parent.gameObject);
    }
}
