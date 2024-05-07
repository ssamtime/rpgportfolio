using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UseItemPurchase : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] int price;
    [SerializeField] string itemName;
    [SerializeField] Image imageToInventory;

    [SerializeField] int useItemIndex;

    GameManager gameManager;

    public Image useItemConfirmWindow;

    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;



    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    public void OnPointerClick(PointerEventData eData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            // ����Ŭ�� ������ ����Ǵ� �κ�
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;
                        
            // ����� �������� ���� �ޱ�
            gameManager.itemPrice = price;
            gameManager.itemNameText = itemName;
            gameManager.instantiateImageAtInven = imageToInventory;
            gameManager.useItemIndex = useItemIndex;

            // ���� Ȯ��â ����
            useItemConfirmWindow.gameObject.SetActive(true);
            useItemConfirmWindow.transform.parent.SetAsLastSibling();
        }
        else
        {
            isDoubleClicked = false;
            doubleClickedTime = Time.time;
        }
    }

}