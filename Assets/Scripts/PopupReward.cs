using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupReward : MonoBehaviour
{
    public GameObject imageCoin;
    public GameObject imageHeart;
    public GameObject imageSkin; 
    public GameObject ButtonX2Reward;
    public TextMeshProUGUI textNoThank;
    public GameObject buttonNoThanks;
    public TextMeshProUGUI textNumber;
    public GameObject yougotanewSkin;

    public DataFuture dataFuture;

    GameplayManager gameplayManager;
    private void Awake()
    {
        gameplayManager = GameplayManager.Instance;
    }

    public void ActivePoup(RewardData _rewardData)
    {
        this.gameObject.SetActive(true);
        ActiveFalseAll(); 
        StopCoroutine(WaitAndActiveNoThanks());
        StartCoroutine(WaitAndActiveNoThanks());

        if (_rewardData.rewardType == RewardType.Skin)
        {
            yougotanewSkin.SetActive(true);
            imageSkin.SetActive(true);

            ButtonX2Reward.SetActive(false);
            textNumber.gameObject.SetActive(false);

            textNoThank.text = "Continue";
        }
        else 
        {
            textNumber.gameObject.SetActive(true);
            ButtonX2Reward.SetActive(true);
            textNumber.text = "+" + _rewardData.number;
            textNoThank.text = "No, Thanks";

            if (_rewardData.rewardType == RewardType.Coin)
            {
                imageCoin.SetActive(true);
            }
            else if (_rewardData.rewardType == RewardType.Heart) 
            {
                imageHeart.SetActive(true);
            }
        }
    }

    IEnumerator WaitAndActiveNoThanks()
    {
        yield return new WaitForSeconds(2);
        buttonNoThanks.SetActive(true);
    }

    public void OnClickNoThanksButton()
    {
        this.gameObject.SetActive(false);
        dataFuture.Claim();
    }

    public void OnclickButtonX2Reward()
    {
        GameplayManager.Instance.ShowVideoReward(delegate
        {
            dataFuture.ClaimX2();
            this.gameObject.SetActive(false);
        });
    }

    void ActiveFalseAll()
    {
        yougotanewSkin.SetActive(false);
        imageCoin.SetActive(false);
        imageHeart.SetActive(false);
        imageSkin.SetActive(false);
        ButtonX2Reward.SetActive(false);
        buttonNoThanks.SetActive(false);
        textNumber.gameObject.SetActive(false);
    }
}