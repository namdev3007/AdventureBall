using UniRx;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource jumpAudio;
    public AudioSource onGroundAudio;
    public AudioSource hitAudio;
    public AudioSource fallInWatter;
    public AudioSource playerDie;

    private void Start()
    {
        if (hitAudio != null)
        {
            RxManager.enemyAttackPlayer.Subscribe(_ => hitAudio.Play());
            RxManager.enemyAttackPlayerMore.Subscribe(_ => hitAudio.Play());
        }
        if (jumpAudio != null)
        {
            RxManager.playerJumping.Subscribe(_ => jumpAudio.Play());
        }
        if (onGroundAudio != null)
        {
            RxManager.playerOnGround.Subscribe(jump => {
                onGroundAudio.volume = jump * 0.4f;
                onGroundAudio.Play();
            });
        }
        if (fallInWatter != null)
        {
            RxManager.playerFallInWatter.Subscribe(_ => fallInWatter.Play());
        }
        if (playerDie != null)
        {
            RxManager.playerDie.Subscribe(_ => playerDie.Play());
        }
    }
}
