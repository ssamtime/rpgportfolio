using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwordEquip : MonoBehaviour, IPointerClickHandler
{
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;


    public GameObject sword;
    GameObject player;
    PlayerMove playermoveScript;

    public Image instantiateImageAtInven;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playermoveScript = player.GetComponent<PlayerMove>();
        sword = playermoveScript.equippedSword;
    }

    public void OnPointerClick(PointerEventData eData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;

            // 더블클릭시 아이템 장비or해제
            if (sword.activeSelf)
            {
                sword.SetActive(false);
                playermoveScript._animator.runtimeAnimatorController = playermoveScript.originalOverrideAnimator;
                Instantiate<Image>(instantiateImageAtInven, ImageAtEquipWindow.transform);

            }
            else
            {
                sword.SetActive(true);
                playermoveScript._animator.runtimeAnimatorController = playermoveScript.swordOverrideAnimator;
            }
        }
        else
        {
            isDoubleClicked = false;
            doubleClickedTime = Time.time;
        }
    }
}
