using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointControl : MonoBehaviour
{
    public Transform pointSpawn;
    public TriggerPreloadCheckPoint triggerPreloadCheckPoint;
    public Animator animator;
    private int id;
    public AudioClip checkPoint;

    private Collider2D cl;
    public GameObject redLight;
    private void Start()
    {
        triggerPreloadCheckPoint.checkPoint = this;
        id = Animator.StringToHash("CheckBool");
        animator.SetBool(id, false);
        if (pointSpawn == null)
        {
            pointSpawn = transform;
        }

        cl = transform.gameObject.GetComponent<BoxCollider2D>();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            setTrueOrFalse(false);
            RxManager.saveCheckpoint.OnNext(this);
            RxManager.playAudioE.OnNext(checkPoint);
            if (redLight != null)
            {
                redLight.SetActive(false);

            }
        }
    }

    public void setTrueOrFalse(bool _x)
    {
        cl.enabled = _x;
        animator.SetBool(id, !_x);
    }

    public void ResetCheckPoint()
    {
        setTrueOrFalse(true);
        triggerPreloadCheckPoint.gameObject.SetActive(true);
    }
}
