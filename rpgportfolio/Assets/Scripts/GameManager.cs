using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Image[] InventorySlots;

    public List<int> inventorySlotList = new List<int>();
    public int index;
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

    void Start()
    {
        blockClick = false;
        haveMoney = 500;
    }

    void Update()
    {
        haveMoneyText.text=haveMoney.ToString();
    }
}
