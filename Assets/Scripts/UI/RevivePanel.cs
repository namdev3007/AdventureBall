using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections;
using UniRx;

public class RevivePanel : MonoBehaviour
{
    public Button buttonRevive;
    public Button tapToClose;

    public float timeActiveTapToClose;

    void Awake()
    {
        tapToClose.onClick.AddListener(delegate { OnClickClose(); });
        buttonRevive.onClick.AddListener(delegate { OnClickRevive(); });
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
        StartCoroutine(spawTapToClose());
    }

    private IEnumerator spawTapToClose()
    {
        yield return new WaitForSecondsRealtime(timeActiveTapToClose);
        tapToClose.gameObject.SetActive(true);
    }

    void OnClickClose()
    {
        RxManager.playerTapToCloseBtn.OnNext(true);
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    void OnClickRevive()
    {
        GameplayManager.Instance.ShowVideoReward(delegate
        {
            RxManager.playerSpawn.OnNext(true);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        });
    }
}