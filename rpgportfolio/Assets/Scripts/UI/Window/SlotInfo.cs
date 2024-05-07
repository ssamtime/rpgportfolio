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

    // ���Կ� �������� ������ ������ ��ȯ, ������ null ��ȯ
    GameObject Icon()
    {
        if (transform.childCount > 1)
        {
            return transform.GetChild(1).gameObject;
        }
        else { return null; }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject OriginIcon = Icon();

        // ����� ��ġ�� �̹� �������� ���ٸ�
        if (OriginIcon == null)
        {
            gameManager.beginDraggedIcon.transform.SetParent(transform);
            gameManager.beginDraggedIcon.transform.position = transform.position;
            // beginDraggedIcon �� slotnumber �����ϱ�
            gameManager.beginDraggedIcon.GetComponent<SlotNumber>().slotNumber = slotNumber;
        }
        // ����� ��ġ�� �̹� �������� �ִٸ�
        else
        {
            // ���� �ִ� �̹����� �巡�� ������ġ��
            OriginIcon.transform.parent = gameManager.startParent;
            OriginIcon.transform.position = gameManager.startPosition;
            // �巡���� �̹����� ����� ��ġ��
            gameManager.beginDraggedIcon.transform.SetParent(transform);
            gameManager.beginDraggedIcon.transform.position = transform.position;
        }
        if (gameManager.wasQuickSlot == true)
        {
            // ������ ������ ����
            Destroy(gameManager.beginDraggedIcon.gameObject);
            return;
        }
    }

}
