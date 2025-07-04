using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopRescueSkin : MonoBehaviour
{
    public Image skinImage;
    public TextMeshProUGUI textLevelUnlock;
    public Button buttonGet;
    public Button buttonTry;
    public Button buttonSelect;
    public Button buttonSelected;
    public Button buttonVideoCoin;
    public Transform skinParent;
    public GameObject buttonSkinPrefab;

    private List<ButtonSkin> buttonSkin = new List<ButtonSkin>();
    private SkinData skinData;
    private RescueSkin currentSkin;
    private ButtonSkin currentButtonSkin;

    private void Awake()
    {
        buttonGet.onClick.AddListener(delegate
        {
            GameplayManager.Instance.ShowVideoReward(delegate
            {
                ShopManager.Instance.PurchaseSkin(currentSkin.skinID);
                UpdateCurrentSkin();
            });
        });
        buttonTry.onClick.AddListener(delegate
        {
            GameplayManager.Instance.ShowVideoReward(delegate
            {
                ShopManager.Instance.OnClickTrySkin(currentSkin.skinID);
                UpdateCurrentSkin();
            });
        });
        buttonSelect.onClick.AddListener(delegate
        {
            ShopManager.Instance.SetCurrentSkinSelect(currentSkin.skinID);
            OnClickSkin(currentSkin, currentButtonSkin);
        });
    }

    private void UpdateCurrentSkin()
    {
        OnClickSkin(currentSkin, currentButtonSkin);
        currentButtonSkin.SetSkinData(currentSkin.skinID);
    }

    private void OnEnable()
    {
        skinData = GameplayManager.Instance.skinData;
        foreach (RescueSkin skin in skinData.rescueSkin)
        {
            ButtonSkin button = SmartPool.Instance.Spawn(buttonSkinPrefab, skinParent.position, Quaternion.identity).GetComponent<ButtonSkin>();
            button.transform.SetParent(skinParent);
            button.transform.localScale = Vector3.one;
            buttonSkin.Add(button);
            button.button.onClick.RemoveAllListeners();
            button.button.onClick.AddListener(delegate
            {
                OnClickSkin(skin, button);
            });
            button.SetSkinData(skin.skinID);
            button.SetSelect(false);
        }
        OnClickSkin(skinData.rescueSkin[0], buttonSkin[0]);
        ShopManager.Instance.PushNotiSelectCurrentSkin();
    }

    private void OnDisable()
    {
        foreach (ButtonSkin skin in buttonSkin)
        {
            skin.Despan();
        }
        buttonSkin.Clear();
        currentButtonSkin = null;
    }

    private void OnClickSkin(RescueSkin skin, ButtonSkin button)
    {
        if (currentButtonSkin != null)
        {
            currentButtonSkin.SetSelect(false);
        }
        currentSkin = skin;
        currentButtonSkin = button;
        currentButtonSkin.SetSelect(true);

        if (skinImage != null)
        {
            skinImage.sprite = ShopManager.Instance.GetSpriteBySkinId(skin.skinID);
        }

        if (ShopManager.Instance.IsOwnOrTrySkin(skin.skinID))
        {
            buttonGet.gameObject.SetActive(false);
            buttonTry.gameObject.SetActive(false);
            if (ShopManager.Instance.IsSelectedSkin(skin.skinID))
            {
                buttonSelect.gameObject.SetActive(false);
                buttonSelected.gameObject.SetActive(true);
            }
            else
            {
                buttonSelect.gameObject.SetActive(true);
                buttonSelected.gameObject.SetActive(false);
            }
            buttonVideoCoin.gameObject.SetActive(true);
            textLevelUnlock.gameObject.SetActive(false);
        }
        else
        {
            if (GameplayManager.Instance.Map >= skin.levelUnlock)
            {
                buttonGet.gameObject.SetActive(true);
                buttonTry.gameObject.SetActive(false);
                textLevelUnlock.gameObject.SetActive(false);
            }
            else
            {
                buttonGet.gameObject.SetActive(false);
                buttonTry.gameObject.SetActive(true);
                textLevelUnlock.gameObject.SetActive(true);
                textLevelUnlock.text = "Unlock at Level " + skin.levelUnlock;
            }
            buttonSelect.gameObject.SetActive(false);
            buttonVideoCoin.gameObject.SetActive(true);
            buttonSelected.gameObject.SetActive(false);
        }
    }
}