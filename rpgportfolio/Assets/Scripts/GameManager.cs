using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image[] InventorySlots;

    public List<int> inventorySlotList = new List<int>();
    public int listIndex;
    public int haveMoney;

    public bool blockClick;

    public Text haveMoneyText;

    public Image swordEquip;
    public Image shieldEquip;
    public Image tassetEquip;
    public Image bootsEquip;
    public Image neckEquip;
    public Image shoulderEquip;
    //public Image hatEquip;
    //public Image necklaceEquip;

    public Image instantiateImageAtInven;
    public int itemPrice;
    public string itemNameText;

    public int sellPrice;
    public string sellItemNameString;

    public Image swordToolTipImage;

    public int playerHP;
    public int playerMaxHP;
    public int playerMP;
    public int playerMaxMP;

    public int attackPower;
    public int armorPower;
        
    public int useItemIndex;
    public int[] useItemAmountArray;

    public bool merchantNPCturn;
    public bool blackSmithNPCturn;

    public bool canScreenRotate;

    void Start()
    {
        blockClick = false;
        haveMoney = 500;
        listIndex = 0;

        playerHP = 100;
        playerMaxHP = 100;
        playerMP = 100;
        playerMaxMP = 100;

        attackPower = 10;
        armorPower = 0;

        canScreenRotate = true;

        
        useItemAmountArray = new int[10];
        useItemAmountArray[1] = 0;
        useItemAmountArray[2] = 0;
        useItemAmountArray[3] = 0;
        //useItemAmountArray[1] == redPotionAmount; 
        //useItemAmountArray[2] == bluePotionAmount;
        //useItemAmountArray[3] == ElixirAmount;    
    }

    void Update()
    {
        haveMoneyText.text=haveMoney.ToString();

    }
}
