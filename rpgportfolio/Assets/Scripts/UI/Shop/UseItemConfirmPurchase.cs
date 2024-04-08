using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseItemConfirmPurchase : MonoBehaviour
{
    GameManager gameManager;

    public Image useItemConfirmWindow;

    public Image[] Inventroyslots;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sellSoundAC;

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

                int i = 0;
                while (true)
                {
                    if (Inventroyslots[i].transform.childCount == 1)
                        break;
                    i++;
                }
                Instantiate<Image>(gameManager.instantiateImageAtInven,
                    Inventroyslots[i].transform);

                // slotnumber 저장
                if (gameManager.instantiateImageAtInven.GetComponent<SlotNumber>() != null)
                    gameManager.instantiateImageAtInven.GetComponent<SlotNumber>().slotNumber = i;
                else
                    Debug.Log("slotnumber 스크립트 못차즘..");

                audioSource.PlayOneShot(sellSoundAC);
                gameManager.haveMoney -= gameManager.itemPrice;
            }
            else if(gameManager.useItemAmountArray[gameManager.useItemIndex] > 0)
            {
                gameManager.useItemAmountArray[gameManager.useItemIndex]+=1;

                audioSource.PlayOneShot(sellSoundAC);
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
