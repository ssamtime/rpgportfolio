using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSlotInfo : MonoBehaviour, IDropHandler
{
    GameManager gameManager;

    [SerializeField] public int slotNumber;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject OriginIcon = Icon();
        // ����� ��ġ�� �̹� �������� ���ٸ�
        if (OriginIcon == null)
        {
            if(gameManager.wasQuickSlot == false)
            {
                print("1����");
                //�����Կ� ����ϴ� �κ�

                // �巡���� ��ġ(�� ��ũ��Ʈ�� ���� ����)�� ������ ����
                // �հ� ���ľ� ����������?
                GameObject copyIcon = Instantiate<GameObject>(gameManager.beginDraggedIcon, transform);
                copyIcon.transform.position = transform.position;
                copyIcon.GetComponent<SlotNumber>().slotNumber = copyIcon.transform.parent.GetComponent<QuickSlotInfo>().slotNumber;
                copyIcon.GetComponent<CanvasGroup>().blocksRaycasts = true;
                // �巡���� ������ ������ġ�� �̵�
                gameManager.beginDraggedIcon.transform.position = gameManager.startPosition;
                gameManager.beginDraggedIcon.transform.SetParent(gameManager.startParent);
            }
            else
            {
                print("2����");
                // �巡���� �������� ��ũ��Ʈ���� ������ �̵�
                gameManager.beginDraggedIcon.transform.SetParent(transform);
                gameManager.beginDraggedIcon.transform.position = transform.position;
                // beginDraggedIcon �� slotnumber �����ϱ�
                gameManager.beginDraggedIcon.GetComponent<SlotNumber>().slotNumber = slotNumber;
            }
        }
        // ����� ��ġ�� �̹� �������� �ִٸ�
        else
        {
            if (gameManager.wasQuickSlot == false)
            {
                print("3����");
                // �巡���� ������ ����
                GameObject copyIcon = Instantiate<GameObject>(gameManager.beginDraggedIcon, transform);
                copyIcon.transform.position = transform.position;
                copyIcon.GetComponent<SlotNumber>().slotNumber = copyIcon.transform.parent.GetComponent<QuickSlotInfo>().slotNumber;
                copyIcon.GetComponent<CanvasGroup>().blocksRaycasts = true;
                // �ִ� ������ ����
                Destroy(OriginIcon.gameObject);

                // �巡���� ������ ������ġ�� �̵�
                gameManager.beginDraggedIcon.transform.position = gameManager.startPosition;
                gameManager.beginDraggedIcon.transform.SetParent(gameManager.startParent);
            }
            else
            {
                // �ڸ� �ٲٱ�
                print("4����");
                // ���� �ִ� �̹����� �巡�� ������ġ��
                OriginIcon.transform.parent = gameManager.startParent;
                OriginIcon.transform.position = gameManager.startPosition;
                OriginIcon.GetComponent<SlotNumber>().slotNumber = gameManager.startParent.GetComponent<QuickSlotInfo>().slotNumber;
                // �巡���� �̹����� ����� ��ġ��
                gameManager.beginDraggedIcon.transform.SetParent(transform);
                gameManager.beginDraggedIcon.transform.position = transform.position;
                // beginDraggedIcon �� slotnumber �����ϱ�
                gameManager.beginDraggedIcon.GetComponent<SlotNumber>().slotNumber = slotNumber;
            }
        }
    }

    // ���Կ� �������� ������ ������ ��ȯ, ������ null ��ȯ
    GameObject Icon()
    {
        if (transform.childCount > 1)
        {
            return transform.GetChild(1).gameObject;
        }
        else { return null; }
    }
}
