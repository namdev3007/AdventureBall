using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class ScaleBackGroundByDevice : MonoBehaviour
{
    public RectTransform canvasScaler;

    private void Awake()
    {
        if (canvasScaler == null)
        {
            canvasScaler = UIManager.Instance.GetComponent<RectTransform>();
        }
    }

    void OnEnable()
    {

        Image image = gameObject.GetComponent<Image>();

        float delta = ((image.sprite.bounds.size.x * transform.localScale.x) / (image.sprite.bounds.size.y * transform.localScale.y));
        float deltaScreen = (canvasScaler.sizeDelta.x / canvasScaler.sizeDelta.y);

        if (deltaScreen < delta)
        {
            image.rectTransform.sizeDelta = new Vector2(canvasScaler.sizeDelta.y * delta, canvasScaler.sizeDelta.y) * 1.001f;
        }
        else
        {
            image.rectTransform.sizeDelta = new Vector2(canvasScaler.sizeDelta.x, canvasScaler.sizeDelta.x / delta) * 1.001f;
        }
    }

}
