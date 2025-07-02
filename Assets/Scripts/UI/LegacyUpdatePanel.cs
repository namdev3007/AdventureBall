using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LegacyUpdatePanel : MonoBehaviour
{
    public Button noThank;
    public Button update;

    private void Awake()
    {
        noThank.onClick.AddListener(OnclickNoThanks);
        update.onClick.AddListener(OnclickUpdate);
    }

    public void OnclickNoThanks()
    {
        this.gameObject.SetActive(false);
    }

    public void OnclickUpdate()
    {
        this.gameObject.SetActive(false);
    }
}
