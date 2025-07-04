using System.Collections;
using TMPro;
using UnityEngine;

public class TextCoin : MonoBehaviour
{
    public TextMeshPro textCoin;

    public void setTextCoin(int valua)
    {
        textCoin.text = "+" + valua;
    }
}
