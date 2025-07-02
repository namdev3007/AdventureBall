using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WheelOfFuture : MonoBehaviour
{
    public int numberGift;
    public float timeRotate;
    public int indexGiftWant;
    public int numberRotateCircle;

    private const float CIRCLE = 360.0f;
    private float angleOfOneGift;
    private float currentTime;
    private float angleWant;

    public AnimationCurve curve;
    private UnityAction eventFuture;

    public Transform parrentDataReward;

    private void Start()
    {
        angleOfOneGift = CIRCLE / numberGift;
        setPosition();
    }
    IEnumerator RotateWheel()
    {
        float startAngle = transform.eulerAngles.z;
        currentTime = 0;
        angleWant = (numberRotateCircle * CIRCLE + angleOfOneGift * indexGiftWant) - startAngle;
        while (currentTime < timeRotate)
        {
            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;

            float angleOfFrame = angleWant * curve.Evaluate(currentTime / timeRotate);

            this.transform.eulerAngles = new Vector3(0, 0, startAngle + angleOfFrame);

        }

        if (eventFuture != null)
        {
            eventFuture.Invoke();
        }
    }

    [ContextMenu("rotate")]
    public void RotationNow()
    {
        StartCoroutine(RotateWheel());
    }

    public void RotateCallBack(int _indexGift, UnityAction _event)
    {
        indexGiftWant = _indexGift;
        eventFuture = _event;
        StartCoroutine(RotateWheel());
    }

    public void setPosition()
    {
        for (int i = 0; i < parrentDataReward.childCount; i++)
        {
            parrentDataReward.GetChild(i).localEulerAngles = new Vector3(0, 0, -CIRCLE / numberGift * i);
        }
    }
}
