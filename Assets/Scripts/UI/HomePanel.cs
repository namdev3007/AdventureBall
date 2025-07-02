using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Collections;
using System.Collections.Generic;
using System;

public class HomePanel : MonoBehaviour
{
    public Button buttonPlay;
    public Button buttonNext;
    public Button buttonTapToReplay;
    public Button buttonSkipLevel;
    public Button btnx2Coin;
    public TextMeshProUGUI textCoinX2;
    public Button buttonShop;
    public Button buttonBackSkin;
    public Button buttonNextSkin;
    public Button buttonTrySkin;
    public Button buttonGetSkin;
    public Button buttonAddLive;
    public Button buttonAddCoin;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI liveText;
    public TextMeshProUGUI levelText;
    public GameObject effectVictory;
    public GameObject dailyRewardPanel;
    public GameObject panelTrue;
    public GameObject panelFalse;

    public AudioClip audioWin;
    public AudioSource audioMainHome;

    public int coinBonusFrame;
    private float time1frame;
    private int oldCoin;
    private bool addCoinDone;

    public int UocCuaSoMap;
    private List<Skin> listSkin = new List<Skin>();
    private int currentIndexSkin;
    private IEnumerator delayTapToPlay;

    public DataLottery dataLottery;

    public GameObject backgroundHome1;
    public GameObject backgroundHome2;

    IDisposable disposableUnlockSkin;

    private const string LASTTIMECLAIMDAILYREWARD = "LASTTIMECLAIMDAILYREWARD_FIX";

    GameplayManager gameplayManager;

    private void Awake()
    {
        gameplayManager = GameplayManager.Instance;

        buttonPlay.onClick.AddListener(OnClickPlayBtn);
        buttonNext.onClick.AddListener(OnClickNextMap);
        btnx2Coin.onClick.AddListener(OnClickX2Coin);
        buttonTapToReplay.onClick.AddListener(OnClickReplay);
        buttonSkipLevel.onClick.AddListener(OnClickBtnSkipLevel);
        buttonShop.onClick.AddListener(OnClickShop);
        buttonBackSkin.onClick.AddListener(() => OnClickBackNextSkin(-1));
        buttonNextSkin.onClick.AddListener(() => OnClickBackNextSkin(1));
        buttonTrySkin.onClick.AddListener(OnClickTrySkin);
        buttonGetSkin.onClick.AddListener(OnClickGetSkin);
        buttonAddLive.onClick.AddListener(OnClickShop);
        buttonAddCoin.onClick.AddListener(OnClickShop);

        buttonTapToReplay.GetComponentInChildren<TextMeshProUGUI>().text = "Tap to Continue";

        var skinData = GameplayManager.Instance.skinData;
        listSkin.AddRange(skinData.premiumSkin);
        listSkin.AddRange(skinData.rescueSkin);
        listSkin.AddRange(skinData.coinSkin);
    }

    private void OnEnable()
    {
        GameplayManager.Instance.StopCoroutinePlayerDie();

        SetCurrentSkin();
        RefreshTextCoinAndLive();

        effectVictory.SetActive(false);
        UpdateStateCurrentSkin();

        if (delayTapToPlay != null)
            StartCoroutine(delayTapToPlay);

        disposableUnlockSkin = RxManager.selectSkinId.Subscribe(_ =>
        {
            SetCurrentSkin();
            UpdateStateCurrentSkin();
        });
    }


    void SetCurrentSkin()
    {
        currentIndexSkin = GetRandomSkinLock();

        if (currentIndexSkin < 0)
        {
            int skinId = ShopManager.Instance.GetSkinIdCurrent();
            currentIndexSkin = listSkin.FindIndex(skin => skin.skinID == skinId);
        }
    }

    int GetRandomSkinLock()
    {
        if (!ShopManager.Instance.isRandomSkin) return -1;

        List<int> listIndex = new List<int>();
        for (int i = 0; i < listSkin.Count; i++)
        {
            int id = listSkin[i].skinID;
            if (!ShopManager.Instance.IsOwnSkin(id) && !ShopManager.Instance.IsTrySkin(id))
                listIndex.Add(i);
        }

        return listIndex.Count > 0 ? listIndex[UnityEngine.Random.Range(0, listIndex.Count)] : -1;
    }

    void RefreshTextCoinAndLive()
    {
        liveText.text = GameplayManager.Instance.Live.ToString();
        moneyText.text = GameplayManager.Instance.Money.ToString();
    }

    private bool IsCanPlaySkin()
    {
        int skinID = listSkin[currentIndexSkin].skinID;
        return ShopManager.Instance.IsOwnSkin(skinID) ||
                (ShopManager.Instance.IsSubscribed() && currentIndexSkin < GameplayManager.Instance.skinData.premiumSkin.Count) ||
                ShopManager.Instance.GetCurrentSkinTry() == skinID;
    }

    private void UpdateStateCurrentSkin()
    {
        if (IsCanPlaySkin())
        {
            buttonTrySkin.gameObject.SetActive(false);
            buttonGetSkin.gameObject.SetActive(false);
        }
        else
        {
            bool isRescueSkin = currentIndexSkin >= GameplayManager.Instance.skinData.premiumSkin.Count &&
                                currentIndexSkin < GameplayManager.Instance.skinData.premiumSkin.Count + GameplayManager.Instance.skinData.rescueSkin.Count;

            buttonTrySkin.gameObject.SetActive(!isRescueSkin);
            buttonGetSkin.gameObject.SetActive(isRescueSkin);
        }
    }

    public void OnClickPlayBtn() => PlayGame();
    public void OnClickNextMap() => PlayGame();

    void PlayGame()
    {
        if (!GameplayManager.Instance.CheckCanPlayMap()) return;

        gameObject.SetActive(false);
        GameplayManager.Instance.PreLoadMapAsync();

        RxManager.actionPanelBlack.OnNext(() =>
        {
            CheckPlaySkin();
            GameplayManager.Instance.LoadMap();
            delayTapToPlay = null;
        });
    }

    void CheckPlaySkin()
    {
        int skinId = listSkin[currentIndexSkin].skinID;
        if (skinId != ShopManager.Instance.GetCurrentSkinSelect() && IsCanPlaySkin())
            ShopManager.Instance.SetCurrentSkinSelect(skinId);
    }

    public void OnClickX2Coin()
    {
        GameplayManager.Instance.AddCoin(GameplayManager.Instance.GetMoneyCache());
        buttonNext.gameObject.SetActive(true);
        btnx2Coin.gameObject.SetActive(false);
    }

    public void OnClickReplay() => PlayGame();
    public void OnClickShop() => RxManager.openShop.OnNext(true);

    public void OnClickTrySkin()
    {
        GameplayManager.Instance.ShowVideoReward(() =>
        {
            ShopManager.Instance.OnClickTrySkin(listSkin[currentIndexSkin].skinID);
            UpdateStateCurrentSkin();
        });
    }

    public void OnClickGetSkin()
    {
        GameplayManager.Instance.ShowVideoReward(() =>
        {
            ShopManager.Instance.PurchaseSkin(listSkin[currentIndexSkin].skinID);
            UpdateStateCurrentSkin();
        });
    }

    public void OnClickBackNextSkin(int step)
    {
        currentIndexSkin = (currentIndexSkin + step + listSkin.Count) % listSkin.Count;
        UpdateStateCurrentSkin();
    }

    public void ShowHome()
    {
        levelText.text = $"Level {GameplayManager.Instance.Map}";
        levelText.enabled = true;

        panelTrue.SetActive(true);
        panelFalse.SetActive(false);
        buttonTrySkin.gameObject.SetActive(false);
        buttonTapToReplay.gameObject.SetActive(false);
        buttonNext.gameObject.SetActive(false);
        btnx2Coin.gameObject.SetActive(false);
        buttonPlay.gameObject.SetActive(true);
        buttonSkipLevel.gameObject.SetActive(false);

        UpdateStateCurrentSkin();
    }

    public void ShowLose()
    {
        levelText.text = "Level Failed";
        levelText.enabled = true;

        panelTrue.SetActive(false);
        panelFalse.SetActive(true);
        buttonPlay.gameObject.SetActive(false);
        buttonSkipLevel.gameObject.SetActive(true);
        buttonTrySkin.gameObject.SetActive(false);
        buttonTapToReplay.gameObject.SetActive(false);
        buttonNext.gameObject.SetActive(false);
        btnx2Coin.gameObject.SetActive(false);

        delayTapToPlay = WaitShowTapToPlay(2);
        StartCoroutine(delayTapToPlay);
        UpdateStateCurrentSkin();
    }

    public void ShowVictory()
    {
        levelText.text = "Level Complete";
        levelText.enabled = true;

        panelTrue.SetActive(true);
        panelFalse.SetActive(false);
        buttonTrySkin.gameObject.SetActive(false);
        buttonTapToReplay.gameObject.SetActive(false);
        buttonNext.gameObject.SetActive(false);
        buttonPlay.gameObject.SetActive(false);
        buttonSkipLevel.gameObject.SetActive(false);

        RxManager.playAudioE.OnNext(audioWin);
        audioMainHome.mute = true;
        StartCoroutine(PlayAudioMainHomeIfWin());

        effectVictory.SetActive(true);
        GameplayManager.Instance.AddCoin(GameplayManager.Instance.GetMoneyCache());

        if (GameplayManager.Instance.Map > GameplayManager.LEVEL_X2COIN)
        {
            textCoinX2.text = "+" + GameplayManager.Instance.GetMoneyCache();
            btnx2Coin.gameObject.SetActive(true);
            delayTapToPlay = WaitShowTapToPlay(3);
            StartCoroutine(delayTapToPlay);
        }
        else
        {
            buttonNext.gameObject.SetActive(true);
        }

        UpdateStateCurrentSkin();

        // Đã xóa khối logic kiểm tra và hiển thị panel Rate tại đây.

        if (GameplayManager.LEVEL_SHOW_LOTTERY.Contains($",{GameplayManager.Instance.Map - 1},"))
            UIManager.Instance.ShowLotteryPanel();

        HandleDailyReward();
    }

    void HandleDailyReward()
    {
        if (GameplayManager.Instance.Map <= GameplayManager.LEVEL_DAILY_REWARD) return;

        if (PlayerPrefs.HasKey(LASTTIMECLAIMDAILYREWARD))
        {
            DateTime lastClaim = DateTime.FromBinary(long.Parse(PlayerPrefs.GetString(LASTTIMECLAIMDAILYREWARD)));
            if (lastClaim.AddHours(24) <= DateTime.Now)
            {
                dailyRewardPanel.SetActive(true);
                PlayerPrefs.SetString(LASTTIMECLAIMDAILYREWARD, DateTime.Now.ToBinary().ToString());
            }
        }
        else
        {
            dailyRewardPanel.SetActive(true);
            PlayerPrefs.SetString(LASTTIMECLAIMDAILYREWARD, DateTime.Now.ToBinary().ToString());
        }
    }

    private IEnumerator WaitShowTapToPlay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        buttonTapToReplay.gameObject.SetActive(true);
        buttonTapToReplay.GetComponentInChildren<TextMeshProUGUI>().text = "Tap to Continue";
        delayTapToPlay = null;
    }

    private IEnumerator PlayAudioMainHomeIfWin()
    {
        yield return new WaitForSeconds(4);
        audioMainHome.mute = false;
    }

    public void OnclickSpinButton() => UIManager.Instance.ActiveSpinPanel(true);
    public void OnClickGiftButton() => UIManager.Instance.ActiveGiftPanel(true);

    void OnClickBtnSkipLevel()
    {
        if (!GameplayManager.Instance.CheckCanPlayNextMap()) return;

        GameplayManager.Instance.ShowVideoReward(() =>
        {
            GameplayManager.Instance.SkipLevel();
            RxManager.actionPanelBlack.OnNext(() =>
            {
                gameObject.SetActive(false);
                GameplayManager.Instance.LoadMap();
                delayTapToPlay = null;
            });
        });
    }
}