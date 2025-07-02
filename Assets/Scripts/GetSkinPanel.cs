using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSkinPanel : MonoBehaviour
{
    public Button buttonContinue;
    public Button buttonTry;
    public Button buttonBuy;
    public Button ButtonGet;

    private byte timeDelay = 2;

    public int skinID;
    private int typeOfSkin;

    private void Awake()
    {
        buttonContinue.onClick.AddListener(delegate { OnClickTapToHome(); });
    }

    private void OnEnable()
    {
        StartCoroutine(wait());
    }

    public void SetUnlockSkin(Skin skin)
    {
        buttonBuy.gameObject.SetActive(false);
        buttonTry.gameObject.SetActive(false);
        ButtonGet.gameObject.SetActive(false);
        skinID = skin.skinID;
        typeOfSkin = 0;
        if (skin is PremiumSkin)
        {
            buttonBuy.gameObject.SetActive(true);
            buttonTry.gameObject.SetActive(true);
            typeOfSkin = 1;
        }
        else if (skin is RescueSkin)
        {
            ButtonGet.gameObject.SetActive(true);
            typeOfSkin = 2;
        }
        else if (skin is CoinSkin)
        {
            buttonBuy.gameObject.SetActive(true);
            buttonTry.gameObject.SetActive(true);
            typeOfSkin = 3;
        }
        buttonContinue.gameObject.SetActive(false);
    }

    IEnumerator wait()
    {
        buttonContinue.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();


        StartCoroutine(WaitContinue());
    }

    IEnumerator WaitContinue()
    {
        yield return new WaitForSeconds(timeDelay);
        buttonContinue.gameObject.SetActive(true);
    }

    public void OnClickGetSkinButton()
    {
        GameplayManager.Instance.ShowVideoReward(delegate
        {
            ShopManager.Instance.PurchaseSkin(skinID);
            RxManager.victory.OnNext(true);
            gameObject.SetActive(false);
        });
    }

    public void OnClickTapToHome()
    {
        RxManager.victory.OnNext(true);
        gameObject.SetActive(false);
    }


    public void OnclickTry()
    {
        GameplayManager.Instance.ShowVideoReward(delegate
        {
            ShopManager.Instance.OnClickTrySkin(skinID);
            OnClickTapToHome();
        });
    }

    public void OnClickBuy()
    {
        RxManager.openShop.OnNext(true);
        StartCoroutine(waitX());
    }

    IEnumerator waitX()
    {
        yield return new WaitForEndOfFrame();
        RxManager.ShowTabShopSkin.OnNext(typeOfSkin);
        OnClickTapToHome();
    }
}