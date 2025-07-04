using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeSpawnByTime : MonoBehaviour
{
    public float time;
    void OnEnable()
    {
        StartCoroutine(DeSpwan());
    }
    private IEnumerator DeSpwan()
    {
        yield return new WaitForSeconds(time);
        SmartPool.Instance.Despawn(gameObject);
    }
}
