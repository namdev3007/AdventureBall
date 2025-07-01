using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivePhysic : MonoBehaviour
{
    public string tagTrigger = "Player";
    public Rigidbody2D[] rigidbodys;
    public MonoBehaviour[] activeComponents;
    public GameObject[] objects;

    public AudioSource audioActive;
    private void OnEnable()
    {
        Active(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagTrigger))
        {
            Active(true);
            if (audioActive)
            {
                audioActive.mute = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagTrigger))
        {
            Active(false);
            if (audioActive)
            {
                audioActive.mute = true;
            }
        }
    }

    private void Active(bool isActive)
    {
        for (int i = 0; i < rigidbodys.Length; i++)
        {
            rigidbodys[i].simulated = isActive;
        }
        for (int i = 0; i < activeComponents.Length; i++)
        {
            activeComponents[i].enabled = isActive;
        }
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(isActive);
        }
    }
}
