using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Purchaser : SingletonDontDestroy<Purchaser>
{
    private Dictionary<string, UnityAction<bool>> productCallbackDict = new Dictionary<string, UnityAction<bool>>();

    void DebugLog(object log)
    {
        Debug.Log(log);
    }

    public void AddProductConsume(string id, UnityAction<bool> callbackPurchase)
    {
        AddProductType(id, callbackPurchase);
    }

    public void AddProductNonConsume(string id, UnityAction<bool> callbackPurchase)
    {
        AddProductType(id, callbackPurchase);
    }

    public void AddProductSubscription(string id, UnityAction<bool> callbackPurchase)
    {
        AddProductType(id, callbackPurchase);
    }

    private void AddProductType(string id, UnityAction<bool> callbackPurchase)
    {
        if (!productCallbackDict.ContainsKey(id))
        {
            productCallbackDict.Add(id, callbackPurchase);
        }
    }

    public void Init()
    {
        DebugLog("Purchaser (Mock) is initialized.");
    }

    public void BuyProductID(string productId, UnityAction<bool> callback = null)
    {
        DebugLog($"Simulating an instant successful purchase for product: '{productId}'");

        callback?.Invoke(true);

        if (productCallbackDict.ContainsKey(productId))
        {
            productCallbackDict[productId]?.Invoke(true);
        }
        else
        {
            Debug.LogWarning($"No persistent callback found for product ID: {productId}");
        }
    }

    public string GetLocalizedPriceString(string pPackageId)
    {
        return "0.99$";
    }

    public bool IsSubscribed(string sku)
    {
        return false;
    }

    public void RestorePurchases()
    {
        DebugLog("Simulating restore purchases. Nothing to do in mock mode.");
    }
}
