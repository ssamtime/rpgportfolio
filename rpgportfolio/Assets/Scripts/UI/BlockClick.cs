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
        // 클릭할때 공격x
        gameManager.blockClick = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        gameManager.blockClick = false;
    }
}
