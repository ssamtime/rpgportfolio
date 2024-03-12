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
    public Image instantiateImageAtInven;
    Image instanceImage;

    public Image purchaseWindow;
    public Image sellConfirmWindow;
    [SerializeField] string sellItemName;
    [SerializeField] int sellItemPrice;

    [SerializeField] Image toolTipImage;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playermoveScript = player.GetComponent<PlayerMove>();
        sword = playermoveScript.equippedSword;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        toolTipImage = gameManager.swordToolTipImage;
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

            // 더블클릭시 아이템 장비or해제
            if (sword.activeSelf)
            {
                sword.SetActive(false);
                playermoveScript._animator.runtimeAnimatorController = playermoveScript.originalOverrideAnimator;
                //if (instanceImage)
                //{
                //    Destroy(instanceImage); 지우면댐
                //}

            }
            else
            {
                sword.SetActive(true);
                playermoveScript._animator.runtimeAnimatorController = playermoveScript.swordOverrideAnimator;
                //instanceImage = Instantiate<Image>(instantiateImageAtInven, gameManager.swordEquip.transform);
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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipImage.gameObject.SetActive(false);

    }
}
