using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum RewardType
{
    Coin,
    Heart,
    Skin,
}

[System.Serializable]
public enum SkinType
{
    CoinSkin,
    PremiumSkin,
    RescuaSkin
}

[System.Serializable]
public enum SizeOfIcon
{
    Small, Medium, Big, None
}

[System.Serializable]
public class RewardData
{
    public RewardType rewardType;
    public SkinType skinType;
    public int number;
    public float rate;

    public SizeOfIcon sizeOfIcon;
    public void ClaimSkin()
    {
        if (rewardType == RewardType.Skin)
        {
            ShopManager.Instance.UnlockSkin(number);
            ShopManager.Instance.SetCurrentSkinSelect(number);
        }
    }

    public void Claim()
    {
        if (rewardType == RewardType.Coin)
        {
            GameplayManager.Instance.AddCoin(number);
        }
        else if (rewardType == RewardType.Heart)
        {
            RxManager.TextLiveBonus.OnNext(number);
        }
    }

    public void Claimx2()
    {
        if (rewardType == RewardType.Coin)
        {
            GameplayManager.Instance.AddCoin(number * 2);
        }
        else if (rewardType == RewardType.Heart)
        {
            RxManager.TextLiveBonus.OnNext(number * 2);
        }
    }
}
