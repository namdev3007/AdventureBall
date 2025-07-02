using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotiVideoPanel : MonoBehaviour
{
    public Button button;
    public Button tapToClose;

    private System.Action clickVideo;
    private void Awake()
    {
        button.onClick.AddListener(delegate
        {
            if (clickVideo != null)
            {
                clickVideo();
            }
            gameObject.SetActive(false);
        });
        tapToClose.onClick.AddListener(delegate { OnClickClose(); });
    }

    void OnClickClose()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        Time.timeScale = 0;
        StartCoroutine(spawTapToClose());
    }

    private IEnumerator spawTapToClose()
    {

        yield return new WaitForSecondsRealtime(1);
        tapToClose.gameObject.SetActive(true);

    }

    void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void ShowNoti(System.Action _clickVideo)
    {
        clickVideo = _clickVideo;
        gameObject.SetActive(true);
    }
}
