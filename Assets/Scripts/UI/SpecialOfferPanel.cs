using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpecialOfferPanel : MonoBehaviour
{
    public TextMeshProUGUI textPriceSpecialOffer;
    public GameObject buttonSpecialOffer;
    public TextMeshProUGUI text;
    public TextMeshProUGUI textPriceFake;
    public TextMeshProUGUI textRate;
    public GameObject buttonNoThanks;

    GameplayManager gameplayManager;
    private void Awake()
    {
        gameplayManager = GameplayManager.Instance;
    }
    private void Start()
    {
    }
    public void OnClickBuySpecialOffer()
    {
    }

    private void OnEnable()
    {
        StartCoroutine(WaitAndShowNoThanksButton());
    }

    IEnumerator WaitAndShowNoThanksButton()
    {
        buttonNoThanks.SetActive(false);
        yield return new WaitForSeconds(2);
        buttonNoThanks.SetActive(true);
    }

    private void OnDisable()
    {
    }
}