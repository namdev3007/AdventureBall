using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Gift : MonoBehaviour
{
    public Image skin;
    public TextMeshProUGUI text;
    public Image barGreen;
    public Image bgBarGreen;
    public GameObject unlockState;

    public GameObject borderYellow;
    public GameObject borderNormal;

    public GameObject buttonGetSkin;
    public GameObject buttonUnlock;
    public Button buttonUnlocked;

    public Animator animator;

    private int idTrigger;
    private int isGiftUnlock;
    private ListGift listGift;

    private int idGift;

    private void Start()
    {
        listGift = this.gameObject.GetComponentInParent<ListGift>();
        isGiftUnlock = Animator.StringToHash("isGiftUnlock");
        idTrigger = Animator.StringToHash("UnlockGift");
    }
    public void SetViewGift(GiftData _giftData, int _count)
    {
        idGift = _giftData.idGift;
        Reset();
        if (!_giftData.isView)
        {
            unlockState.SetActive(true);

            buttonUnlock.SetActive(false);
            buttonGetSkin.SetActive(false);
            buttonUnlocked.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(wait1frame());
            buttonUnlock.SetActive(_count >= _giftData.numberVideoUnlock);
            buttonGetSkin.SetActive(!(_count >= _giftData.numberVideoUnlock));
            unlockState.SetActive(false);
            if (_giftData.isUnlock)
            {
                Reset();
                buttonUnlocked.gameObject.SetActive(true);

                this.GetComponent<Button>().interactable = false;
                borderYellow.SetActive(false);
                borderNormal.SetActive(true);
                barGreen.gameObject.SetActive(false);
                bgBarGreen.gameObject.SetActive(false);
                text.gameObject.SetActive(false);
            }
            else
            {
                borderYellow.SetActive(_giftData.typeOfSkin == TypeOfSkin.PremiumSkin);
                borderNormal.SetActive(_giftData.typeOfSkin == TypeOfSkin.CoinSkin);

                barGreen.fillAmount = (float)_count / (float)_giftData.numberVideoUnlock;
                text.text = _count + "/" + _giftData.numberVideoUnlock;
            }
        }

        if (skin != null)
        {
            skin.gameObject.SetActive(true);

            skin.sprite = ShopManager.Instance.GetSpriteBySkinId(_giftData.idSkin);
        }
    }

    IEnumerator wait1frame()
    {
        yield return new WaitForEndOfFrame();
        animator.SetBool(isGiftUnlock, true);
    }
    void Reset()
    {
        borderYellow.SetActive(false);
        unlockState.SetActive(false);
        borderNormal.SetActive(false);
        buttonUnlock.SetActive(false);
        buttonGetSkin.SetActive(false);
        buttonUnlocked.gameObject.SetActive(false);
    }

    public void OnClickUnlockSkin()
    {
        if (listGift)
        {
            listGift.OnClickUnlockSkin(idGift);
        }
    }

    public void OnClickGetSkin()
    {
        if (listGift)
        {
            listGift.OnClickGetSkin();
        }
    }

    public void OnClickInSideGift()
    {
        if (listGift)
        {
            animator.SetTrigger(idTrigger);
            animator.enabled = true;
        }
    }

    public void ViewNew()
    {
        listGift.OnClickInsideGift(idGift);
    }
}