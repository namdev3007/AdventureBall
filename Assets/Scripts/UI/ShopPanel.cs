using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopPanel : MonoBehaviour
{
    public Button backBtn;

    public Button buttonSubscription;
    public Button buttonCoinPack;
    public Button buttonLivePack;

    public TextMeshProUGUI textPriceCoin;
    public TextMeshProUGUI textPriceLive;
    public TextMeshProUGUI textPriceSubscription1;

    public TextMeshProUGUI textCoinPack;
    public TextMeshProUGUI textCoinSubscription;

    private void Awake()
    {
        backBtn.onClick.AddListener(delegate
        {
            UIManager.Instance.ShopBackToHome();
        });

        buttonCoinPack.onClick.AddListener(delegate
        {
            ShopManager.Instance.PurchaseCoin(success =>
            {
                if (success)
                {
                    RxManager.ActionNotice.OnNext("+" + ShopManager.coinPack + " Coins");
                }
            });
        });
        buttonLivePack.onClick.AddListener(delegate
        {
            ShopManager.Instance.PurchaseLive();
        });
    }


    private void Start()
    {
        textPriceCoin.text = ShopManager.Instance.GetPrice(ShopManager.productCoin);
        textPriceLive.text = ShopManager.Instance.GetPrice(ShopManager.productLive);
        textPriceSubscription1.text = ShopManager.Instance.GetPrice(ShopManager.productSubscription) + "/week";
        textCoinPack.text = string.Format("+{0} Coins", ShopManager.coinPack);
        textCoinSubscription.text = string.Format("+{0} Coins", ShopManager.coinSubscription);
    }
}
