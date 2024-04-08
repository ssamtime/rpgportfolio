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
            // ����Ű ������
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PurchaseItem();
            }
        }
    }

    // Ȯ�� ��ư ������ ����Ǵ� �Լ�
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

            // slotnumber ����
            if (gameManager.instantiateImageAtInven.GetComponent<SlotNumber>()!=null)
                gameManager.instantiateImageAtInven.GetComponent<SlotNumber>().slotNumber = i;
            else
                Debug.Log("slotnumber ��ũ��Ʈ ������..");

            audioSource.PlayOneShot(sellSoundAC);
            gameManager.haveMoney -= gameManager.itemPrice;
            confirmWindow.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("�������� �����մϴ�");
            confirmWindow.gameObject.SetActive(false);
        }
    }

    public void DestroyConfirmWindow()
    {
        confirmWindow.gameObject.SetActive(false);
    }
}
