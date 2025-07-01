using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopManager : SingletonDontDestroy<ShopManager>
{
    public const string KEYSKIN = "KEYSKIN";
    public const string KEYTRYSKIN = "KEYTRYSKIN";
    public const string CURENTSKIN = "CURENTSKIN";
    public const string TRYSKIN = "TRYSKIN";
    public const string COIN_PACK = "COIN_PACK";
    public const string UNLIMITED_LIVE = "UNLIMITED_LIVE";
    public const string SUBSCRIPTION = "SUBSCRIPTION";
    public const string SPECIAL_OFFER = "SPECIALOFFER";

    public const string productCoin = "Coin";
    public const string productLive = "Live";
    public const string productSubscription = "Subscription";
    public const string specialOffer = "SpecialOffer";

    public static int coinPack = 80000;
    public static int coinSubscription = 100000;
    public static int coinSpecialOffer = 100000;

    public const int SKIN_SPECIAL_OFFER = 13;
    public bool isRandomSkin = true;


    public bool SpecialOffer
    {
        get => PlayerPrefs.HasKey(SPECIAL_OFFER);
        set
        {
            if (value) PlayerPrefs.SetInt(SPECIAL_OFFER, 1);
            else PlayerPrefs.DeleteKey(SPECIAL_OFFER);
            PlayerPrefs.Save();
        }
    }

    public bool CoinPack
    {
        get => PlayerPrefs.HasKey(COIN_PACK);
        set
        {
            if (value) PlayerPrefs.SetInt(COIN_PACK, 1);
            else PlayerPrefs.DeleteKey(COIN_PACK);
            PlayerPrefs.Save();
        }
    }

    public bool Subscription
    {
        get => PlayerPrefs.HasKey(SUBSCRIPTION);
        set
        {
            if (value) PlayerPrefs.SetInt(SUBSCRIPTION, 1);
            else PlayerPrefs.DeleteKey(SUBSCRIPTION);
            PlayerPrefs.Save();
        }
    }

    public bool UnlimitedLive
    {
        get => PlayerPrefs.HasKey(UNLIMITED_LIVE);
        set
        {
            if (value) PlayerPrefs.SetInt(UNLIMITED_LIVE, 1);
            else PlayerPrefs.DeleteKey(UNLIMITED_LIVE);
            PlayerPrefs.Save();
        }
    }

    private void Start()
    {
      
    }

    void Ramdom1SkinPremium()
    {
        List<int> listIDSkinNotYetUnlock = new List<int>();

        foreach (var item in GameplayManager.Instance.skinData.premiumSkin)
        {
            if (!IsOwnSkin(item.skinID) && !item.isFree)
            {
                listIDSkinNotYetUnlock.Add(item.skinID);
            }
        }

        if (listIDSkinNotYetUnlock.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, listIDSkinNotYetUnlock.Count);
            UnlockSkin(listIDSkinNotYetUnlock[index]);
            SetCurrentSkinSelect(listIDSkinNotYetUnlock[index]);
        }
    }

    public bool IsOwnSkin(int skinId)
    {
        return skinId == 0 || PlayerPrefs.HasKey(KEYSKIN + skinId);
    }

    public int GetSkinIdCurrent()
    {
        int skinId = GetCurrentSkinTry();
        if (skinId < 0) skinId = GetCurrentSkinSelect();
        return skinId;
    }

    public bool IsSelectedSkin(int skinId)
    {
        return GetCurrentSkinSelect() == skinId;
    }

    public bool IsTrySkin(int skinId)
    {
        return PlayerPrefs.HasKey(KEYTRYSKIN + skinId);
    }

    public bool IsOwnOrTrySkin(int skinId)
    {
        return IsOwnSkin(skinId) || IsTrySkin(skinId);
    }

    public void OnClickTrySkin(int skinId)
    {
        PlayerPrefs.SetInt(KEYTRYSKIN + skinId, 1);
        SetCurrentSkinSelect(skinId);
    }

    public void PurchaseSkin(int skinId)
    {
        UnlockSkin(skinId);
        SetCurrentSkinSelect(skinId);
    }

    public void UnlockSkin(int skinId)
    {
        PlayerPrefs.SetInt(KEYSKIN + skinId, 1);
        SetCurrentSkinSelect(skinId);
        RxManager.NewSkinPanel.OnNext(skinId);
    }

    public int GetCurrentSkinSelect()
    {
        int currentSkin = GetCurrentSkinTry();
        if (currentSkin < 0) currentSkin = PlayerPrefs.GetInt(CURENTSKIN, 0);
        return currentSkin;
    }

    public void SetCurrentSkinSelect(int skinId)
    {
        if (IsTrySkin(skinId))
        {
            PlayerPrefs.SetInt(TRYSKIN, skinId);
            RxManager.selectSkinId.OnNext(skinId);
        }
        else if (IsOwnSkin(skinId))
        {
            RemoveTrySkin();
            PlayerPrefs.SetInt(CURENTSKIN, skinId);
            RxManager.selectSkinId.OnNext(skinId);
        }
        isRandomSkin = false;
    }

    public int GetCurrentSkinTry()
    {
        return PlayerPrefs.GetInt(TRYSKIN, -1);
    }

    public void CheckFinishTrySkin()
    {
        int currentSkin = GetCurrentSkinTry();
        if (currentSkin > 0)
        {
            PlayerPrefs.DeleteKey(KEYTRYSKIN + currentSkin);
            RemoveTrySkin();
        }
    }

    public void PushNotiSelectCurrentSkin()
    {
        RxManager.selectSkinId.OnNext(GetSkinIdCurrent());
    }

    public void RemoveTrySkin()
    {
        PlayerPrefs.DeleteKey(TRYSKIN);
    }

    public bool IsUnlockAllSkin()
    {
        foreach (var item in GameplayManager.Instance.skinData.coinSkin)
        {
            if (!IsOwnSkin(item.skinID)) return false;
        }

        foreach (var item in GameplayManager.Instance.skinData.premiumSkin)
        {
            if (!IsOwnSkin(item.skinID)) return false;
        }

        foreach (var item in GameplayManager.Instance.skinData.rescueSkin)
        {
            if (!IsOwnSkin(item.skinID)) return false;
        }

        return true;
    }

    void SuccessSpecialOffer()
    {
        SpecialOffer = true;
        GameplayManager.Instance.AddCoin(coinSpecialOffer);
        UnlockSkin(SKIN_SPECIAL_OFFER);
        SetCurrentSkinSelect(SKIN_SPECIAL_OFFER);
    }
}
