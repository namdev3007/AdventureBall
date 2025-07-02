using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TypeOfLottery
{
    Coin, Skin
}

[System.Serializable]
public class Lottery
{
    public TypeOfLottery typeOfLottery;
    public TypeOfSkin typeOfSkin;
    public int numberOfLottery; //if skin -> number = id skin
    public int rate;
    public bool isBesPrize;
}

[System.Serializable]
public class ListLotteryData
{
    public Lottery[] listLottery;
}

[CreateAssetMenu(fileName = "DataLottery", menuName = "ScriptableObject/DataLottery", order = 1)]
public class DataLottery : ScriptableObject
{
    public ListLotteryData listLottery;
    //public int[] LevelShowLotteryPanel;
    public int coinIfFullSkin;
}
