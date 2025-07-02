using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextCoinVictory : MonoBehaviour
{
    public TextMeshProUGUI textCoin;

    public AudioSource audioCoinWin;
    public AudioClip audioCoin;

    Vector3 root;

    private void Awake()
    {
        root = transform.position;
    }

    public void SetTextCoin(int valua, System.Action success)
    {
        textCoin.text = "+" + valua;

        gameObject.SetActive(true);

        transform.position = root;
        transform.localScale = Vector3.zero;

        transform.DOScale(1.5f, 1).SetEase(Ease.OutQuart).OnComplete(delegate
        {
            transform.DOScale(0.2f, 0.5f);
            transform.DOMove(MoneyBar.currentMoneyBar.position, 0.5f).SetEase(Ease.OutQuad).OnComplete(delegate {
                audioCoinWin.PlayOneShot(audioCoin);
                this.gameObject.SetActive(false);

                if (success != null)
                {
                    success();
                }
            });
        });
    }
}
