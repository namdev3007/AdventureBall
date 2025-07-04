using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonVideoLive : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI text;

    void Awake()
    {
        button.onClick.AddListener(delegate
        {
            GameplayManager.Instance.AddLive(GameplayManager.LIVE_BONUS);
        });

        text.text = "+" + GameplayManager.LIVE_BONUS;
    }
}