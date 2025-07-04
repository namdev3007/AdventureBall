using System.Collections;
using TMPro;
using UnityEngine;

public class CointScript : MonoBehaviour
{
    public int coinValue;
    //public AudioClip audioCoin;

    public GameObject textCoin;


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            RxManager.playerEatCoin.OnNext(coinValue);
            //RxManager.playAudioCoin.OnNext(audioCoin);
            GameObject temp = SmartPool.Instance.Spawn(textCoin, transform.position, Quaternion.identity);
            temp.GetComponent<TextCoin>().setTextCoin(coinValue);
            Destroy(gameObject);
        }
    }

}
