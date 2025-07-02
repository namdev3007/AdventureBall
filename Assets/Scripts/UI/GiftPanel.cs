using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GiftPanel : MonoBehaviour
{
    private const string ISREFRESHGIFTFIRSTRIGHT = "ISREFRESHGIFTFIRSTRIGHT";
    public ListGift listGift;

    public TextMeshProUGUI textTimeCountDown;
    public TextMeshProUGUI textRefresh;
    public Image imageAds;

    GameplayManager gameplayManager;
    private void Awake()
    {
        gameplayManager = GameplayManager.Instance;
    }
    public void OnclickBackButton()
    {
        RxManager.openHomePanel.OnNext(true);
        UIManager.Instance.ActiveGiftPanel(false);
    }

    private void Update()
    {
        textTimeCountDown.text = "" + string.Format("{0}:{1}:{2}", listGift.GetTimeCountDown().Hours, listGift.GetTimeCountDown().Minutes, listGift.GetTimeCountDown().Seconds);
    }

    public void OnclickRefreshButton()
    {
        if (ShopManager.Instance.IsUnlockAllSkin())
        {
            RxManager.ActionNotice.OnNext("You got unlock all skins");
        }
        else
        {
            if (PlayerPrefs.HasKey(ISREFRESHGIFTFIRSTRIGHT))
            {
                GameplayManager.Instance.ShowVideoReward(delegate
                {
                    listGift.Refresh();
                    CheckRefresh();
                });
            }
            else
            {
                listGift.Refresh();
                PlayerPrefs.SetString(ISREFRESHGIFTFIRSTRIGHT, ISREFRESHGIFTFIRSTRIGHT);
                CheckRefresh();
            }
        }
    }

    void CheckRefresh()
    {
        if (!PlayerPrefs.HasKey(ISREFRESHGIFTFIRSTRIGHT))
        {
            textTimeCountDown.gameObject.SetActive(false);
            textRefresh.text = "Free Refresh";
            textRefresh.alignment = TextAlignmentOptions.Center;
            imageAds.enabled = false;
        }
        else
        {
            textTimeCountDown.gameObject.SetActive(true);
            textRefresh.text = "Refresh";
            textRefresh.alignment = TextAlignmentOptions.Right;
            imageAds.enabled = true;
        }
    }
}