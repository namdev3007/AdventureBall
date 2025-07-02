using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListGiftDataLocal
{
    public GiftData[] listGiftDataLocal;
    public byte countVideo;
}
public class ListGift : MonoBehaviour
{
    private const string KEYGIFTDATA = "GIFTDATA";

    public ListGiftDataLocal listGiftData;

    private Gift[] listGif;

    private const string KEYLASTGIFTREFRESH = "KEYLASTGIFTREFRESH";

    private DateTime nextDay;
    private void OnEnable()
    {
        listGif = gameObject.GetComponentsInChildren<Gift>();
        CreateKey();
    }
    void CreateKey()
    {
        if (!PlayerPrefs.HasKey(KEYGIFTDATA))
        {
            CheckTime();
            CreateNewData();
        }
        else
        {
            CheckTime();
        }
        SetViewGift();
    }
    void CreateNewData()
    {
        List<int> listIDSkinUnlock = new List<int>();

        List<int> listIDSkinPremium = new List<int>();
        if (!ShopManager.Instance.IsSubscribed())
        {
            foreach (var item in GameplayManager.Instance.skinData.premiumSkin)
            {
                if (!ShopManager.Instance.IsOwnSkin(item.skinID) && !item.isFree)
                {
                    listIDSkinPremium.Add(item.skinID);
                }
                else
                {
                    listIDSkinUnlock.Add(item.skinID);
                }
            }
        }
        List<int> listIDSkinCoin = new List<int>();
        foreach (var item in GameplayManager.Instance.skinData.coinSkin)
        {
            if (!ShopManager.Instance.IsOwnSkin(item.skinID))
            {
                listIDSkinCoin.Add(item.skinID);
            }
            else
            {
                listIDSkinUnlock.Add(item.skinID);
            }
        }

        for (int i = 0; i < listGif.Length; i++)
        {
            listGiftData.listGiftDataLocal[i].idGift = i;
            if (listGiftData.listGiftDataLocal[i].typeOfSkin == TypeOfSkin.PremiumSkin)
            {
                if (listIDSkinPremium.Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, listIDSkinPremium.Count);
                    listGiftData.listGiftDataLocal[i].idSkin = listIDSkinPremium[index];
                    listIDSkinPremium.RemoveAt(index);
                    listGiftData.listGiftDataLocal[i].isUnlock = false;
                }
                else if (listIDSkinCoin.Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, listIDSkinCoin.Count);
                    listGiftData.listGiftDataLocal[i].idSkin = listIDSkinCoin[index];
                    listIDSkinCoin.RemoveAt(index);
                    listGiftData.listGiftDataLocal[i].isUnlock = false;
                    listGiftData.listGiftDataLocal[i].typeOfSkin = TypeOfSkin.CoinSkin;
                }
                else
                {
                    int index = UnityEngine.Random.Range(0, listIDSkinUnlock.Count);
                    listGiftData.listGiftDataLocal[i].idSkin = listIDSkinUnlock[index];
                    listIDSkinUnlock.RemoveAt(index);
                    listGiftData.listGiftDataLocal[i].isUnlock = true;
                }
            }
            else if (listGiftData.listGiftDataLocal[i].typeOfSkin == TypeOfSkin.CoinSkin)
            {
                if (listIDSkinCoin.Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, listIDSkinCoin.Count);
                    listGiftData.listGiftDataLocal[i].idSkin = listIDSkinCoin[index];
                    listIDSkinCoin.RemoveAt(index);
                    listGiftData.listGiftDataLocal[i].isUnlock = false;
                }
                else if (listIDSkinPremium.Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, listIDSkinPremium.Count);
                    listGiftData.listGiftDataLocal[i].idSkin = listIDSkinPremium[index];
                    listIDSkinPremium.RemoveAt(index);
                    listGiftData.listGiftDataLocal[i].isUnlock = false;
                    listGiftData.listGiftDataLocal[i].typeOfSkin = TypeOfSkin.PremiumSkin;
                }
                else
                {
                    int index = UnityEngine.Random.Range(0, listIDSkinUnlock.Count);
                    listGiftData.listGiftDataLocal[i].idSkin = listIDSkinUnlock[index];
                    listIDSkinUnlock.RemoveAt(index);
                    listGiftData.listGiftDataLocal[i].isUnlock = true;
                }
            }
        }


        PlayerPrefs.SetString(KEYGIFTDATA, JsonUtility.ToJson(listGiftData));
    }
    void UpdateData()
    {
        PlayerPrefs.SetString(KEYGIFTDATA, JsonUtility.ToJson(listGiftData));
    }

    void SetViewGift()
    {
        for (int i = 0; i < listGif.Length; i++)
        {
            listGif[i].SetViewGift(listGiftData.listGiftDataLocal[i], listGiftData.countVideo);
        }
    }
    void GetListInLocal()
    {
        listGiftData = JsonUtility.FromJson<ListGiftDataLocal>(PlayerPrefs.GetString(KEYGIFTDATA));
    }
    public void OnClickInsideGift(int _index)
    {
        GetListInLocal();

        if (!listGiftData.listGiftDataLocal[_index].isView)
        {
            listGiftData.listGiftDataLocal[_index].isView = true;
            SetViewGift();
            UpdateData();
        }
    }
    public void OnClickGetSkin()
    {
        // Đã xóa tham số Firebase khỏi lệnh gọi ShowVideoReward
        GameplayManager.Instance.ShowVideoReward(delegate
        {
            listGiftData.countVideo += 1;
            SetViewGift();
            UpdateData();
        });
    }
    public void OnClickUnlockSkin(int _indexGift)
    {
        listGiftData.listGiftDataLocal[_indexGift].isUnlock = true;
        listGiftData.countVideo -= listGiftData.listGiftDataLocal[_indexGift].numberVideoUnlock;
        ShopManager.Instance.UnlockSkin(listGiftData.listGiftDataLocal[_indexGift].idSkin);
        ShopManager.Instance.SetCurrentSkinSelect(listGiftData.listGiftDataLocal[_indexGift].idSkin);
        SetViewGift();
        UpdateData();
    }
    void CheckTime()
    {
        if (!PlayerPrefs.HasKey(KEYLASTGIFTREFRESH))
        {
            PlayerPrefs.SetString(KEYLASTGIFTREFRESH, System.DateTime.Now.ToBinary().ToString());

            UpdateTimeSpan();
        }
        else
        {
            UpdateTimeSpan();

            if (nextDay <= System.DateTime.Now)
            {
                GetListInLocal();
                Refresh();
            }
            else
            {
                GetListInLocal();
            }
        }
    }

    public TimeSpan GetTimeCountDown()
    {
        return nextDay.Subtract(System.DateTime.Now);
    }
    public void Refresh()
    {
        PlayerPrefs.SetString(KEYLASTGIFTREFRESH, System.DateTime.Now.ToBinary().ToString());
        UpdateTimeSpan();
        CreateNewData();

        SetViewGift();
    }

    void UpdateTimeSpan()
    {
        long lastTime = long.Parse(PlayerPrefs.GetString(KEYLASTGIFTREFRESH));
        DateTime cache = DateTime.FromBinary(lastTime);
    }
}