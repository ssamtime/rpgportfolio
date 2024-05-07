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
            // ����Ű ������
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PurchaseUseItem();
            }
        }
    }

    // ����Ȯ�� ��ư ������ ����Ǵ� �Լ�
    public void PurchaseUseItem()
    {
        // �������� ������ ���ݺ��� ũ�� ���Ű���
        if (gameManager.haveMoney >= gameManager.itemPrice)
        {
            // ����� ��� �������� ������ 0����
            if (gameManager.useItemAmountArray[gameManager.useItemIndex]==0)
            {
                gameManager.useItemAmountArray[gameManager.useItemIndex] += 1;

                // �������� ������ �κ��丮�� ���ڸ� ã��
                int i = 0;
                while (true)
                {
                    if (Inventroyslots[i].transform.childCount == 1)
                        break;
                    i++;
                }
                Instantiate<Image>(gameManager.instantiateImageAtInven,
                    Inventroyslots[i].transform);

                audioSource.PlayOneShot(sellSoundAC);
                gameManager.haveMoney -= gameManager.itemPrice;

                // slotnumber ����
                if (gameManager.instantiateImageAtInven.GetComponent<SlotNumber>() != null)
                    gameManager.instantiateImageAtInven.GetComponent<SlotNumber>().slotNumber = i;
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
            Debug.Log("�������� �����մϴ�");
            useItemConfirmWindow.gameObject.SetActive(false);
        }
    }

    public void DestroyConfirmWindow()
    {
        useItemConfirmWindow.gameObject.SetActive(false);
    }
}
