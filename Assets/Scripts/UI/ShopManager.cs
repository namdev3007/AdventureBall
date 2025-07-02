using System;
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

    public const string productCoin = "Coin";
    public const string productLive = "Live";
    public const string productSubscription = "Subscription";

    public static int coinPack = 80000;
    public static int coinSubscription = 100000;

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

    public bool isRandomSkin = true;

    private void Start()
    {
        if (GameplayManager.IAP)
        {
            Purchaser.Instance.AddProductConsume(productCoin, success =>
            {
                if (success)
                {
                    GameplayManager.Instance.AddCoin(coinPack);
                }
            });

            Purchaser.Instance.AddProductNonConsume(productLive, success =>
            {
                if (success && !UnlimitedLive)
                {
                    UnlimitedLive = true;
                }
            });

            Purchaser.Instance.AddProductSubscription(productSubscription, success =>
            {
                if (success && !Subscription)
                {
                    GameplayManager.Instance.AddCoin(coinSubscription);
                    foreach (var item in GameplayManager.Instance.skinData.premiumSkin)
                        UnlockSkin(item.skinID);
                    Subscription = true;
                }
            });

            Purchaser.Instance.Init();
        }
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

    public bool IsSubscribed()
    {
        return Subscription;
    }

    public void PurchaseCoin(UnityAction<bool> callback = null)
    {
        Purchaser.Instance.BuyProductID(productCoin, callback);
    }

    public void PurchaseLive(UnityAction<bool> callback = null)
    {
        Purchaser.Instance.BuyProductID(productLive, callback);
    }

    public void PurchaseSubscription(UnityAction<bool> callback = null)
    {
        if (Subscription) callback = null;
        Purchaser.Instance.BuyProductID(productSubscription, callback);
    }

    public string GetPrice(string product)
    {
        return Purchaser.Instance.GetLocalizedPriceString(product);
    }

    public string GetPriceSaleOff(string product, int off = 2)
    {
        return "1.99$";
    }

    public Sprite GetSpriteBySkinId(int skinId)
    {
        SkinData skinData = GameplayManager.Instance.skinData;
        if (skinData == null) return null;

        foreach (var skin in skinData.premiumSkin)
        {
            if (skin.skinID == skinId)
            {
                return skin.skinSprite;
            }
        }

        foreach (var skin in skinData.rescueSkin)
        {
            if (skin.skinID == skinId)
            {
                return skin.skinSprite;
            }
        }

        foreach (var skin in skinData.coinSkin)
        {
            if (skin.skinID == skinId)
            {
                return skin.skinSprite;
            }
        }

        Debug.LogWarning($"Sprite for Skin ID {skinId} not found in SkinData.");
        return null;
    }


    public bool IsOwnSkin(int skinId)
    {
        return skinId == 0 || PlayerPrefs.HasKey(KEYSKIN + skinId);
    }

    public int GetSkinIdCurrent()
    {
        int skinId = GetCurrentSkinTry();
        if (skinId < 0)
            skinId = GetCurrentSkinSelect();
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
    }

    public int GetCurrentSkinSelect()
    {
        int currentSkin = GetCurrentSkinTry();
        if (currentSkin < 0)
            currentSkin = PlayerPrefs.GetInt(CURENTSKIN, 0);
        return currentSkin;
    }

    public void SetCurrentSkinSelect(int skinId)
    {
        if (IsTrySkin(skinId))
            PlayerPrefs.SetInt(TRYSKIN, skinId);
        else if (IsOwnSkin(skinId))
        {
            RemoveTrySkin();
            PlayerPrefs.SetInt(CURENTSKIN, skinId);
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

    public void PushNotiSelectCurrentSkin() { }

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
}