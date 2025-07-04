using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonVideoCoin : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI text;

    void Awake()
    {
        button.onClick.AddListener(delegate
        {
            // THAY ĐỔI: Gọi thẳng hàm cộng coin thay vì hàm xem video
            GameplayManager.Instance.AddCoin(GameplayManager.COIN_BONUS);
        });

        text.text = "+" + GameplayManager.COIN_BONUS;
    }
}