using UniRx;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    public static float speed = 1;
    private System.Action action;

    public Animator animator;
    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        gameObject.SetActive(false);
        RxManager.loadingDone.Subscribe(_action => setAction(_action));
    }

    public void setAction(System.Action _action)
    {
        animator.speed = speed;
        RxManager.audioSnapOnOff.OnNext(false);
        action = _action;
        gameObject.SetActive(true);

        Debug.Log("loadingDone");

    }

    public void DoneLoading()
    {
        if (action != null)
        {
            action.Invoke();

            action = null;
        }
        gameObject.SetActive(false);
        RxManager.audioSnapOnOff.OnNext(true);
        speed = 1;
    }
}
