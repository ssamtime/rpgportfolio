using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NeckEquip : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;


    public GameObject neck;
    GameObject player;
    PlayerMove playermoveScript;

    GameManager gameManager;
    public Image instantiateImageAtEquipWindow;
    Image instanceImage;

    [SerializeField] Image toolTipImage;
    GameObject priorityImage;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playermoveScript = player.GetComponent<PlayerMove>();
        neck = playermoveScript.equippedNeck;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        priorityImage = transform.parent.gameObject;
    }

    public void OnPointerClick(PointerEventData eData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;

            // ����Ŭ���� ������ ���or����
            if (neck.activeSelf)
            {
                neck.SetActive(false);

                gameManager.armorPower -= 5;
                if (instanceImage)
                {
                    Destroy(instanceImage);

                }
            }
            else
            {
                neck.SetActive(true);
                gameManager.armorPower += 5;
                // ���â�� ������ �̹��� ����
                toolTipImage.gameObject.SetActive(false);
                instanceImage = Instantiate<Image>(instantiateImageAtEquipWindow, gameManager.neckEquip.transform);

            }
        }
        else
        {
            isDoubleClicked = false;
            doubleClickedTime = Time.time;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTipImage.gameObject.SetActive(true);
        // �̹����� hierarchy ��������ΰ��� ���̵���
        priorityImage.transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipImage.gameObject.SetActive(false);

    }
}
