using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDND : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Transform targetTr; // 이동될 UI
    Vector2 startingPoint;
    Vector2 moveOffset;
    Vector2 moveBegin;

    void Start()
    {
        targetTr = transform.parent;
    }

    // 드래그 시작 위치 지정
    public void OnBeginDrag(PointerEventData eventData)
    {
        //GetComponent<CanvasGroup>().blocksRaycasts = false;
        startingPoint = targetTr.position;
        moveBegin = eventData.position;
    }

    // 드래그 : 마우스 커서 위치로 이동
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
