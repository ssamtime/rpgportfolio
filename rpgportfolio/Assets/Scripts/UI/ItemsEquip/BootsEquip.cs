using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BootsEquip : MonoBehaviour, IPointerClickHandler
{
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;


    public GameObject boots;
    GameObject player;
    PlayerMove playermoveScript;

    GameManager gameManager;
    public Image instantiateImageAtInven;
    Image instanceImage;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playermoveScript = player.GetComponent<PlayerMove>();
        boots = playermoveScript.equippedBoots;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnPointerClick(PointerEventData eData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;

            // ����Ŭ���� ������ ���or����
            if (boots.activeSelf)
            {
                boots.SetActive(false);
                if (instanceImage)
                {
                    Destroy(instanceImage);

                }
            }
            else
            {
                boots.SetActive(true);
                instanceImage = Instantiate<Image>(instantiateImageAtInven, gameManager.bootsEquip.transform);

            }
        }
        else
        {
            isDoubleClicked = false;
            doubleClickedTime = Time.time;
        }
    }
}
