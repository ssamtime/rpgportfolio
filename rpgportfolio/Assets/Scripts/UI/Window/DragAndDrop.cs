using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour ,IBeginDragHandler, IDragHandler , IEndDragHandler
{
    public SlotInfo droppedSlot;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        gameManager.beginDraggedIcon = gameObject;

        gameManager.startPosition = transform.position;
        gameManager.startParent = transform.parent;

        GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        transform.SetAsLastSibling();
        transform.parent.parent.SetAsLastSibling();

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 마우스 위에 있는 UI 요소 가져오기
        GameObject droppedObject = eventData.pointerCurrentRaycast.gameObject;

        if (droppedObject != null)
        {
            //Debug.Log("현재 포인터위치의 오브젝트 : "+ droppedObject.name);

            SlotInfo droppedSlot = droppedObject.GetComponent<SlotInfo>();
            Image somethingImage = droppedObject.GetComponent<Image>();
            if (droppedSlot != null)
            {
                //Debug.Log("SlotInfo 스크립트를 찾음");
            }
            else
            {
                if(somethingImage != null)
                {
                }
                else
                {
                    // 드롭한 위치에 Slot 스크립트가 없거나 image가 없으면 원래 위치로 이동
                    transform.position = gameManager.startPosition;
                    transform.SetParent(gameManager.startParent);
                }
            }
        }
        else
        {
            transform.position = gameManager.startPosition;
            transform.SetParent(gameManager.startParent);
        }

        gameManager.beginDraggedIcon = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

    }

}
