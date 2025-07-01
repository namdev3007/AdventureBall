using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public string tagTrigger;
    public float jumpForce;

    public AudioClip playerJumpAudio;

    public Animator animator;

    private int idTrigger;

    private void Start()
    {
        idTrigger = Animator.StringToHash("Trigger");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagTrigger))
        {
            collision.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, jumpForce);
            animator.SetTrigger(idTrigger);
            RxManager.playAudioE.OnNext(playerJumpAudio);
        }
    }
}
