using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwordEquip : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;

    public GameObject sword;
    GameObject player;
    PlayerMove playermoveScript;

    GameManager gameManager;
    public Image instantiateImageAtEquipWindow;
    Image instanceImage;

    //public Image purchaseWindow;
    //public Image sellConfirmWindow;
    //[SerializeField] string sellItemName;
    //[SerializeField] int sellItemPrice;

    [SerializeField] Image toolTipImage;    
    GameObject priorityImage;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playermoveScript = player.GetComponent<PlayerMove>();
        sword = playermoveScript.equippedSword;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        priorityImage = transform.parent.gameObject;
    }

    public void OnPointerClick(PointerEventData eData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;

            //if(purchaseWindow.gameObject.activeSelf)
            //{
            //    gameManager.sellItemNameString = sellItemName;

            //    sellConfirmWindow.gameObject.SetActive(false);
            //}
            //else
            //{

            //}

            // ����Ŭ���� ������ ���or����
            if (sword.activeSelf)
            {
                sword.SetActive(false);
                playermoveScript._animator.runtimeAnimatorController = playermoveScript.originalOverrideAnimator;
                gameManager.attackPower -= 30;
                // ���â�� ������ �̹��� ����
                if (instanceImage)
                {
                    Destroy(instanceImage);
                }
            }
            else
            {
                sword.SetActive(true);
                playermoveScript._animator.runtimeAnimatorController = playermoveScript.swordOverrideAnimator;
                gameManager.attackPower += 30;

                // ���â�� ������ �̹��� ����
                toolTipImage.gameObject.SetActive(false);
                instanceImage = Instantiate<Image>(instantiateImageAtEquipWindow, gameManager.swordEquip.transform);

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
