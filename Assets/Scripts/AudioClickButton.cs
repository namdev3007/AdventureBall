using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioClickButton : MonoBehaviour
{

    private Button thisButton;

    private void Awake()
    {
        thisButton = GetComponent<Button>();

        if (thisButton)
        {
            thisButton.onClick.AddListener(delegate { OnNextAudioClick(); });
        }
    }

    public void OnNextAudioClick()
    {
        RxManager.audioClick.OnNext(true);
    }
}
