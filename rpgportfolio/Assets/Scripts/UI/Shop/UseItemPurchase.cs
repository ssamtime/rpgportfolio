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
            // 더블클릭 했을때 실행되는 부분
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;
                        
            // 사려는 아이템의 정보 받기
            gameManager.itemPrice = price;
            gameManager.itemNameText = itemName;
            gameManager.instantiateImageAtInven = imageToInventory;
            gameManager.useItemIndex = useItemIndex;

            // 구매 확인창 띄우기
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