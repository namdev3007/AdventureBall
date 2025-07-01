using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMapControl : MonoBehaviour
{
    public Animator animator;
    public Transform posEnd;
    public AudioSource audioOpen;
    public AudioSource audioClose;

    private int id;
    private bool isOpen = false;

    //public GameObject cageSkinRescua;

    //private Skin skin;

    private void Start()
    {
        id = Animator.StringToHash("isOpen");
        isOpen = false;
        animator.SetBool(id, isOpen);

        //cageSkinRescua.SetActive(false);

        //skin = GameplayManager.Instance.skinData.GetSkinUnlockByLevel(GameplayManager.Instance.Map);

        //if (skin != null)
        //{
        //    cageSkinRescua.SetActive(true);
        //    skelatonSkin.skeletonDataAsset = ShopManager.Instance.GetSkeletonBySkinId(skin.skinID);
        //    skelatonSkin.Initialize(true);
        //}
    }

    public void OpenGate()
    {
        if (!isOpen)
        {
            isOpen = true;
            animator.SetBool(id, isOpen);
            if (audioOpen != null)
            {
                audioOpen.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(CloseGate(other.transform));
            other.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
            other.gameObject.GetComponent<GameController>().ExitButton();
            other.gameObject.GetComponent<GameController>().enabled = false;
        }
    }

    private IEnumerator CloseGate(Transform target)
    {
        //FirebaseManager.Instance.LogLevelWin(GameplayManager.Instance.Map);
        while ((target.position - posEnd.position).sqrMagnitude > 0.2f)
        {
            yield return new WaitForEndOfFrame();
        }
        //ShopManager.Instance.CheckFinishTrySkin();
        PlayerController playerController = target.GetComponent<PlayerController>();
        //playerController.Changelayer("Object");
        isOpen = false;
        animator.SetBool(id, isOpen);
        if (audioClose != null)
        {
            audioClose.Play();
        }
        yield return new WaitForSeconds(1);
        //if (skin == null)
        //{
        //    RxManager.victory.OnNext(true);
        //}
        //else
        //{
        //    UIManager.Instance.ActiveGetSkinPanel(skin);
        //}
    }
}
