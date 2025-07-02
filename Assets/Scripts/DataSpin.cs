using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReWardSpinDaily
{
    public RewardData[] listRewardData;
}

[CreateAssetMenu(fileName = "DataSpin", menuName = "ScriptableObject/DataSpin", order = 1)]
public class DataSpin : ScriptableObject
{
    public ReWardSpinDaily listRewardDaily;
    public int numberCoinIfFullSkin;

    public DataSpin Clone()
    {
        DataSpin dataSpin = new DataSpin();
        dataSpin.listRewardDaily = JsonUtility.FromJson<ReWardSpinDaily>(JsonUtility.ToJson(this.listRewardDaily));
        dataSpin.numberCoinIfFullSkin = this.numberCoinIfFullSkin;
        return dataSpin;
    }
}
