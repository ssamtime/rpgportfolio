using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        // Ŭ���Ҷ� ����x
        gameManager.blockClick = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        gameManager.blockClick = false;
    }
}
