using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Purchase : MonoBehaviour, IPointerClickHandler
{
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;

    public Image instantiateImageAtInven;

    GameManager gameManager;

    public int itemPrice;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if( gameManager == null )
        { Debug.Log("gameManager 못찾음");}

        gameManager.index = 0;
    }

    public void OnPointerClick(PointerEventData eData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;

            //Debug.Log("더블클릭댐");

            if(gameManager.haveMoney >= itemPrice)
            {
                gameManager.inventorySlotList.Add(gameManager.index);
                Instantiate<Image>(instantiateImageAtInven, gameManager.InventorySlots[gameManager.index].transform);
                gameManager.index++;

                gameManager.haveMoney -= itemPrice;
            }
        }
        else
        {
            isDoubleClicked = false;
            doubleClickedTime = Time.time;
        }
    }


}
