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

    AudioSource audioSource;
    [SerializeField] AudioClip swordEquipAC;
    public int slotNumber;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playermoveScript = player.GetComponent<PlayerMove>();
        sword = playermoveScript.equippedSword;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        priorityImage = transform.parent.gameObject;

        audioSource = transform.GetComponent<AudioSource>();
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
                gameManager.attackPower -= 30;
                // 장비창에 아이템 이미지 삭제
                if (gameManager.swordEquip.transform.GetChild(2).gameObject)
                {
                    Destroy(gameManager.swordEquip.transform.GetChild(2).gameObject);
                    if (gameManager.swordEquip.transform.childCount >=4)
                    {
                        Destroy(gameManager.swordEquip.transform.GetChild(3).gameObject);
                    }
                }
            }
            else
            {
                sword.SetActive(true);
                playermoveScript._animator.runtimeAnimatorController = playermoveScript.swordOverrideAnimator;
                gameManager.attackPower += 30;

                // 장비창에 아이템 이미지 생성
                toolTipImage.gameObject.SetActive(false);
                instanceImage = Instantiate<Image>(instantiateImageAtEquipWindow, gameManager.swordEquip.transform);

                audioSource.PlayOneShot(swordEquipAC);
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
        // 이미지가 hierarchy 가장밑으로가서 보이도록
        priorityImage = transform.parent.gameObject;
        priorityImage.transform.SetAsLastSibling();

        transform.parent.parent.transform.SetAsLastSibling();
        toolTipImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipImage.gameObject.SetActive(false);

    }
}
