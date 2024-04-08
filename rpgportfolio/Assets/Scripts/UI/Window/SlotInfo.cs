using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SlotInfo : MonoBehaviour, IDropHandler
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

        if(gameManager.wasQuickSlot == true)
        {
            print("5������");
            // ������ ������ ����
            Destroy(gameManager.beginDraggedIcon.gameObject);
            return;
        }

        // ����� ��ġ�� �̹� �������� ���ٸ�
        if (OriginIcon == null)
        {
            print("6������");
            gameManager.beginDraggedIcon.transform.SetParent(transform);
            gameManager.beginDraggedIcon.transform.position = transform.position;
            // beginDraggedIcon �� slotnumber �����ϱ�
            gameManager.beginDraggedIcon.GetComponent<SlotNumber>().slotNumber = slotNumber;
        }
        // ����� ��ġ�� �̹� �������� �ִٸ�
        else
        {
            print("7������");
            // ���� �ִ� �̹����� �巡�� ������ġ��
            OriginIcon.transform.parent = gameManager.startParent;
            OriginIcon.transform.position = gameManager.startPosition;
            // �巡���� �̹����� ����� ��ġ��
            gameManager.beginDraggedIcon.transform.SetParent(transform);
            gameManager.beginDraggedIcon.transform.position = transform.position;
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
