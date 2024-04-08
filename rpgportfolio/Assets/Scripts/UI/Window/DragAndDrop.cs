using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour ,IBeginDragHandler, IDragHandler , IEndDragHandler
{
    //public SlotInfo droppedSlot;

    Image discardConfirmWindow;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        discardConfirmWindow = gameManager.discardConfirmWindow;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gameObject.transform.parent.GetComponent<QuickSlotInfo>() != null)
            gameManager.wasQuickSlot = true;

        gameManager.beginDraggedIcon = gameObject;

        gameManager.startPosition = transform.position;
        gameManager.startParent = transform.parent;

        GetComponent<CanvasGroup>().blocksRaycasts = false;        

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

        // 드래그한 이미지 맨위로 보이도록
        transform.SetAsLastSibling();
        transform.parent.SetAsLastSibling();
        transform.parent.parent.SetAsLastSibling();
        transform.parent.parent.parent.SetAsLastSibling();

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 마우스 위에 있는 UI 요소 가져오기
        GameObject droppedObject = eventData.pointerCurrentRaycast.gameObject;

        if (droppedObject != null)
        {
            //Debug.Log("현재 포인터위치의 오브젝트 : "+ droppedObject.name);

            SlotInfo droppedSlotInfo = droppedObject.GetComponent<SlotInfo>();
            Image somethingImage = droppedObject.GetComponent<Image>();
            QuickSlotInfo quickSlotInfo = droppedObject.GetComponent<QuickSlotInfo>();

            if (droppedSlotInfo != null)
            {
                //Debug.Log("SlotInfo 스크립트를 찾음");
            }
            else if(quickSlotInfo != null)
            {
                // quickSlotInfo OnDrop()에서 복사체 생성
            }
            else
            {
                if(somethingImage != null)
                {
                }
                else
                // 드롭한 위치에 Slot 스크립트가 없거나 image가 없으면     
                {
                    if (gameManager.wasQuickSlot == false)
                    {
                        // 아이템 버리기
                        discardConfirmWindow.transform.parent.SetAsLastSibling();
                        discardConfirmWindow.gameObject.SetActive(true);
                        transform.position = gameManager.startPosition;
                        transform.SetParent(gameManager.startParent);
                        // 게임메니저에 버릴지 아이템 등록
                        gameManager.discardDraggedIcon = gameManager.beginDraggedIcon;
                    }
                    else
                    {
                        // 퀵슬롯에 있던 이미지 삭제
                        Destroy(gameManager.beginDraggedIcon.gameObject);
                    }
                }
            }
        }
        // 마우스 포인터 위에 오브젝트가 없을 경우
        else
        {
            // 아이템 버리기
            if (gameManager.wasQuickSlot == false)
            {
                discardConfirmWindow.transform.parent.SetAsLastSibling();
                discardConfirmWindow.gameObject.SetActive(true);
                transform.position = gameManager.startPosition;
                transform.SetParent(gameManager.startParent);
                // 게임메니저에 버릴지 아이템 등록
                gameManager.discardDraggedIcon = gameManager.beginDraggedIcon;

            }
            else
            {
                // 퀵슬롯에 있던 이미지 삭제
                Destroy(gameManager.beginDraggedIcon.gameObject);
            }
        }

        gameManager.beginDraggedIcon = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        gameManager.wasQuickSlot = false;
    }

}
