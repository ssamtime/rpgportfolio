using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPurchase : MonoBehaviour
{
    GameManager gameManager;

    public Image confirmWindow;

    public Image[] Inventroyslots;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sellSoundAC;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }


    void Update()
    {
        if (confirmWindow.gameObject.activeSelf)
        {
            // 엔터키 누르면
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PurchaseItem();
            }
        }
    }

    // 확인 버튼 누르면 실행되는 함수
    public void PurchaseItem()
    {
        if (gameManager.haveMoney >= gameManager.itemPrice)
        {
            int i = 0;
            while (true)
            {
                if (Inventroyslots[i].transform.childCount == 1)
                    break;
                i++; 
            }
            Instantiate<Image>(gameManager.instantiateImageAtInven,
                Inventroyslots[i].transform);

            gameManager.haveMoney -= gameManager.itemPrice;
            audioSource.PlayOneShot(sellSoundAC);
            confirmWindow.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("소지금이 부족합니다");
            confirmWindow.gameObject.SetActive(false);
        }
    }

    public void DestroyConfirmWindow()
    {
        confirmWindow.gameObject.SetActive(false);
    }
}
