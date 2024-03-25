using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDND : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Transform targetTr; // �̵��� UI
    Vector2 startingPoint;
    Vector2 moveOffset;
    Vector2 moveBegin;

    void Start()
    {
        targetTr = transform.parent;
    }

    // �巡�� ���� ��ġ ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        //GetComponent<CanvasGroup>().blocksRaycasts = false;
        startingPoint = targetTr.position;
        moveBegin = eventData.position;
    }

    // �巡�� : ���콺 Ŀ�� ��ġ�� �̵�
    public void OnDrag(PointerEventData eventData)
    {
        moveOffset = eventData.position - moveBegin;
        targetTr.position = startingPoint + moveOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //GetComponent<CanvasGroup>().blocksRaycasts = true;        
    }

}
