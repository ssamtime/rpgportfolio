using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemEquip : MonoBehaviour, IPointerClickHandler
{
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;

    public GameObject equipObject;

    void Start()
    {
        GameObject.FindGameObjectWithTag("Sword");
    }

    public void OnPointerClick(PointerEventData eData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;

            // ����Ŭ���� ������ ���or����
            if(equipObject.activeSelf)
            {
                equipObject.SetActive(false);
            }
            else
            {
                equipObject.SetActive(true);
            }
        }
        else
        {
            isDoubleClicked = false;
            doubleClickedTime = Time.time;
        }
    }
}
