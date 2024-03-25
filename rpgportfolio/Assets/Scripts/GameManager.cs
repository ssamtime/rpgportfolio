using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image[] InventorySlots;

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
    public float playerEXP;
    public float playerMaxEXP;
    public int playerLevel;

    public int attackPower;
    public int armorPower;
    public int canStatUpClick;
        
    public int useItemIndex;
    public int[] useItemAmountArray;

    public bool merchantNPCturn;
    public bool blackSmithNPCturn;
    public bool sceneNPCturn;

    public bool canScreenRotate;

    public GameObject beginDraggedIcon;
    public Vector3 startPosition;
    public Transform startParent;

    public bool thirdFloorIn;

    void Start()
    {
        blockClick = false;
        haveMoney = 600;

        playerHP = 100;
        playerMaxHP = 100;
        playerMP = 100;
        playerMaxMP = 100;
        playerEXP=0;
        playerMaxEXP = 0;
        playerMaxEXP = 100;
        playerLevel = 1;

        attackPower = 10;
        armorPower = 0;
        canStatUpClick = 0;

        canScreenRotate = true;

        useItemAmountArray = new int[10];
        useItemAmountArray[1] = 0;
        useItemAmountArray[2] = 0;
        useItemAmountArray[3] = 0;

        thirdFloorIn = false;
    }
    private void Awake()
    {
        var obj = FindObjectsOfType<GameManager>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            //destroy(this)?
        }
    
    }
    void Update()
    {
        haveMoneyText.text=haveMoney.ToString();

    }
}
