using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DataFuture : MonoBehaviour
{
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
        listDataChild = parrentDataChild.GetComponentsInChildren<DataChild>();

        UpdateView();
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

        RewardData winningReward = dataSpinCurrent.listRewardDaily.listRewardData[randomIndex];
        Debug.Log($"<color=green>QUYẾT ĐỊNH PHẦN THƯỞNG: Vị trí {randomIndex}, Loại: {winningReward.rewardType}, Giá trị: {winningReward.number}</color>");

        eventFuture = ClaimReward;
        wheelOfFuture.RotateCallBack(randomIndex, eventFuture);
    }

    void ClaimReward()
    {
        RewardData claimedReward = dataSpinCurrent.listRewardDaily.listRewardData[randomIndex];
        Debug.Log($"<color=blue>TRAO PHẦN THƯỞNG: Vị trí {randomIndex}, Loại: {claimedReward.rewardType}, Giá trị: {claimedReward.number}</color>");

        ClaimSkin();
        spinPanel.SpinDone();

        // Không cần làm mới lại vòng quay nữa
        // StartCoroutine(WaitAndRefreshReWard()); 

        popupReward.ActivePoup(dataSpinCurrent.listRewardDaily.listRewardData[randomIndex]);
    }

    // Đã xóa Coroutine WaitAndRefreshReWard()

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


    void UpdateView()
    {
        for (int i = 0; i < listDataChild.Length; i++)
        {
            listDataChild[i].SetData(dataSpinCurrent.listRewardDaily.listRewardData[i]);
        }
    }
}