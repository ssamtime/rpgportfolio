using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShieldEquip : MonoBehaviour, IPointerClickHandler
{
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;


    public GameObject shiled;
    GameObject player;
    PlayerMove playermoveScript;

    GameManager gameManager;
    public Image instantiateImageAtInven;
    Image instanceImage;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playermoveScript = player.GetComponent<PlayerMove>();
        shiled = playermoveScript.equippedShield;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnPointerClick(PointerEventData eData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;

            // 더블클릭시 아이템 장비or해제
            if (shiled.activeSelf)
            {
                shiled.SetActive(false); 
                if (instanceImage)
                {
                    Destroy(instanceImage);//<--??

                }
            }
            else
            {
                shiled.SetActive(true);
                instanceImage = Instantiate<Image>(instantiateImageAtInven, gameManager.shieldEquip.transform);

            }
        }
        else
        {
            isDoubleClicked = false;
            doubleClickedTime = Time.time;
        }
    }
}
