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
        // 드랍한 위치에 이미 아이콘이 없다면
        if (OriginIcon == null)
        {
            if(gameManager.wasQuickSlot == false)
            {
                print("1실행");
                //퀵슬롯에 등록하는 부분

                // 드래그한 위치(이 스크립트를 가진 슬롯)에 아이콘 생성
                // 먼가 고쳐야 프리팹으로?
                GameObject copyIcon = Instantiate<GameObject>(gameManager.beginDraggedIcon, transform);
                copyIcon.transform.position = transform.position;
                copyIcon.GetComponent<SlotNumber>().slotNumber = copyIcon.transform.parent.GetComponent<QuickSlotInfo>().slotNumber;
                copyIcon.GetComponent<CanvasGroup>().blocksRaycasts = true;
                // 드래그한 아이콘 원래위치로 이동
                gameManager.beginDraggedIcon.transform.position = gameManager.startPosition;
                gameManager.beginDraggedIcon.transform.SetParent(gameManager.startParent);
            }
            else
            {
                print("2실행");
                // 드래그한 아이콘을 스크립트가진 곳으로 이동
                gameManager.beginDraggedIcon.transform.SetParent(transform);
                gameManager.beginDraggedIcon.transform.position = transform.position;
                // beginDraggedIcon 에 slotnumber 저장하기
                gameManager.beginDraggedIcon.GetComponent<SlotNumber>().slotNumber = slotNumber;
            }
        }
        // 드랍한 위치에 이미 아이콘이 있다면
        else
        {
            if (gameManager.wasQuickSlot == false)
            {
                print("3실행");
                // 드래그한 아이콘 생성
                GameObject copyIcon = Instantiate<GameObject>(gameManager.beginDraggedIcon, transform);
                copyIcon.transform.position = transform.position;
                copyIcon.GetComponent<SlotNumber>().slotNumber = copyIcon.transform.parent.GetComponent<QuickSlotInfo>().slotNumber;
                copyIcon.GetComponent<CanvasGroup>().blocksRaycasts = true;
                // 있던 아이콘 삭제
                Destroy(OriginIcon.gameObject);

                // 드래그한 아이콘 원래위치로 이동
                gameManager.beginDraggedIcon.transform.position = gameManager.startPosition;
                gameManager.beginDraggedIcon.transform.SetParent(gameManager.startParent);
            }
            else
            {
                // 자리 바꾸기
                print("4실행");
                // 원래 있던 이미지를 드래그 시작위치로
                OriginIcon.transform.parent = gameManager.startParent;
                OriginIcon.transform.position = gameManager.startPosition;
                OriginIcon.GetComponent<SlotNumber>().slotNumber = gameManager.startParent.GetComponent<QuickSlotInfo>().slotNumber;
                // 드래그한 이미지를 드랍한 위치로
                gameManager.beginDraggedIcon.transform.SetParent(transform);
                gameManager.beginDraggedIcon.transform.position = transform.position;
                // beginDraggedIcon 에 slotnumber 저장하기
                gameManager.beginDraggedIcon.GetComponent<SlotNumber>().slotNumber = slotNumber;
            }
        }
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
}
