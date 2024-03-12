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
    }

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
            Debug.Log("�������� �����մϴ�");
        }
    }

    public void DestroyConfirmWindow()
    {
        confirmWindow.gameObject.SetActive(false);
    }
}
