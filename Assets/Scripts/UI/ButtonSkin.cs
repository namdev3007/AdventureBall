using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

public class ButtonSkin : MonoBehaviour
{
    public Color colorLock;
    public Image border;
    public GameObject unLock;
    public GameObject select;
    public GameObject selected;
    public Image skinImage;
    public Button button;
    public GameObject trySkin;

    private int skinId = -1;

    IDisposable subscritionSelectSkin;

    private void OnEnable()
    {
        subscritionSelectSkin = RxManager.selectSkinId.Subscribe(id => OnSelectSkinId(id));
    }

    private void OnDisable()
    {
        if (subscritionSelectSkin != null)
        {
            subscritionSelectSkin.Dispose();
            subscritionSelectSkin = null;
        }
    }

    public void SetSkinData(int skinId)
    {
        this.skinId = skinId;
        selected.SetActive(false);
        if (skinImage != null)
        {
            skinImage.sprite = ShopManager.Instance.GetSpriteBySkinId(skinId);
        }

        SetUnlock(ShopManager.Instance.IsOwnSkin(skinId) || ShopManager.Instance.IsTrySkin(skinId));
        trySkin.SetActive(ShopManager.Instance.IsTrySkin(skinId));
    }

    public void UnlockSkinSubscription()
    {
        SetUnlock(true);
        trySkin.SetActive(false);
    }

    void OnSelectSkinId(int id)
    {
        if (id == skinId)
        {
            selected.SetActive(true);
        }
        else
        {
            selected.SetActive(false);
        }
    }

    public void SetUnlock(bool isUnlock)
    {
        unLock.SetActive(!isUnlock);
        if (isUnlock)
        {
            if (skinImage != null) skinImage.color = Color.white;
            if (border != null) border.color = Color.white;
        }
        else
        {
            if (skinImage != null) skinImage.color = colorLock;
            if (border != null) border.color = colorLock;
        }
    }

    public void SetCanUnlock(bool isUnlock)
    {

    }

    public void SetSelect(bool isSelect)
    {
        select.SetActive(isSelect);
    }

    public void Despan()
    {
        SmartPool.Instance.Despawn(gameObject);
        if (skinImage != null) skinImage.sprite = null;
    }
}