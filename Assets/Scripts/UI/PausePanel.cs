using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public AudioMixerSnapshot snapshotPause;
    public AudioMixerSnapshot snapshotGamePlay;
    public Button offPauseBtn;
    public Button backToHomeBtn;
    public Button playAgainBtn;
    public Button skipADS;

    public Image tickMusic;
    public Image tickSound;
    public Image tickVibration;

    public Button Music;
    public Button Sound;
    public Button Vibration;

    const string KEY_MUSIC = "MUSIC";
    const string KEY_SOUND = "SOUND";
    public const string KEY_VIBRATION = "VIBRATION";


    GameplayManager gameplayManager;
    private void Awake()
    {
        gameplayManager = GameplayManager.Instance;
        offPauseBtn.onClick.AddListener(delegate { OnClickBtnOffPause(); });
        backToHomeBtn.onClick.AddListener(delegate { OnClickBackToHome(); });
        playAgainBtn.onClick.AddListener(delegate { OnClickPlayAgain(); });
        skipADS.onClick.AddListener(delegate { OnClickSkipAds(); });
    }

    private void OnEnable()
    {
        checkTickMusic();
        checkTickSound();
        if (snapshotPause != null)
        {
            snapshotPause.TransitionTo(0.1f);
        }

    }

    void OnClickBtnOffPause()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        if (snapshotGamePlay != null)
        {
            snapshotGamePlay.TransitionTo(0.1f);
        }

    }
    void OnClickBackToHome()
    {
        Time.timeScale = 1;
        RxManager.openHomePanel.OnNext(true);
    }

    void OnClickPlayAgain()
    {
        GameplayManager.Instance.StopCoroutinePlayerDie();
        RxManager.loadingDone.OnNext(delegate {
            Time.timeScale = 1;
            gameObject.SetActive(false);
            GameplayManager.Instance.player.Reset();
            RxManager.updateHeal.OnNext(GameplayManager.Instance.Heal);
            RxManager.clickedPlayAgainonPause.OnNext(true);

        });
    }

    void OnClickSkipAds()
    {
        GameplayManager.Instance.SkipLevel();
    }
    public void checkTickMusic()
    {
        if (PlayerPrefs.HasKey(KEY_MUSIC))
        {
            tickMusic.enabled = false;
        }
        else
        {
            tickMusic.enabled = true;
        }
    }

    public void checkTickSound()
    {
        if (PlayerPrefs.HasKey(KEY_SOUND))
        {
            tickSound.enabled = false;
        }
        else
        {
            tickSound.enabled = true;
        }
    }

    public void checkTickVibration()
    {
        if (PlayerPrefs.HasKey(KEY_VIBRATION))
        {
            tickVibration.enabled = false;
        }
        else
        {
            tickVibration.enabled = true;

            Handheld.Vibrate();
        }
    }
}