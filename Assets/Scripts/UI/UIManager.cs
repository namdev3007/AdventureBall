using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UIManager : SingletonDontDestroy<UIManager>
{
    public HomePanel homePanel;
    public GamePlayUIManager gameplayUIPanel;
    public PausePanel pausePanel;
    public ShopPanel shopPanel;
    public RevivePanel revivePanel;
    public GetSkinPanel getSkinPanel;
    public NotiVideoPanel notiVideoPanel;
    public GameObject SpinPanel;
    public GameObject GiftPanel;
    public GameObject newSkinPanel;
    public LotteryPanel lotteryPanel;
    public LegacyUpdatePanel legacyUpdatePanel;
    public ForceUpdatePanel forceUpdatePanel;
    public SpecialOfferPanel specialOfferPanel;

    public RectTransform rectCanvas;
    void Start()
    {
        RxManager.victory.Subscribe(_ => ShowVictory());
        RxManager.openHomePanel.Subscribe(_ => ShowHomePanel());
        RxManager.openGameUI.Subscribe(_ => ShowGameplay());
        RxManager.showGameOverPanel.Subscribe(_ => ShowGameOver());
        RxManager.showRevivePanel.Subscribe(_ => showPanelRevive());
        RxManager.onClickPauseBtn.Subscribe(_ => PauseGame());
        RxManager.openShop.Subscribe(_ => ShowShopPanel());
        RxManager.NewSkinPanel.Subscribe(_idSkin => ActiveNewSkinPanel(true, _idSkin));
        // ?ã xóa: Dòng ??ng ký s? ki?n LoseLevelEvent
    }

    public void ShowLotteryPanel()
    {
        lotteryPanel.gameObject.SetActive(true);
    }
    void ShowShopPanel()
    {
        shopPanel.gameObject.SetActive(true);
        homePanel.gameObject.SetActive(false);
    }

    public void ShopBackToHome()
    {
        shopPanel.gameObject.SetActive(false);
        homePanel.gameObject.SetActive(true);
        // homePanel.BackToHome(); // Dòng này b? l?i vì HomePanel không có ph??ng th?c BackToHome, có th? b?n mu?n g?i ShowHome()
        homePanel.ShowHome();
    }

    public void HideHomePanel()
    {
        homePanel.gameObject.SetActive(false);
    }
    void showPanelRevive()
    {
        revivePanel.gameObject.SetActive(true);
    }

    public void ActiveGetSkinPanel(Skin skin = null)
    {
        gameplayUIPanel.gameObject.SetActive(false);

        if (skin != null)
        {
            getSkinPanel.SetUnlockSkin(skin);
        }
        getSkinPanel.gameObject.SetActive(true);
        AudioManager.Instance.ActiveAudioHome(true);
    }

    public void ActiveSpinPanel(bool _bool)
    {
        homePanel.gameObject.SetActive(!_bool);
        SpinPanel.SetActive(_bool);
    }
    public void ActiveGiftPanel(bool _bool)
    {
        homePanel.gameObject.SetActive(!_bool);
        GiftPanel.SetActive(_bool);
    }

    public void ActiveNewSkinPanel(bool _bool, int _idSkin = 0)
    {
        newSkinPanel.SetActive(_bool);
    }
    void ShowHomePanel()
    {
        homePanel.gameObject.SetActive(true);
        gameplayUIPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        homePanel.ShowHome();
        GameplayManager.Instance.StopCoroutinePlayerDie();
        AudioManager.Instance.ActiveAudioHome(true);
    }

    void ShowGameplay()
    {
        gameplayUIPanel.gameObject.SetActive(true);
        AudioManager.Instance.ActiveAudioHome(false);
    }

    void ShowVictory()
    {
        gameplayUIPanel.gameObject.SetActive(false);
        AudioManager.Instance.ActiveAudioHome(true);
        GameplayManager.Instance.Map += 1;
        homePanel.gameObject.SetActive(true);
        homePanel.ShowVictory();
    }


    void PauseGame()
    {
        pausePanel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    void ShowGameOver()
    {
        AudioManager.Instance.ActiveAudioHome(true);
        homePanel.gameObject.SetActive(true);
        homePanel.ShowLose();
    }

    public void OnClickSpecialOffer()
    {
        if (GameplayManager.IAP)
        {
            specialOfferPanel.gameObject.SetActive(true);
        }
    }
}