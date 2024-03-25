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
        // ���콺 ���� �ִ� UI ��� ��������
        GameObject droppedObject = eventData.pointerCurrentRaycast.gameObject;

        if (droppedObject != null)
        {
            //Debug.Log("���� ��������ġ�� ������Ʈ : "+ droppedObject.name);

            SlotInfo droppedSlot = droppedObject.GetComponent<SlotInfo>();
            Image somethingImage = droppedObject.GetComponent<Image>();
            if (droppedSlot != null)
            {
                //Debug.Log("SlotInfo ��ũ��Ʈ�� ã��");
            }
            else
            {
                if(somethingImage != null)
                {
                }
                else
                {
                    // ����� ��ġ�� Slot ��ũ��Ʈ�� ���ų� image�� ������ ���� ��ġ�� �̵�
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
