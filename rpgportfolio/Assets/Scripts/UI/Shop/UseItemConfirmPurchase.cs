using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseItemConfirmPurchase : MonoBehaviour
{
    GameManager gameManager;

    public Image useItemConfirmWindow;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }


    void Update()
    {
        if (useItemConfirmWindow.gameObject.activeSelf)
        {
            // 엔터키 누르면
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PurchaseUseItem();
            }
        }
    }

    // 확인 버튼 누르면 실행되는 함수
    public void PurchaseUseItem()
    {
        if (gameManager.haveMoney >= gameManager.itemPrice)
        {
            if (gameManager.useItemAmountArray[gameManager.useItemIndex]==0)
            {
                gameManager.useItemAmountArray[gameManager.useItemIndex] += 1;

                gameManager.inventorySlotList.Add(gameManager.listIndex);
                Instantiate<Image>(gameManager.instantiateImageAtInven, gameManager.InventorySlots[gameManager.listIndex].transform);
                gameManager.listIndex++;

                gameManager.haveMoney -= gameManager.itemPrice;
            }
            else if(gameManager.useItemAmountArray[gameManager.useItemIndex] > 0)
            {
                gameManager.useItemAmountArray[gameManager.useItemIndex]+=1;
                gameManager.haveMoney -= gameManager.itemPrice;
            }


            useItemConfirmWindow.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("소지금이 부족합니다");
            useItemConfirmWindow.gameObject.SetActive(false);
        }
    }

    public void DestroyConfirmWindow()
    {
        useItemConfirmWindow.gameObject.SetActive(false);
    }
}
