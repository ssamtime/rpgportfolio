using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TassetEquip : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;


    public GameObject tasset;
    GameObject player;
    PlayerMove playermoveScript;

    GameManager gameManager;
    public Image instantiateImageAtEquipWindow;
    Image instanceImage;

    [SerializeField] Image toolTipImage;
    GameObject priorityImage;
    AudioSource audioSource;
    [SerializeField] AudioClip EquipAC;
    public int slotNumber;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playermoveScript = player.GetComponent<PlayerMove>();
        tasset = playermoveScript.equippedTasset;

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

            // 더블클릭시 아이템 장비or해제
            if (tasset.activeSelf)
            {
                tasset.SetActive(false);
                gameManager.armorPower -= 5;
                // 장비창 이미지 삭제
                if (gameManager.tassetEquip.transform.GetChild(2).gameObject)
                {
                    Destroy(gameManager.tassetEquip.transform.GetChild(2).gameObject);

                }
            }
            else
            {
                tasset.SetActive(true);
                gameManager.armorPower += 5;
                // 장비창에 아이템 이미지 생성
                toolTipImage.gameObject.SetActive(false);
                instanceImage = Instantiate<Image>(instantiateImageAtEquipWindow, gameManager.tassetEquip.transform);

                audioSource.PlayOneShot(EquipAC);
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
