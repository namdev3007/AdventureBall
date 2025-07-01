using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skin
{
    public int skinID;
    public int levelUnlock;
}

[System.Serializable]
public class PremiumSkin : Skin
{
    public int coin = 30000;
    public bool isFree;
}

[System.Serializable]
public class RescueSkin : Skin
{
    /*public int levelUnlock;*/
}

[System.Serializable]
public class CoinSkin : Skin
{
}

[CreateAssetMenu(fileName = "SkinData", menuName = "ScriptableObject/SkinData", order = 1)]
public class SkinData : ScriptableObject
{
    public List<PremiumSkin> premiumSkin;
    public List<RescueSkin> rescueSkin;
    public List<CoinSkin> coinSkin;


    public Skin GetSkinUnlockByLevel(int level)
    {
        Skin skin = null;
        for (int i = 0; i < rescueSkin.Count; i++)
        {
            if (rescueSkin[i].levelUnlock == level)
            {
                if (!ShopManager.Instance.IsOwnSkin(rescueSkin[i].skinID))
                {
                    skin = rescueSkin[i];
                    break;
                }
            }
        }

        if (skin == null)
        {
            for (int i = 0; i < premiumSkin.Count; i++)
            {
                if (premiumSkin[i].levelUnlock == level)
                {
                    if (!ShopManager.Instance.IsOwnSkin(premiumSkin[i].skinID))
                    {
                        skin = premiumSkin[i];
                        break;
                    }
                }
            }
        }

        if (skin == null)
        {
            for (int i = 0; i < coinSkin.Count; i++)
            {
                if (coinSkin[i].levelUnlock == level)
                {
                    if (!ShopManager.Instance.IsOwnSkin(coinSkin[i].skinID))
                    {
                        skin = coinSkin[i];
                        break;
                    }
                }
            }
        }
        return skin;
    }

    public void RemoveSkinSpecial(int skinId)
    {
        for (int i = 0; i < premiumSkin.Count; i++)
        {
            if (premiumSkin[i].skinID == skinId)
            {
                premiumSkin.RemoveAt(i);
                return;
            }
        }

        for (int i = 0; i < rescueSkin.Count; i++)
        {
            if (rescueSkin[i].skinID == skinId)
            {
                rescueSkin.RemoveAt(i);
                return;
            }
        }

        for (int i = 0; i < coinSkin.Count; i++)
        {
            if (coinSkin[i].skinID == skinId)
            {
                coinSkin.RemoveAt(i);
                return;
            }
        }
    }
}
