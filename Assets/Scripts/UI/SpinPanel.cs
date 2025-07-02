using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SpinPanel : MonoBehaviour
{
    public DataFuture dataFuture;

    public TextMeshProUGUI textCoin;
    public TextMeshProUGUI textHeart;
    public Button SpinBtton;

    public TextMeshProUGUI textTime;
    public TextMeshProUGUI textSpin;

    // ?� x�a: public bool freeSpin;
    // ?� x�a: public Image adsImage;
    // ?� x�a: private const string LASTTIMESPINFREE = "LASTTIMESPINFREE";

    public GameObject panel;
    public AudioClip audioSpine;
    public Animator animator;

    private void OnEnable()
    {
        RefreshText();
        textTime.enabled = false;
        textSpin.text = "Spin";
    }


    public void OnclickSpinButton()
    {

        animator.enabled = false;
        dataFuture.ActionRotate();
        SpinBtton.interactable = false;
        panel.SetActive(true);
        RxManager.playAudioE.OnNext(audioSpine);
    }

    public void OnClickBackButton()
    {
        UIManager.Instance.ActiveSpinPanel(false);
        RxManager.openHomePanel.OnNext(true);
        UIManager.Instance.ShopBackToHome();
    }

    public void RefreshText()
    {
        SpinBtton.interactable = true;
        textCoin.text = GameplayManager.Instance.Money.ToString();
        textHeart.text = GameplayManager.Instance.Live.ToString();
    }

    public void SpinDone()
    {
        panel.SetActive(false);
        animator.enabled = true;
    }
}