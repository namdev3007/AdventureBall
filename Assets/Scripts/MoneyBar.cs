using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UniRx;

public class MoneyBar : MonoBehaviour
{
    public static Transform currentMoneyBar;

    public TextMeshProUGUI text;

    System.IDisposable idis;

    private void Awake()
    {
        if (!GameplayManager.IAP)
        {
            gameObject.GetComponent<Button>().enabled = false;
        }
    }

    private void OnEnable()
    {
        idis = RxManager.updateMoney.Subscribe(value => UpdateText(value));
        text.text = GameplayManager.Instance.Money.ToString();
        currentMoneyBar = text.transform;
    }

    private void OnDisable()
    {
        if (idis != null)
        {
            idis.Dispose();
        }
    }

    private void UpdateText(int value)
    {
        text.text = value.ToString();
    }

}
