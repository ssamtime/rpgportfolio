using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SlotInfo : MonoBehaviour, IDropHandler
{
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    GameObject Icon()
    {
        if(transform.childCount > 1)
        {
            return transform.GetChild(1).gameObject;
        }
        else { return null; }
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject OriginIcon = Icon();
        if (OriginIcon == null)
        {
            gameManager.beginDraggedIcon.transform.SetParent(transform);
            gameManager.beginDraggedIcon.transform.position = transform.position;
        }
        else
        {
            // 원래 있던 이미지의 위치로 이동
            OriginIcon.transform.parent = gameManager.startParent;
            OriginIcon.transform.position = gameManager.startPosition;

            gameManager.beginDraggedIcon.transform.SetParent(transform);
            gameManager.beginDraggedIcon.transform.position = transform.position;
        }
    }

    
}
