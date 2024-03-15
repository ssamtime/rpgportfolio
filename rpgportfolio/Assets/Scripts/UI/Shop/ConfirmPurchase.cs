using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPurchase : MonoBehaviour
{
    GameManager gameManager;

    public Image confirmWindow;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }


    void Update()
    {
        if (confirmWindow.gameObject.activeSelf)
        {
            // 엔터키 누르면
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PurchaseItem();
            }
        }
    }

    // 확인 버튼 누르면 실행되는 함수
    public void PurchaseItem()
    {
        if (gameManager.haveMoney >= gameManager.itemPrice)
        {            
            gameManager.inventorySlotList.Add(gameManager.listIndex);
            Instantiate<Image>(gameManager.instantiateImageAtInven, gameManager.InventorySlots[gameManager.listIndex].transform);
            gameManager.listIndex++;

            gameManager.haveMoney -= gameManager.itemPrice;

            confirmWindow.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("소지금이 부족합니다");
            confirmWindow.gameObject.SetActive(false);
        }
    }

    public void DestroyConfirmWindow()
    {
        confirmWindow.gameObject.SetActive(false);
    }
}
