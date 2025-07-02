using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotteryPanel : MonoBehaviour
{
    GameplayManager gameplayManager;
    private void Awake()
    {
        gameplayManager = GameplayManager.Instance;
    }
    private void OnEnable()
    {
        //gameplayManager.HideBanner();
    }

    private void OnDisable()
    {
        if (gameplayManager != null)
        {
            //gameplayManager.ShowBanner();
        }
    }

    public void OnclickX()
    {
        this.gameObject.SetActive(false);
    }
}
