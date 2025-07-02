using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopPanel : MonoBehaviour
{
    public Button backBtn;

    public Button buttonSubscription;
    public Button buttonSubscriptionPack;
    public Button buttonRemoveAdsPack;
    public Button buttonCoinPack;
    public Button buttonLivePack;

    public Button buttonPolicy;
    public Button buttonTermsOfUse;

    public GameObject panelSubscription;

    public TextMeshProUGUI textPriceRemoveAds;
    public TextMeshProUGUI textPriceCoin;
    public TextMeshProUGUI textPriceLive;
    public TextMeshProUGUI textPriceSubscription1;
    public TextMeshProUGUI textPriceSubscription2;

    public TextMeshProUGUI textCoinPack;
    public TextMeshProUGUI textCoinSubscription;
    public TextMeshProUGUI textCoinSubscription2;

    private void Awake()
    {
        backBtn.onClick.AddListener(delegate
        {
            UIManager.Instance.ShopBackToHome();
        });

        buttonSubscriptionPack.onClick.AddListener(delegate
        {
            ShopManager.Instance.PurchaseSubscription(success =>
            {
                if (success)
                {
                    panelSubscription.SetActive(false);
                    RxManager.ActionNotice.OnNext("+" + ShopManager.coinSubscription + " Coins");
                }
            });
        });

        buttonRemoveAdsPack.onClick.AddListener(delegate
        {
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
        textPriceSubscription2.text = textPriceSubscription1.text;
        textCoinPack.text = string.Format("+{0} Coins", ShopManager.coinPack);
        textCoinSubscription.text = string.Format("+{0} Coins", ShopManager.coinSubscription);
        textCoinSubscription2.text = string.Format("Instantly get {0} Coins", ShopManager.coinSubscription);
    }
}
