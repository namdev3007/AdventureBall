using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TypeOfSkin
{
    PremiumSkin,
    CoinSkin
}

[System.Serializable]
public class GiftData
{
    public TypeOfSkin typeOfSkin;
    public int idSkin;
    public int idGift;
    public byte numberVideoUnlock;
    public bool isUnlock;
    public bool isView;
}
