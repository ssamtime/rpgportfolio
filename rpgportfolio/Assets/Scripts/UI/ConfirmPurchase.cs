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

        gameManager.index = 0;
    }


    void Update()
    {
        
    }

    public void PurchaseItem()
    {
        if (gameManager.haveMoney >= gameManager.itemPrice)
        {
            gameManager.inventorySlotList.Add(gameManager.index);
            Instantiate<Image>(gameManager.instantiateImageAtInven, gameManager.InventorySlots[gameManager.index].transform);
            gameManager.index++;

            gameManager.haveMoney -= gameManager.itemPrice;


            confirmWindow.gameObject.SetActive(false);

        }
        else
        {
            Debug.Log("소지금이 부족합니다");
        }
    }

    public void DestroyConfirmWindow()
    {
        confirmWindow.gameObject.SetActive(false);
    }
}
