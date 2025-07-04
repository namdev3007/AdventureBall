using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class NewSkinPanel : MonoBehaviour
{

    public AudioClip enableAudio;

    private void OnEnable()
    {
        RxManager.playAudioE.OnNext(enableAudio);
    }

    public void OnclickBackButton()
    {
    }
}
