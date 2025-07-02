using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceUpdatePanel : MonoBehaviour
{
    public Button update;

    private void Awake()
    {
        update.onClick.AddListener(OnclickUpdate);
    }


    public void OnclickUpdate()
    {
    }
}
