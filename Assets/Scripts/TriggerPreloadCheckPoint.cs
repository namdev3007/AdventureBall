using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPreloadCheckPoint : MonoBehaviour
{
    [HideInInspector]
    public CheckpointControl checkPoint;
    bool isTrigger = false;

    private void OnEnable()
    {
        isTrigger = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTrigger && other.CompareTag("Player"))
        {
            isTrigger = true;
            RxManager.preloadCheckpoint.OnNext(checkPoint);
            gameObject.SetActive(false);
        }
    }
}
