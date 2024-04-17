using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShoulderEquip : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;


    public GameObject shoulder;
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
        shoulder = playermoveScript.equippedShoulder;

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

            // ����Ŭ���� ������ ���or����
            if (shoulder.activeSelf)
            {
                shoulder.SetActive(false);
                gameManager.armorPower -= 5;
                if (gameManager.shoulderEquip.transform.GetChild(2).gameObject)
                {
                    Destroy(gameManager.shoulderEquip.transform.GetChild(2).gameObject);
                    if (gameManager.shoulderEquip.transform.childCount >= 4)
                    {
                        Destroy(gameManager.shoulderEquip.transform.GetChild(3).gameObject);
                    }
                }
            }
            else
            {
                shoulder.SetActive(true);
                gameManager.armorPower += 5;
                // ���â�� ������ �̹��� ����
                toolTipImage.gameObject.SetActive(false);
                instanceImage = Instantiate<Image>(instantiateImageAtEquipWindow, gameManager.shoulderEquip.transform);

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
        // �̹����� hierarchy ��������ΰ��� ���̵���
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
