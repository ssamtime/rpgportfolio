using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image toolTipImage;
    GameObject priorityImage;

    void Start()
    {
        priorityImage = transform.parent.gameObject;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        // �̹����� hierarchy ��������ΰ��� ���̵���
        priorityImage = transform.parent.gameObject;
        priorityImage.transform.SetAsLastSibling();
        toolTipImage.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipImage.gameObject.SetActive(false);

    }

}
