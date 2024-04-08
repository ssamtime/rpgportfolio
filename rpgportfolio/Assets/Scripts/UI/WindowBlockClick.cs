using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowBlockClick : MonoBehaviour
{
    GameManager gameManager;


    [SerializeField] Image ShopWindow;
    [SerializeField] Image PotionShopWindow;
    [SerializeField] Image EquipWindow;
    [SerializeField] Image Inventory;
    [SerializeField] Image MoveToDungeon;
    [SerializeField] Image optionWindow;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    void Update()
    {
        if(ShopWindow.gameObject.activeSelf == true||
            PotionShopWindow.gameObject.activeSelf == true ||
            EquipWindow.gameObject.activeSelf == true ||
            Inventory.gameObject.activeSelf == true ||
            MoveToDungeon.gameObject.activeSelf == true ||
            optionWindow.gameObject.activeSelf == true) 
        { 
            gameManager.blockClick=true;
            gameManager.canScreenRotate = false;
        }

    }
}
