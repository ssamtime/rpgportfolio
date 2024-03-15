using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PotionUse : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;


    GameManager gameManager;


    [SerializeField] Image toolTipImage;
    GameObject priorityImage;

    public int potiontype; // 1: 빨간포션 2: 파란포션 3: 앨릭서

    int redPotionRecoverAmount;
    int bluePotionRecoverAmount;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        priorityImage = transform.parent.gameObject;
        redPotionRecoverAmount = 50;
        bluePotionRecoverAmount = 50;
            
    }

    public void OnPointerClick(PointerEventData eData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;

            //-------- 더블클릭시 실행되는 내용

            if(potiontype==1)
            {
                gameManager.useItemAmountArray[1]--;
                if (gameManager.useItemAmountArray[1] == 0)
                {
                    Destroy(gameObject);
                    // index 빈곳 저장해서 거기먼저 채우도록,,
                }
                if (gameManager.playerHP + redPotionRecoverAmount> gameManager.playerMaxHP)
                {
                    gameManager.playerHP = gameManager.playerMaxHP;
                }
                else
                {
                    gameManager.playerHP += redPotionRecoverAmount;
                }
            }
            else if(potiontype==2) 
            {
                gameManager.useItemAmountArray[2]--;
                if (gameManager.useItemAmountArray[2] == 0)
                {
                    Destroy(gameObject);
                    // index 빈곳 저장해서 거기먼저 채우도록,,
                }
                if (gameManager.playerMP + bluePotionRecoverAmount > gameManager.playerMaxMP)
                {
                    gameManager.playerMP = gameManager.playerMaxMP;
                }
                else
                {
                    gameManager.playerMP += bluePotionRecoverAmount;
                }
            }
            else if(potiontype ==3)
            {
                gameManager.useItemAmountArray[3]--;
                if (gameManager.useItemAmountArray[3] == 0)
                {
                    Destroy(gameObject);
                    // index 빈곳 저장해서 거기먼저 채우도록,,
                }

                gameManager.playerHP = gameManager.playerMaxHP;
                gameManager.playerMP = gameManager.playerMaxMP;
            }

            //-------------
        }
        else
        {
            // 클릭 시간 초기화
            isDoubleClicked = false;
            doubleClickedTime = Time.time;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTipImage.gameObject.SetActive(true);
        // 이미지가 hierarchy 가장밑으로가서 보이도록
        priorityImage.transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipImage.gameObject.SetActive(false);

    }
}
