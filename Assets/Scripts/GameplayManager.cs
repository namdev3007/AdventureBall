using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Collections;
using UnityEngine.Events;

public class GameplayManager : SingletonDontDestroy<GameplayManager>
{
    public const int MAX_MAP = 100;

    public static int COIN_BONUS
    {
        get => PlayerPrefs.GetInt("COIN_BONUS", 3000);
        set => PlayerPrefs.SetInt("COIN_BONUS", value);
    }
    public static int LIVE_BONUS
    {
        get => PlayerPrefs.GetInt("LIVE_BONUS", 2);
        set => PlayerPrefs.SetInt("LIVE_BONUS", value);
    }
    public static int RANDOM_SKIN_PRICE
    {
        get => PlayerPrefs.GetInt("RANDOM_SKIN_PRICE", 20000);
        set => PlayerPrefs.SetInt("RANDOM_SKIN_PRICE", value);
    }
    public static int LEVEL_RATE
    {
        get => PlayerPrefs.GetInt("LEVEL_RATE", 15);
        set => PlayerPrefs.SetInt("LEVEL_RATE", value);
    }
    public static int DELTA_LEVEL_RATE
    {
        get => PlayerPrefs.GetInt("DELTA_LEVEL_RATE", 10);
        set => PlayerPrefs.SetInt("DELTA_LEVEL_RATE", value);
    }
    public static int LEVEL_DAILY_REWARD
    {
        get => PlayerPrefs.GetInt("LEVEL_DAILY_REWARD", 3);
        set => PlayerPrefs.SetInt("LEVEL_DAILY_REWARD", value);
    }
    public static int LEVEL_X2COIN
    {
        get => PlayerPrefs.GetInt("LEVEL_X2COIN", 10);
        set => PlayerPrefs.SetInt("LEVEL_X2COIN", value);
    }
    public static string LEVEL_SHOW_LOTTERY
    {
        get => PlayerPrefs.GetString("LEVEL_SHOW_LOTTERY", ",10,19,30,39,50,59,70,79,90,99,");
        set => PlayerPrefs.SetString("LEVEL_SHOW_LOTTERY", value);
    }

    public int mapTest = 1;
    public Transform parent;
    public PlayerController player;
    public Camera cam;

    private MapManager map;
    private Vector3 checkpointPos;
    private int money = 0;

    public const string MAP = "MAP";
    public const string MONEY = "MONEY";
    public const string LIVE = "LIVE";
    public const int MAX_HEAL = 3;

    public float timePlayerDieEfect = 3;
    public SkinData skinData;

    public AudioClip audioBossBg;
    public AudioClip audioBGNormal;

    private bool bugShowHome;

    public Camera cameraUI;
    public Camera cameraGamePlay;
    public GamePlayUIManager gameplayUI;

    public const string KEYRATE = "keyrate";
    public bool CanShowRate
    {
        get => (!PlayerPrefs.HasKey(KEYRATE) && (Map - 1 >= LEVEL_RATE && (Map - 1 - LEVEL_RATE) % DELTA_LEVEL_RATE == 0));
    }

    public float StartCameraSize { get; private set; }

    public bool iap = true;
    public static bool IAP
    {
        get
        {
#if UNITY_EDITOR
            return Instance.iap;
#else
                return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
#endif
        }
    }

    public GameObject backGround1;
    public GameObject backGround2;

    private int heal = 3;
    public int Heal { get => heal; }
    private ResourceRequest requestMap;

    private void Awake()
    {
        StartCameraSize = Camera.main.orthographicSize;
    }

    void Start()
    {
#if UNITY_EDITOR
        if (mapTest >= 0)
        {
            Map = mapTest;
        }
        Debug.Log($"LOG DEVICE: {SystemInfo.deviceUniqueIdentifier}");
#endif

        if (CheckCanPlayMap(false))
        {
            PreLoadMapAsync();
        }
        RxManager.loadingDone.OnNext(delegate {
            if (Map == 1)
            {
                Map = 1;
            }
            if (CheckCanPlayMap(false))
            {
                LoadMap();
            }
            else
            {
                RxManager.openHomePanel.OnNext(true);
            }
        });

        RxManager.victory.Subscribe(_ => {
            DestroyMap();
        });
        RxManager.trapped.Subscribe(_ => Trapped());
        RxManager.playerEatCoin.Subscribe(coin => EatCoin(coin));
        RxManager.enemyAttackPlayer.Subscribe(_ => EnemyAttack());
        RxManager.enemyAttackPlayerMore.Subscribe(_x => BeAttacked(_x));
        RxManager.openHomePanel.Subscribe(_ => DestroyMap());
        RxManager.clickedPlayAgainonPause.Subscribe(_ => OnClickPlayAgain());
        RxManager.SaveCoinIfVictory.Subscribe(_ => SaveMoney());
        RxManager.addCoin.Subscribe(coin => AddCoin(coin));
        RxManager.playerTapToCloseBtn.Subscribe(_ => PlayerTaptoClose());
        RxManager.playerSpawn.Subscribe(_ => PlayerRespawn());
        RxManager.TextLiveBonus.Subscribe(_value => AddLive(_value));
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) PlayerPrefs.Save();
    }

    public bool IsBackGround1()
    {
        return Map <= 31 || (Map >= 61 && Map < 81);
    }

    void SetBackGround()
    {
        if (IsBackGround1())
        {
            backGround1.SetActive(true);
            backGround2.SetActive(false);
        }
        else
        {
            backGround1.SetActive(false);
            backGround2.SetActive(true);
        }
    }

    public int Map
    {
        get => PlayerPrefs.GetInt(MAP, 1);
        set
        {
            PlayerPrefs.SetInt(MAP, value);
            PlayerPrefs.Save();
        }
    }

    public int Money
    {
        get => PlayerPrefs.GetInt(MONEY, 0);
        set
        {
            RxManager.updateMoney.OnNext(value);
            PlayerPrefs.SetInt(MONEY, value);
            PlayerPrefs.Save();
        }
    }

    public int Live
    {
        get => PlayerPrefs.GetInt(LIVE, 1);
        set
        {
            PlayerPrefs.SetInt(LIVE, value);
            PlayerPrefs.Save();
        }
    }

    public void PreLoadMapAsync()
    {
        requestMap = Resources.LoadAsync($"Map/Map {Map}", typeof(GameObject));
    }

    public void LoadMap()
    {
        StartCoroutine(LoadAsyncMap());
    }

    IEnumerator LoadAsyncMap()
    {
        SetBackGround();

        if (requestMap == null)
        {
            PreLoadMapAsync();
        }
        while (!requestMap.isDone)
        {
            yield return null;
        }

        DestroyMap();
        map = (Instantiate(requestMap.asset, parent) as GameObject).GetComponent<MapManager>();
        map.setStartMap(Map);
        requestMap = null;

        CreatePlayer();

        RxManager.openGameUI.OnNext(true);
        heal = 3;
        money = 0;

        if (Map == 15)
        {
            RxManager.SetBGMusicInMap.OnNext(audioBossBg);
        }
        else
        {
            RxManager.SetBGMusicInMap.OnNext(audioBGNormal);
        }

        RxManager.updateLive.OnNext(Live);
        RxManager.updateHeal.OnNext(Heal);

        Resources.UnloadUnusedAssets();
    }

    public void CreatePlayer()
    {
        checkpointPos = map.startPoint.startPoint.position;
        map.startPoint.textLevel.text = "Level " + Map;
        player.transform.position = checkpointPos;
        cam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, cam.transform.position.z);
        player.Reset();
        player.gameObject.SetActive(true);
    }

    public void DestroyMap()
    {
        if (map != null)
        {
            Destroy(map.gameObject);
        }
        player.gameObject.SetActive(false);
    }

    void Nextlevel()
    {
        LoadMap();
    }

    public void SkipLevel()
    {
        Map += 1;
        DestroyMap();
        PreLoadMapAsync();
    }

    void OnClickPlayAgain()
    {
        if (Map == 15 || Map == 30 || Map == 45)
        {
            DestroyMap();
            LoadMap();
        }
        else
        {
            map.ReStartMap();
        }
        CreatePlayer();
    }

    void Trapped()
    {
        BeAttacked(1);
    }

    void EnemyAttack()
    {
        BeAttacked(1);
    }

    void BeAttacked(int damage)
    {
        if (heal > 0)
        {
            Vibrate();
            heal -= damage;
            RxManager.updateHeal.OnNext(heal);
            if (heal <= 0)
            {
                StartCoroutine(PlayerDie());
            }
        }
    }

    private IEnumerator PlayerDie()
    {
        RxManager.playerDie.OnNext(true);
        bugShowHome = false;
        yield return new WaitForSeconds(timePlayerDieEfect);
        if (!bugShowHome)
        {
            if (Live <= 1 && !ShopManager.Instance.UnlimitedLive)
            {
                RxManager.showRevivePanel.OnNext(true);
            }
            else
            {
                PlayerRespawn();
                if (!ShopManager.Instance.UnlimitedLive)
                {
                    Live--;
                }
                RxManager.updateLive.OnNext(Live);
            }
        }
    }

    void PlayerTaptoClose()
    {
        DestroyMap();
        RxManager.showGameOverPanel.OnNext(true);
    }

    void PlayerRespawn()
    {
        heal = 3;
        StartCoroutine(WaitLoad());
    }

    IEnumerator WaitLoad()
    {
        yield return new WaitForSeconds(0.4f);

        map.reLoadCurrenCheckPoint();
        RxManager.updateHeal.OnNext(Heal);
        player.Reset();
        player.rb.simulated = false;
        player.transform.position = map.GetPlayerPosition();
        cam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, cam.transform.position.z);
        player.ShowEfectPlayerSpawn(player.transform.position);
    }

    public Vector3 ConverGameplayToUI(Vector3 _vector3)
    {
        return cameraUI.ScreenToWorldPoint(cameraGamePlay.WorldToScreenPoint(_vector3));
    }
    public Vector3 ConverUIToGameplay(Vector3 _vector3)
    {
        return cameraGamePlay.ScreenToWorldPoint(cameraUI.WorldToScreenPoint(_vector3));
    }

    public void ActivePlayer()
    {
        player.rb.simulated = true;
    }

    public void StopCoroutinePlayerDie()
    {
        bugShowHome = true;
    }

    public void ResetHealth()
    {
        heal = 3;
    }

    public int GetMoneyCache() => money;

    void EatCoin(int coinValue)
    {
        money += coinValue;
    }

    void SaveMoney()
    {
        Money += money;
    }

    public void AddCoin(int value)
    {
        Money += value;
    }

    void SubCoin(int value)
    {
        Money -= value;
    }

    public void UseMoney(int value, UnityAction callbackSuccess)
    {
        if (value <= Money)
        {
            callbackSuccess?.Invoke();
            SubCoin(value);
        }
        else
        {
            RxManager.ActionNotice.OnNext("Not Enough Coin");
        }
    }

    public void AddLive(int value)
    {
        Live += value;
        RxManager.updateLive.OnNext(Live);
    }

    public void OnClickVideoLive()
    {
        ShowVideoReward(() => AddLive(LIVE_BONUS));
    }

    public void OnClickVideoCoin()
    {
        ShowVideoReward(() => AddCoin(COIN_BONUS));
    }

    public void ShowVideoReward(UnityAction callbackSuccess)
    {
        callbackSuccess?.Invoke();
    }

    public void Vibrate()
    {
        if (!PlayerPrefs.HasKey(PausePanel.KEY_VIBRATION))
        {
            Handheld.Vibrate();
        }
    }

    public bool CheckCanPlayMap(bool noti = true)
    {
        if (Map <= MAX_MAP) return true;

        if (noti)
        {
            RxManager.ActionNotice.OnNext("Coming soon...");
        }
        return false;
    }

    public bool CheckCanPlayNextMap(bool noti = true)
    {
        if (Map < MAX_MAP) return true;

        if (noti)
        {
            RxManager.ActionNotice.OnNext("Coming soon...");
        }
        return false;
    }
}