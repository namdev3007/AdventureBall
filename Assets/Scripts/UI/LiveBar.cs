using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UniRx;

public class LiveBar : MonoBehaviour
{
    public static Transform liveBarCurrent;
    public TextMeshProUGUI text;
    private void Awake()
    {
        RxManager.updateLive.Subscribe(value => UpdateText(value));
        if (!GameplayManager.IAP)
        {
            gameObject.GetComponent<Button>().enabled = false;
        }
    }

    private void OnEnable()
    {
        text.text = GameplayManager.Instance.Live.ToString();
        liveBarCurrent = text.transform;
    }

    private void UpdateText(int value)
    {
        text.text = value.ToString();
    }
}
