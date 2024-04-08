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

        // �巡���� �̹��� ������ ���̵���
        transform.SetAsLastSibling();
        transform.parent.SetAsLastSibling();
        transform.parent.parent.SetAsLastSibling();
        transform.parent.parent.parent.SetAsLastSibling();

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // ���콺 ���� �ִ� UI ��� ��������
        GameObject droppedObject = eventData.pointerCurrentRaycast.gameObject;

        if (droppedObject != null)
        {
            //Debug.Log("���� ��������ġ�� ������Ʈ : "+ droppedObject.name);

            SlotInfo droppedSlotInfo = droppedObject.GetComponent<SlotInfo>();
            Image somethingImage = droppedObject.GetComponent<Image>();
            QuickSlotInfo quickSlotInfo = droppedObject.GetComponent<QuickSlotInfo>();

            if (droppedSlotInfo != null)
            {
                //Debug.Log("SlotInfo ��ũ��Ʈ�� ã��");
            }
            else if(quickSlotInfo != null)
            {
                // quickSlotInfo OnDrop()���� ����ü ����
            }
            else
            {
                if(somethingImage != null)
                {
                }
                else
                // ����� ��ġ�� Slot ��ũ��Ʈ�� ���ų� image�� ������     
                {
                    if (gameManager.wasQuickSlot == false)
                    {
                        // ������ ������
                        discardConfirmWindow.transform.parent.SetAsLastSibling();
                        discardConfirmWindow.gameObject.SetActive(true);
                        transform.position = gameManager.startPosition;
                        transform.SetParent(gameManager.startParent);
                        // ���Ӹ޴����� ������ ������ ���
                        gameManager.discardDraggedIcon = gameManager.beginDraggedIcon;
                    }
                    else
                    {
                        // �����Կ� �ִ� �̹��� ����
                        Destroy(gameManager.beginDraggedIcon.gameObject);
                    }
                }
            }
        }
        // ���콺 ������ ���� ������Ʈ�� ���� ���
        else
        {
            // ������ ������
            if (gameManager.wasQuickSlot == false)
            {
                discardConfirmWindow.transform.parent.SetAsLastSibling();
                discardConfirmWindow.gameObject.SetActive(true);
                transform.position = gameManager.startPosition;
                transform.SetParent(gameManager.startParent);
                // ���Ӹ޴����� ������ ������ ���
                gameManager.discardDraggedIcon = gameManager.beginDraggedIcon;

            }
            else
            {
                // �����Կ� �ִ� �̹��� ����
                Destroy(gameManager.beginDraggedIcon.gameObject);
            }
        }

        gameManager.beginDraggedIcon = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        gameManager.wasQuickSlot = false;
    }

}
