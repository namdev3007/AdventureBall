using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataChild : MonoBehaviour
{
    public Image skinImage;
    public GameObject iconRewardCoin;
    public GameObject iconRewardCoin1;
    public GameObject iconRewardCoin2;
    public GameObject iconRewardHearth;
    public GameObject iconRewardHearth1;
    public GameObject iconRewardHearth2;
    public TextMeshProUGUI text;

    public void SetData(RewardData _rewardData)
    {
        skinImage.gameObject.SetActive(false);
        iconRewardCoin.SetActive(false);
        iconRewardCoin1.SetActive(false);
        iconRewardCoin2.SetActive(false);
        iconRewardHearth.SetActive(false);
        iconRewardHearth1.SetActive(false);
        iconRewardHearth2.SetActive(false);
        text.text = "";

        if (_rewardData.rewardType == RewardType.Coin)
        {
            this.text.text = "+" + _rewardData.number;
            if (_rewardData.sizeOfIcon == SizeOfIcon.Small) this.iconRewardCoin.SetActive(true);
            else if (_rewardData.sizeOfIcon == SizeOfIcon.Medium) this.iconRewardCoin1.SetActive(true);
            else if (_rewardData.sizeOfIcon == SizeOfIcon.Big) this.iconRewardCoin2.SetActive(true);
        }
        else if (_rewardData.rewardType == RewardType.Heart)
        {
            this.text.text = "+" + _rewardData.number;
            if (_rewardData.sizeOfIcon == SizeOfIcon.Small) this.iconRewardHearth.SetActive(true);
            else if (_rewardData.sizeOfIcon == SizeOfIcon.Medium) this.iconRewardHearth1.SetActive(true);
            else if (_rewardData.sizeOfIcon == SizeOfIcon.Big) this.iconRewardHearth2.SetActive(true);
        }
        else if (_rewardData.rewardType == RewardType.Skin)
        {
            this.text.text = "SKIN";
            this.skinImage.gameObject.SetActive(true);
            if (ShopManager.Instance != null)
            {
                this.skinImage.sprite = ShopManager.Instance.GetSpriteBySkinId(_rewardData.number);
            }
        }
    }
}