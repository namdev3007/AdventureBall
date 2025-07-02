using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextLiveBonus : MonoBehaviour
{

    public TextMeshProUGUI textLiveBonus;

    Vector3 root;

    private void Awake()
    {
        root = transform.position;
    }

    public void SetTextLive(int _value)
    {
        textLiveBonus.text = "+" + _value.ToString();

        gameObject.SetActive(true);

        transform.position = root;
        transform.localScale = Vector3.zero;

        transform.DOScale(2, 1).SetEase(Ease.OutQuart).OnComplete(delegate
        {
            transform.DOScale(0.2f, 0.5f);
            transform.DOMove(LiveBar.liveBarCurrent.position, 0.5f).SetEase(Ease.OutQuad).OnComplete(delegate
            {
                GameplayManager.Instance.AddLive(_value);
                this.gameObject.SetActive(false);
            });
        });
    }
}
