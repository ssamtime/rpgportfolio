using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellConfirm : MonoBehaviour
{
    GameManager gameManager;

    public Image confirmWindow;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    public void SellItem()
    {
        //gameManager.inventorySlotList.Add(gameManager.listIndex);
        //Instantiate<Image>(gameManager.instantiateImageAtInven, gameManager.InventorySlots[gameManager.listIndex].transform);
        //gameManager.listIndex++;

        gameManager.haveMoney += gameManager.sellPrice;

        confirmWindow.gameObject.SetActive(false);
    }

    public void InActiveConfirmWindow()
    {
        confirmWindow.gameObject.SetActive(false);
    }
}
