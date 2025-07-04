using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class MiniPanelManager : MonoBehaviour
{
    public Transform tabParent;
    public Transform contentParent;
    private Button[] button;

    private int id;

    private int idSub;

    public Animator animatorMiniShop;

    private void Start()
    {
        RxManager.ShowTabShopSkin.Subscribe(_index => OnClickTabBtn(_index));

        id = Animator.StringToHash("Selected");
        idSub = Animator.StringToHash("OnEnable");

        button = tabParent.GetComponentsInChildren<Button>();
        for (int i = 0; i < button.Length; i++)
        {
            Button b = button[i];
            b.onClick.AddListener(delegate
            {
                OnClickTabBtn(b.transform.GetSiblingIndex());
            });
        }
        if (!GameplayManager.IAP)
        {
            contentParent.GetChild(0).gameObject.SetActive(false);
            button[0].gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForEndOfFrame();
        if (GameplayManager.IAP)
        {
            OnClickTabBtn(0);
        }
        else
        {
            OnClickTabBtn(1);
        }
    }

    public void OnClickTabBtn(int vitri)
    {
        for (int i = 0; i < button.Length; i++)
        {
            contentParent.GetChild(i).gameObject.SetActive(false);
            button[i].GetComponent<Animator>().SetBool(id, false);
        }
        contentParent.GetChild(vitri).gameObject.SetActive(true);
        button[vitri].GetComponent<Animator>().SetBool(id, true);
    }
}
