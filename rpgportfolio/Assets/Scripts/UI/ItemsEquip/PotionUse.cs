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
    GameObject player;


    [SerializeField] Image toolTipImage;
    GameObject priorityImage;

    public int potiontype; // 1: �������� 2: �Ķ����� 3: �ٸ���

    int redPotionRecoverAmount;
    int bluePotionRecoverAmount;

    [SerializeField] GameObject healingEffectPrefab;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
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

            // ����Ŭ���� ����Ǵ� ����
            UsePotion();

        }
        else
        {
            // Ŭ�� �ð� �ʱ�ȭ
            isDoubleClicked = false;
            doubleClickedTime = Time.time;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // �̹����� hierarchy ��������ΰ��� ���̵���
        priorityImage = transform.parent.gameObject;
        priorityImage.transform.SetAsLastSibling();
        toolTipImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipImage.gameObject.SetActive(false);

    }

    public void UsePotion()
    {
        GameObject healingFX = Instantiate<GameObject>(healingEffectPrefab, player.transform);
        Destroy(healingFX, 2f);
        if (potiontype == 1)
        {
            gameManager.useItemAmountArray[1]--;
            if (gameManager.useItemAmountArray[1] == 0)
            {
                Destroy(gameObject);
            }
            if (gameManager.playerHP + redPotionRecoverAmount > gameManager.playerMaxHP)
            {
                gameManager.playerHP = gameManager.playerMaxHP;
            }
            else
            {
                gameManager.playerHP += redPotionRecoverAmount;
            }
        }
        else if (potiontype == 2)
        {
            gameManager.useItemAmountArray[2]--;
            if (gameManager.useItemAmountArray[2] == 0)
            {
                Destroy(gameObject);
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
        else if (potiontype == 3)
        {
            gameManager.useItemAmountArray[3]--;
            if (gameManager.useItemAmountArray[3] == 0)
            {
                Destroy(gameObject);
            }

            gameManager.playerHP = gameManager.playerMaxHP;
            gameManager.playerMP = gameManager.playerMaxMP;
        }
    }
}
