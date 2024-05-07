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

    // 슬롯에 아이템이 있으면 아이템 반환, 없으면 null 반환
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

        // 드랍한 위치에 이미 아이콘이 없다면
        if (OriginIcon == null)
        {
            gameManager.beginDraggedIcon.transform.SetParent(transform);
            gameManager.beginDraggedIcon.transform.position = transform.position;
            // beginDraggedIcon 에 slotnumber 저장하기
            gameManager.beginDraggedIcon.GetComponent<SlotNumber>().slotNumber = slotNumber;
        }
        // 드랍한 위치에 이미 아이콘이 있다면
        else
        {
            // 원래 있던 이미지를 드래그 시작위치로
            OriginIcon.transform.parent = gameManager.startParent;
            OriginIcon.transform.position = gameManager.startPosition;
            // 드래그한 이미지를 드랍한 위치로
            gameManager.beginDraggedIcon.transform.SetParent(transform);
            gameManager.beginDraggedIcon.transform.position = transform.position;
        }
        if (gameManager.wasQuickSlot == true)
        {
            // 퀵슬롯 아이콘 삭제
            Destroy(gameManager.beginDraggedIcon.gameObject);
            return;
        }
    }

}
