using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ShopCoinSkin : MonoBehaviour
{
    public Button buttonRandom;
    public Button buttonSelect;
    public Button buttonSelected;
    public Button buttonTry;
    public Button buttonVideoCoin;
    public Transform skinParent;
    public GameObject buttonSkinPrefab;
    public TextMeshProUGUI textRandomPrice;
    public GameObject panelRandom;

    private List<ButtonSkin> buttonSkin = new List<ButtonSkin>();
    private SkinData skinData;
    private CoinSkin currentSkin;
    private ButtonSkin currentButtonSkin;
    private List<int> listRandom = new List<int>();

    private void Awake()
    {
        textRandomPrice.text = GameplayManager.RANDOM_SKIN_PRICE.ToString();

        buttonRandom.onClick.AddListener(delegate
        {
            OnClickRandom();
        });
        buttonSelect.onClick.AddListener(delegate
        {
            ShopManager.Instance.SetCurrentSkinSelect(currentSkin.skinID);
            OnClickSkin(currentSkin, currentButtonSkin);
        });
        buttonTry.onClick.AddListener(delegate
        {
            GameplayManager.Instance.ShowVideoReward(delegate
            {
                ShopManager.Instance.OnClickTrySkin(currentSkin.skinID);
                UpdateCurrentSkin();
            });
        });
    }

    private void UpdateCurrentSkin()
    {
        OnClickSkin(currentSkin, currentButtonSkin);
        currentButtonSkin.SetSkinData(currentSkin.skinID);
        buttonRandom.gameObject.SetActive(CheckCanRandom());
    }

    private void OnEnable()
    {
        skinData = GameplayManager.Instance.skinData;

        foreach (ButtonSkin btn in buttonSkin)
        {
            SmartPool.Instance.Despawn(btn.gameObject);
        }
        buttonSkin.Clear();

        List<CoinSkin> skinsToShow = new List<CoinSkin>();
        foreach (CoinSkin skin in skinData.coinSkin)
        {
            if (skin.skinID >= 7 && skin.skinID <= 9)
            {
                skinsToShow.Add(skin);
            }
        }

        for (int i = 0; i < skinsToShow.Count; i++)
        {
            CoinSkin skinIndex = skinsToShow[i];
            ButtonSkin button = SmartPool.Instance.Spawn(buttonSkinPrefab, skinParent.position, Quaternion.identity).GetComponent<ButtonSkin>();
            button.transform.SetParent(skinParent);
            button.transform.localScale = Vector3.one;
            buttonSkin.Add(button);
            button.button.onClick.RemoveAllListeners();
            button.button.onClick.AddListener(delegate
            {
                OnClickSkin(skinIndex, button);
            });
            button.SetSkinData(skinIndex.skinID);
            button.SetSelect(false);
        }

        if (skinsToShow.Count > 0)
        {
            OnClickSkin(skinsToShow[0], buttonSkin[0]);
        }

        ShopManager.Instance.PushNotiSelectCurrentSkin();
    }

    private void OnDisable()
    {
        // Vòng lặp này cũng nên bắt đầu từ 0
        for (int i = 0; i < buttonSkin.Count; i++)
        {
            ButtonSkin skin = buttonSkin[i];
            SmartPool.Instance.Despawn(skin.gameObject);
        }
        buttonSkin.Clear();
        currentButtonSkin = null;
    }

    private bool CheckCanRandom()
    {
        foreach (CoinSkin skin in skinData.coinSkin)
        {
            if (!ShopManager.Instance.IsOwnSkin(skin.skinID))
            {
                return true;
            }
        }
        return false;
    }

    private void OnClickSkin(CoinSkin skin, ButtonSkin button)
    {
        if (currentButtonSkin != null)
        {
            currentButtonSkin.SetSelect(false);
        }

        currentSkin = skin;
        currentButtonSkin = button;
        currentButtonSkin.SetSelect(true);
        buttonRandom.gameObject.SetActive(CheckCanRandom());
        if (ShopManager.Instance.IsOwnSkin(skin.skinID))
        {
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
        }
        else
        {
            buttonTry.gameObject.SetActive(false);
            buttonSelect.gameObject.SetActive(false);
            buttonVideoCoin.gameObject.SetActive(true);
            buttonSelected.gameObject.SetActive(false);
        }
    }

    public void OnClickRandom() //random
    {
        GameplayManager.Instance.UseMoney(GameplayManager.RANDOM_SKIN_PRICE, delegate
        {
            panelRandom.SetActive(true);
            listRandom.Clear();
            for (int i = 0; i < skinData.coinSkin.Count; i++)
            {
                CoinSkin skin = skinData.coinSkin[i];
                if (!ShopManager.Instance.IsOwnSkin(skin.skinID))
                {
                    listRandom.Add(i);
                }
            }
            int skinIndex = listRandom[UnityEngine.Random.Range(0, listRandom.Count)];

            int currentIndex = 0;
            int numberOfCircle = 4;
            int numberOfSelect = buttonSkin.Count * numberOfCircle + skinIndex;

            DOTween.To(() => currentIndex, x => currentIndex = x, numberOfSelect, 8).OnUpdate(delegate {
                for (int i = 0; i < buttonSkin.Count; i++)
                {
                    buttonSkin[i].SetSelect(currentIndex % buttonSkin.Count == i);
                }
            })
            .OnComplete(delegate
            {
                foreach (ButtonSkin bSkin in buttonSkin)
                {
                    bSkin.SetSelect(false);
                }
                panelRandom.SetActive(false);
                ShopManager.Instance.UnlockSkin(skinData.coinSkin[skinIndex].skinID);
                buttonSkin[skinIndex].SetSkinData(skinData.coinSkin[skinIndex].skinID);
                UpdateCurrentSkin();
            });
        });
    }
}