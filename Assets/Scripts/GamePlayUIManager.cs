using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GamePlayUIManager : MonoBehaviour
{
    public Transform currentTextLiveInGamePlay { get { return liveTxt.transform; } }
    public Button pauseButton;
    public TextMeshProUGUI liveTxt;

    public Image health1;
    public Image health2;
    public Image health3;

    public GameplayManager gameplayManager;

    public AudioSource audioBG;
    public AudioClip audioDefault;
    private void Awake()
    {
        pauseButton.onClick.AddListener(delegate
        {
            OnClickPauseBtn();
        });
        RxManager.updateHeal.Subscribe(heal => UpdateHeal(heal));
        RxManager.updateLive.Subscribe(live => UpdateLive(live));
        RxManager.SetBGMusicInMap.Subscribe(music => ChangeAudioBG(music));
        RxManager.SetBGMusicDefaultInMap.Subscribe(_ => ChangeAudioBG(audioDefault));
    }

 
    public void ChangeAudioBG(AudioClip _audio)
    {
        audioBG.clip = _audio;
        if (audioBG)
        {
            audioBG.Play();
        }
    }
    private void UpdateLive(int live)
    {
        liveTxt.text = live.ToString();

    }
    private void UpdateHeal(int heal)
    {
        health3.gameObject.SetActive(heal >= 3);
        health2.gameObject.SetActive(heal >= 2);
        health1.gameObject.SetActive(heal >= 1);
    }


    void OnClickPauseBtn()
    {
        RxManager.onClickPauseBtn.OnNext(true);
    }
}
