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
            // ����Ű ������
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PurchaseUseItem();
            }
        }
    }

    // Ȯ�� ��ư ������ ����Ǵ� �Լ�
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
            Debug.Log("�������� �����մϴ�");
            useItemConfirmWindow.gameObject.SetActive(false);
        }
    }

    public void DestroyConfirmWindow()
    {
        useItemConfirmWindow.gameObject.SetActive(false);
    }
}
