using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Purchase : MonoBehaviour, IPointerClickHandler
{
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;

    public Image instantiateImageAtInven;
    public Image[] InventorySlots;
    
    List<int> inventorySlotList = new List<int>();

    int index;

    void Start()
    {
        index = 0;
    }

    public void OnPointerClick(PointerEventData eData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;

            //Debug.Log("´õºíÅ¬¸¯´ï");

            inventorySlotList.Add(index);
            Instantiate<Image>(instantiateImageAtInven, InventorySlots[index].transform);
            index++;
        }
        else
        {
            isDoubleClicked = false;
            doubleClickedTime = Time.time;
        }
    }


}
