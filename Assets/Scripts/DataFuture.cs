using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class DataFuture : MonoBehaviour
{
    private const string KEYDATA = "SpinRaward";

    private UnityAction eventFuture;

    public WheelOfFuture wheelOfFuture;

    public DataSpin dataSpin;
    private DataSpin _dataSpinCurrent;
    private DataSpin dataSpinCurrent
    {
        get
        {
            if (_dataSpinCurrent == null)
            {
                _dataSpinCurrent = dataSpin.Clone();
            }
            return _dataSpinCurrent;
        }
    }

    public Transform parrentDataChild;
    private DataChild[] listDataChild;

    private float totalRate;

    int randomIndex = 0;

    public PopupReward popupReward;

    public SpinPanel spinPanel;
    private void Start()
    {
        dicNumberCoin = new Dictionary<int, int>();
        dicNumberHearth = new Dictionary<int, int>();

        listDataChild = parrentDataChild.GetComponentsInChildren<DataChild>();
        CreateReWard();

    }
    public void ActionRotate()
    {
        totalRate = 0;
        foreach (var item in dataSpinCurrent.listRewardDaily.listRewardData)
        {
            totalRate += item.rate;
        }

        float randomRate = Random.Range(0, totalRate);
        float currentRate = 0;


        for (int i = 0; i < dataSpinCurrent.listRewardDaily.listRewardData.Length; i++)
        {
            currentRate += dataSpinCurrent.listRewardDaily.listRewardData[i].rate;
            if (currentRate > randomRate)
            {
                randomIndex = i;
                break;
            }
        }

        eventFuture = ClaimReward;
        wheelOfFuture.RotateCallBack(randomIndex, eventFuture);
    }

    void ClaimReward()
    {
        ClaimSkin();
        spinPanel.SpinDone();

        StartCoroutine(WaitAndRefreshReWard());
        popupReward.ActivePoup(dataSpinCurrent.listRewardDaily.listRewardData[randomIndex]);
    }

    IEnumerator WaitAndRefreshReWard()
    {
        yield return new WaitForSeconds(0.5f);
        CreateNewReward();
        UpdateView();
    }

    public void ClaimSkin()
    {
        dataSpinCurrent.listRewardDaily.listRewardData[randomIndex].ClaimSkin();
        spinPanel.RefreshText();
    }

    public void Claim()
    {
        dataSpinCurrent.listRewardDaily.listRewardData[randomIndex].Claim();
        spinPanel.RefreshText();
    }

    public void ClaimX2()
    {
        dataSpinCurrent.listRewardDaily.listRewardData[randomIndex].Claimx2();
        spinPanel.RefreshText();
    }

    void CreateReWard()
    {
        if (PlayerPrefs.HasKey(KEYDATA))
        {
            dataSpinCurrent.listRewardDaily = JsonUtility.FromJson<ReWardSpinDaily>(PlayerPrefs.GetString(KEYDATA));
        }
        else
        {
            CreateNewReward();
        }
        UpdateView();
    }

    private Dictionary<int, int> dicNumberCoin;
    private Dictionary<int, int> dicNumberHearth;
    void CreateNewReward()
    {
        dicNumberCoin.Clear();
        dicNumberHearth.Clear();
        List<int> listPremiumSkin = new List<int>();
        foreach (var item in GameplayManager.Instance.skinData.premiumSkin)
        {
            if (!ShopManager.Instance.IsOwnSkin(item.skinID))
            {
                listPremiumSkin.Add(item.skinID);
            }
        }
        List<int> listCoinSkin = new List<int>();
        foreach (var item in GameplayManager.Instance.skinData.coinSkin)
        {
            if (!ShopManager.Instance.IsOwnSkin(item.skinID))
            {
                listCoinSkin.Add(item.skinID);
            }
        }
        List<int> listRescuaSkin = new List<int>();
        foreach (var item in GameplayManager.Instance.skinData.rescueSkin)
        {
            if (!ShopManager.Instance.IsOwnSkin(item.skinID))
            {
                listRescuaSkin.Add(item.skinID);
            }
        }
        for (int i = 0; i < listDataChild.Length; i++)
        {
            if (dataSpinCurrent.listRewardDaily.listRewardData[i].rewardType == RewardType.Skin)
            {
                listDataChild[i].text.text = "SKIN";

                if (dataSpinCurrent.listRewardDaily.listRewardData[i].skinType == SkinType.PremiumSkin)
                {

                    if (listPremiumSkin.Count > 0)
                    {
                        int index = Random.Range(0, listPremiumSkin.Count);
                        dataSpinCurrent.listRewardDaily.listRewardData[i].number = listPremiumSkin[index];
                        listPremiumSkin.RemoveAt(index);
                    }
                    else if (listCoinSkin.Count > 0)
                    {
                        int index = Random.Range(0, listCoinSkin.Count);
                        dataSpinCurrent.listRewardDaily.listRewardData[i].number = listCoinSkin[index];
                        listCoinSkin.RemoveAt(index);
                    }
                    else
                    {
                        dataSpinCurrent.listRewardDaily.listRewardData[i].rewardType = RewardType.Coin;
                        dataSpinCurrent.listRewardDaily.listRewardData[i].number = dataSpinCurrent.numberCoinIfFullSkin;
                    }

                }
                else if (dataSpinCurrent.listRewardDaily.listRewardData[i].skinType == SkinType.CoinSkin)
                {
                    if (listCoinSkin.Count > 0)
                    {
                        int index = Random.Range(0, listCoinSkin.Count);

                        dataSpinCurrent.listRewardDaily.listRewardData[i].number = listCoinSkin[index];
                        listCoinSkin.RemoveAt(index);
                    }
                    else
                    {
                        dataSpinCurrent.listRewardDaily.listRewardData[i].rewardType = RewardType.Coin;
                        dataSpinCurrent.listRewardDaily.listRewardData[i].number = dataSpinCurrent.numberCoinIfFullSkin;
                    }
                }
                else if (dataSpinCurrent.listRewardDaily.listRewardData[i].skinType == SkinType.RescuaSkin)
                {
                    if (listRescuaSkin.Count > 0)
                    {
                        int index = Random.Range(0, listRescuaSkin.Count);

                        dataSpinCurrent.listRewardDaily.listRewardData[i].number = listRescuaSkin[index];
                        listRescuaSkin.RemoveAt(index);
                    }
                    else
                    {
                        dataSpinCurrent.listRewardDaily.listRewardData[i].rewardType = RewardType.Coin;
                        dataSpinCurrent.listRewardDaily.listRewardData[i].number = dataSpinCurrent.numberCoinIfFullSkin;
                    }
                }
            }
            if (dataSpinCurrent.listRewardDaily.listRewardData[i].rewardType == RewardType.Coin)
            {
                dicNumberCoin.Add(i, dataSpinCurrent.listRewardDaily.listRewardData[i].number);
            }
            else if (dataSpinCurrent.listRewardDaily.listRewardData[i].rewardType == RewardType.Heart)
            {
                dicNumberHearth.Add(i, dataSpinCurrent.listRewardDaily.listRewardData[i].number);
            }
        }

        int numberCoinSmall = dicNumberCoin[dicNumberCoin.Keys.ElementAt(1)];
        int numberCoinBig = dicNumberCoin[dicNumberCoin.Keys.ElementAt(1)];

        int numberHearthSmall = dicNumberHearth[dicNumberHearth.Keys.ElementAt(1)];
        int numberHearthBig = dicNumberHearth[dicNumberHearth.Keys.ElementAt(1)];

        for (int i = 0; i < dicNumberCoin.Count; i++)
        {
            if (numberCoinSmall > dicNumberCoin[dicNumberCoin.Keys.ElementAt(i)])
            {
                numberCoinSmall = dicNumberCoin[dicNumberCoin.Keys.ElementAt(i)];
            }

            if (numberCoinBig < dicNumberCoin[dicNumberCoin.Keys.ElementAt(i)])
            {
                numberCoinBig = dicNumberCoin[dicNumberCoin.Keys.ElementAt(i)];
            }
        }

        for (int i = 0; i < dicNumberHearth.Count; i++)
        {
            if (numberHearthSmall > dicNumberHearth[dicNumberHearth.Keys.ElementAt(i)])
            {
                numberHearthSmall = dicNumberHearth[dicNumberHearth.Keys.ElementAt(i)];
            }

            if (numberHearthBig < dicNumberHearth[dicNumberHearth.Keys.ElementAt(i)])
            {

                numberHearthBig = dicNumberHearth[dicNumberHearth.Keys.ElementAt(i)];
            }
        }

        for (int i = 0; i < listDataChild.Length; i++)
        {
            if (dataSpinCurrent.listRewardDaily.listRewardData[i].rewardType == RewardType.Coin)
            {
                if (dataSpinCurrent.listRewardDaily.listRewardData[i].number == numberCoinSmall)
                {
                    dataSpinCurrent.listRewardDaily.listRewardData[i].sizeOfIcon = SizeOfIcon.Small;
                }
                else if (dataSpinCurrent.listRewardDaily.listRewardData[i].number == numberCoinBig)
                {
                    dataSpinCurrent.listRewardDaily.listRewardData[i].sizeOfIcon = SizeOfIcon.Big;
                }
                else
                {
                    dataSpinCurrent.listRewardDaily.listRewardData[i].sizeOfIcon = SizeOfIcon.Medium;
                }
            }
            else if (dataSpinCurrent.listRewardDaily.listRewardData[i].rewardType == RewardType.Heart)
            {
                if (dataSpinCurrent.listRewardDaily.listRewardData[i].number == numberHearthSmall)
                {
                    dataSpinCurrent.listRewardDaily.listRewardData[i].sizeOfIcon = SizeOfIcon.Small;
                }
                else if (dataSpinCurrent.listRewardDaily.listRewardData[i].number == numberHearthBig)
                {
                    dataSpinCurrent.listRewardDaily.listRewardData[i].sizeOfIcon = SizeOfIcon.Big;
                }
                else
                {
                    dataSpinCurrent.listRewardDaily.listRewardData[i].sizeOfIcon = SizeOfIcon.Medium;
                }
            }
        }

        PlayerPrefs.SetString(KEYDATA, JsonUtility.ToJson(dataSpinCurrent.listRewardDaily));
    }

    void UpdateView()
    {
        for (int i = 0; i < listDataChild.Length; i++)
        {
            listDataChild[i].SetData(dataSpinCurrent.listRewardDaily.listRewardData[i]);
        }
    }
}