using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxTrigger : MonoBehaviour
{
    public string tagTrigger = "Player";
    public UnityEvent trigerEnter;
    public UnityEvent trigerExit;

    private bool delayYet;
    public float timeDelayFirstHit;

    private void OnEnable()
    {
        delayYet = false;
    }

    IEnumerator DeLayFirstHit()
    {
        yield return new WaitForSeconds(timeDelayFirstHit);
        trigerEnter.Invoke();
        delayYet = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (trigerEnter != null && collision.gameObject.CompareTag(tagTrigger))
        {
            delayYet = false;

            if (delayYet)
            {
                trigerEnter.Invoke();
            }
            else
            {
                StartCoroutine(DeLayFirstHit());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (trigerExit != null && collision.gameObject.CompareTag(tagTrigger))
        {
            trigerExit.Invoke();
        }
    }
}
    