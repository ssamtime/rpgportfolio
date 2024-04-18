using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Runtime.ConstrainedExecution;
using System.Reflection;
using UnityEngine.Assertions.Comparers;
using Unity.VisualScripting;

[System.Serializable]
public class SavedData
{
    public int haveMoney;
    public int playerHP;
    public int playerMaxHP;
    public int playerMP;
    public int playerMaxMP;
    public float playerEXP;
    public float playerMaxEXP;
    public int playerLevel;
    public int canStatUpClick;
    public int attackPower;
    public int armorPower;

    public bool isEquippedShield;
    public bool isEquippedSword;
    public bool isEquippedNeck;
    public bool isEquippedShoulder;
    public bool isEquippedTasset;
    public bool isEquippedBoots;

    public bool isSwordImage;
    public bool isShieldImage;
    public bool isTassetImage;
    public bool isBootsImage;
    public bool isNeckImage;
    public bool isShoulderImage;

    public int[] inventorySlotNumber = new int[15];
    public string[] inventoryItemName = new string[15];
    public int[] inventoryUseItemAmount = new int[15];
    public int[] inventoryUseItemPotionType = new int[15];

    public int[] quickSlotNumber = new int[4];
    public string[] quickSlotItemName = new string[4];
    //public int[] quickSlotUseItemAmount = new int[4];
    //public int[] quickSlotUseItemPotionType = new int[4];


}

public class DataManager : MonoBehaviour
{
    //�̱���
    public static DataManager instance;
    
    public SavedData savedData = new SavedData();

    GameManager gameManager;
    PlayerMove playermoveScript;

    string savePath;
    string saveFileName = "SaveFile";

    // ���â�� ������ �̹���
    [SerializeField] GameObject swordImagePrefab;
    [SerializeField] GameObject shieldImagePrefab;
    [SerializeField] GameObject tassetImagePrefab;
    [SerializeField] GameObject bootsImagePrefab;
    [SerializeField] GameObject neckImagePrefab;
    [SerializeField] GameObject shoulderImagePrefab;
    [SerializeField] GameObject redPotionImagePrefab;
    [SerializeField] GameObject bluePotionImagePrefab;
    [SerializeField] GameObject elixirImagePrefab;

    [SerializeField] Image inventorySlots;
    [SerializeField] GameObject quickSlots;

    [SerializeField] GameObject fireBallSkillPrefab;
    [SerializeField] GameObject iceRangeSkillPrefab;
    
                                
    private void Awake()
    {
        #region �̱���
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();        
        playermoveScript = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();

        savePath = Application.persistentDataPath + "/";

        savedData.inventorySlotNumber = new int[15];
        savedData.inventoryItemName = new string[15];

        savedData.quickSlotNumber = new int[4];
        savedData.quickSlotItemName = new string[4];

        // �κ������ �̾��ϱ⸦ ��������� ����
        if(GlobalClassIsContinue.isContinue == true)
        {
            LoadData();
        }
    }

    public void SaveData()
    {
        AssignToDataManager();
        string saveData = JsonUtility.ToJson(savedData);
        File.WriteAllText(savePath + saveFileName, saveData);
        print(savePath);
    }

    public void LoadData()
    {
        print("�ҷ�����");
        if (File.Exists(savePath + saveFileName) == false)
        {
            print("����� �����Ͱ� �����ϴ�");
        }
        string loadData = File.ReadAllText(savePath + saveFileName);
        savedData = JsonUtility.FromJson<SavedData>(loadData);
        AssignFromDataManager();
    }

    void AssignToDataManager()
    {
        // ��������
        savedData.haveMoney = gameManager.haveMoney;
        savedData.playerHP = gameManager.playerHP;
        savedData.playerMaxHP = gameManager.playerMaxHP;
        savedData.playerMP = gameManager.playerMP;
        savedData.playerMaxMP = gameManager.playerMaxMP;
        savedData.playerEXP = gameManager.playerEXP;
        savedData.playerMaxEXP = gameManager.playerMaxEXP;
        savedData.playerLevel = gameManager.playerLevel;
        savedData.canStatUpClick = gameManager.canStatUpClick;
        savedData.attackPower = gameManager.attackPower;
        savedData.armorPower = gameManager.armorPower;

        // ������� ��������
        savedData.isEquippedShield = playermoveScript.equippedShield.activeSelf;
        savedData.isShieldImage = playermoveScript.equippedShield.activeSelf;
        savedData.isEquippedSword = playermoveScript.equippedSword.activeSelf;
        savedData.isSwordImage = playermoveScript.equippedSword.activeSelf;
        savedData.isEquippedNeck = playermoveScript.equippedNeck.activeSelf;
        savedData.isNeckImage = playermoveScript.equippedNeck.activeSelf;
        savedData.isEquippedShoulder = playermoveScript.equippedShoulder.activeSelf;
        savedData.isShoulderImage = playermoveScript.equippedShoulder.activeSelf;
        savedData.isEquippedTasset = playermoveScript.equippedTasset.activeSelf;
        savedData.isTassetImage = playermoveScript.equippedTasset.activeSelf;
        savedData.isEquippedBoots = playermoveScript.equippedBoots.activeSelf;
        savedData.isBootsImage = playermoveScript.equippedBoots.activeSelf;

        // �κ��丮 ����
        for(int i=0; i<15; i++)
        {
            if (inventorySlots.transform.GetChild(i).childCount == 2)
            {
                savedData.inventorySlotNumber[i] = inventorySlots.transform.GetChild(i).GetChild(1).
                    GetComponent<SlotNumber>().slotNumber;
                savedData.inventoryItemName[i] = inventorySlots.transform.GetChild(i).GetChild(1).name;
                if (inventorySlots.transform.GetChild(i).GetChild(1).childCount == 2)
                {
                    savedData.inventoryUseItemAmount[i] = gameManager.useItemAmountArray[
                        inventorySlots.transform.GetChild(i).GetChild(1).GetComponent<PotionUse>().potiontype];
                    savedData.inventoryUseItemPotionType[i] =
                        inventorySlots.transform.GetChild(i).GetChild(1).GetComponent<PotionUse>().potiontype;
                }
            }
        }
        // ������ ����
        for (int i=0; i<4; i++)
        {
            if(quickSlots.transform.GetChild(i).childCount ==2)
            {
                savedData.quickSlotNumber[i] = quickSlots.transform.GetChild(i).GetChild(1).
                    GetComponent<SlotNumber>().slotNumber;
                savedData.quickSlotItemName[i] = quickSlots.transform.GetChild(i).GetChild(1).name;
                //if (quickSlots.transform.GetChild(i).GetChild(1).childCount == 2)
                //{
                //    savedData.quickSlotNumber[i] = gameManager.useItemAmountArray[
                //        quickSlots.transform.GetChild(i).GetChild(1).GetComponent<PotionUse>().potiontype];
                //    savedData.quickSlotUseItemPotionType[i] =
                //        quickSlots.transform.GetChild(i).GetChild(1).GetComponent<PotionUse>().potiontype;
                //}
            }
        }

    }

    void AssignFromDataManager()
    {
        gameManager.haveMoney = savedData.haveMoney;
        gameManager.playerHP = savedData.playerHP;
        gameManager.playerMaxHP = savedData.playerMaxHP;
        gameManager.playerMP = savedData.playerMP;
        gameManager.playerMaxMP = savedData.playerMaxMP; 
        gameManager.playerEXP = savedData.playerEXP;
        gameManager.playerMaxEXP = savedData.playerMaxEXP;
        gameManager.playerLevel = savedData.playerLevel;
        gameManager.canStatUpClick = savedData.canStatUpClick;
        gameManager.attackPower = savedData.attackPower;
        gameManager.armorPower = savedData.armorPower;

        playermoveScript.equippedShield.SetActive(savedData.isEquippedShield);
        playermoveScript.equippedSword.SetActive(savedData.isEquippedSword);
        playermoveScript.equippedNeck.SetActive(savedData.isEquippedNeck);
        playermoveScript.equippedShoulder.SetActive(savedData.isEquippedShoulder);
        playermoveScript.equippedTasset.SetActive(savedData.isEquippedTasset);
        playermoveScript.equippedBoots.SetActive(savedData.isEquippedBoots);

        // ��� ������ϰ� ���嵥���Ͱ� �����߾������ ����
        if (gameManager.swordEquip.transform.childCount == 2 && savedData.isSwordImage)
        {
            Instantiate<GameObject>(swordImagePrefab, gameManager.swordEquip.transform);
            playermoveScript._animator.runtimeAnimatorController = playermoveScript.swordOverrideAnimator;
        }
        // ��� �����߰� ���嵥���Ͱ� ������ߴ���� ����
        else if(gameManager.swordEquip.transform.childCount == 3 && !savedData.isSwordImage)
            Destroy(gameManager.swordEquip.transform.GetChild(2).gameObject); 
        if (gameManager.shieldEquip.transform.childCount == 2 && savedData.isShieldImage)
            Instantiate<GameObject>(shieldImagePrefab, gameManager.shieldEquip.transform);
        else if (gameManager.shieldEquip.transform.childCount == 3 && !savedData.isShieldImage)
            Destroy(gameManager.shieldEquip.transform.GetChild(2).gameObject);
        if (gameManager.neckEquip.transform.childCount == 2 && savedData.isNeckImage)
            Instantiate<GameObject>(neckImagePrefab, gameManager.neckEquip.transform);
        else if (gameManager.neckEquip.transform.childCount == 3 && !savedData.isNeckImage)
            Destroy(gameManager.neckEquip.transform.GetChild(2).gameObject);
        if (gameManager.shoulderEquip.transform.childCount == 2 && savedData.isShoulderImage)
            Instantiate<GameObject>(shoulderImagePrefab, gameManager.shoulderEquip.transform);
        else if (gameManager.shoulderEquip.transform.childCount == 3 && !savedData.isShoulderImage)
            Destroy(gameManager.shoulderEquip.transform.GetChild(2).gameObject);
        if (gameManager.tassetEquip.transform.childCount == 2 && savedData.isTassetImage)
            Instantiate<GameObject>(tassetImagePrefab, gameManager.tassetEquip.transform);
        else if (gameManager.tassetEquip.transform.childCount == 3 && !savedData.isTassetImage)
            Destroy(gameManager.tassetEquip.transform.GetChild(2).gameObject);
        if (gameManager.bootsEquip.transform.childCount == 2 && savedData.isBootsImage)
            Instantiate<GameObject>(bootsImagePrefab, gameManager.bootsEquip.transform);
        else if (gameManager.bootsEquip.transform.childCount == 3 && !savedData.isBootsImage)
            Destroy(gameManager.bootsEquip.transform.GetChild(2).gameObject);

        // ���� hierachy �̸��� ����
        SortInventroy();
        SortQuickSlot();

        // ���Կ� ������ �����ϱ��� ���Կ� �ִ� ������ ����
        for(int i=0; i<15;i++)
        {
            if (inventorySlots.transform.GetChild(i).childCount == 2)
                Destroy(inventorySlots.transform.GetChild(i).GetChild(1).gameObject);
        }
        // �κ��丮�� ������ ����
        for (int i = 0; i < 15; i++)
        {            
            if (savedData.inventoryItemName[i] !="")
            {
                if (savedData.inventoryItemName[i] == "Image_Boots(Clone)")
                    Instantiate<GameObject>(bootsImagePrefab, inventorySlots.transform.GetChild(savedData.inventorySlotNumber[i]).transform);
                else if (savedData.inventoryItemName[i] == "Image_Neck(Clone)")
                    Instantiate<GameObject>(neckImagePrefab, inventorySlots.transform.GetChild(savedData.inventorySlotNumber[i]).transform);
                else if (savedData.inventoryItemName[i] == "Image_Shield 1(Clone)")
                    Instantiate<GameObject>(shieldImagePrefab, inventorySlots.transform.GetChild(savedData.inventorySlotNumber[i]).transform);
                else if (savedData.inventoryItemName[i] == "Image_Shoulder(Clone)")
                    Instantiate<GameObject>(shoulderImagePrefab, inventorySlots.transform.GetChild(savedData.inventorySlotNumber[i]).transform);
                else if (savedData.inventoryItemName[i] == "Image_Sword(Clone)")
                    Instantiate<GameObject>(swordImagePrefab, inventorySlots.transform.GetChild(savedData.inventorySlotNumber[i]).transform);
                else if (savedData.inventoryItemName[i] == "Image_Tasset(Clone)")
                    Instantiate<GameObject>(tassetImagePrefab, inventorySlots.transform.GetChild(savedData.inventorySlotNumber[i]).transform);
                else if (savedData.inventoryItemName[i] == "Image_RedPotion(Clone)")
                    Instantiate<GameObject>(redPotionImagePrefab, inventorySlots.transform.GetChild(savedData.inventorySlotNumber[i]).transform);
                else if (savedData.inventoryItemName[i] == "Image_BluePotion(Clone)")
                    Instantiate<GameObject>(bluePotionImagePrefab, inventorySlots.transform.GetChild(savedData.inventorySlotNumber[i]).transform);
                else if (savedData.inventoryItemName[i] == "Image_Elixir(Clone)")
                    Instantiate<GameObject>(elixirImagePrefab, inventorySlots.transform.GetChild(savedData.inventorySlotNumber[i]).transform);
                
            }
        }

        // ���Կ� ������ �����ϱ��� ���Կ� �ִ� ������ ����
        for (int i = 0; i < 4; i++)
        {
            if (quickSlots.transform.GetChild(i).childCount == 2)
                Destroy(quickSlots.transform.GetChild(i).GetChild(1).gameObject);
        }
        // �����Կ� ������ ����
        for (int i=0; i<4;i++)
        {
            if (savedData.quickSlotItemName[i] != "")
            {
                if (savedData.quickSlotItemName[i] == "Image_Boots(Clone)")
                    Instantiate<GameObject>(bootsImagePrefab, quickSlots.transform.GetChild(savedData.quickSlotNumber[i]).transform);
                else if (savedData.quickSlotItemName[i] == "Image_Neck(Clone)")
                    Instantiate<GameObject>(neckImagePrefab, quickSlots.transform.GetChild(savedData.quickSlotNumber[i]).transform);
                else if (savedData.quickSlotItemName[i] == "Image_Shield 1(Clone)")
                    Instantiate<GameObject>(shieldImagePrefab, quickSlots.transform.GetChild(savedData.quickSlotNumber[i]).transform);
                else if (savedData.quickSlotItemName[i] == "Image_Shoulder(Clone)")
                    Instantiate<GameObject>(shoulderImagePrefab, quickSlots.transform.GetChild(savedData.quickSlotNumber[i]).transform);
                else if (savedData.quickSlotItemName[i] == "Image_Sword(Clone)")
                    Instantiate<GameObject>(swordImagePrefab, quickSlots.transform.GetChild(savedData.quickSlotNumber[i]).transform);
                else if (savedData.quickSlotItemName[i] == "Image_Tasset(Clone)")
                    Instantiate<GameObject>(tassetImagePrefab, quickSlots.transform.GetChild(savedData.quickSlotNumber[i]).transform);
                else if (savedData.quickSlotItemName[i] == "FireBallSkill")
                    Instantiate<GameObject>(fireBallSkillPrefab, quickSlots.transform.GetChild(savedData.quickSlotNumber[i]).transform);
                else if (savedData.quickSlotItemName[i] == "IceRangeSkill")
                    Instantiate<GameObject>(iceRangeSkillPrefab, quickSlots.transform.GetChild(savedData.quickSlotNumber[i]).transform);
                else if (savedData.quickSlotItemName[i] == "FireBallSkill(Clone)")
                    Instantiate<GameObject>(fireBallSkillPrefab, quickSlots.transform.GetChild(savedData.quickSlotNumber[i]).transform);
                else if (savedData.quickSlotItemName[i] == "IceRangeSkill(Clone)")
                    Instantiate<GameObject>(iceRangeSkillPrefab, quickSlots.transform.GetChild(savedData.quickSlotNumber[i]).transform);
                else if (savedData.quickSlotItemName[i] == "Image_RedPotion(Clone)(Clone)")
                    Instantiate<GameObject>(redPotionImagePrefab, quickSlots.transform.GetChild(savedData.quickSlotNumber[i]).transform);
                else if (savedData.quickSlotItemName[i] == "Image_BluePotion(Clone)(Clone)")
                    Instantiate<GameObject>(bluePotionImagePrefab, quickSlots.transform.GetChild(savedData.quickSlotNumber[i]).transform);
                else if (savedData.quickSlotItemName[i] == "Image_Elixir(Clone)(Clone)")
                    Instantiate<GameObject>(elixirImagePrefab, quickSlots.transform.GetChild(savedData.quickSlotNumber[i]).transform);

            }
        }
        for(int i =0;i<15;i++)
        {
            gameManager.useItemAmountArray[savedData.inventoryUseItemPotionType[i]] = savedData.inventoryUseItemAmount[i];
        }
        //for (int i = 0; i < 4; i++)
        //{
        //    gameManager.useItemAmountArray[savedData.quickSlotUseItemPotionType[i]] = savedData.quickSlotUseItemAmount[i];
        //}
    }

    void SortInventroy()
    {
        // �̸�������� hierachy ���� 

        int inventoryOffed = -1;
        if(inventorySlots.transform.parent.gameObject.activeSelf==false)
        {
            inventoryOffed = 1;
            inventorySlots.transform.parent.gameObject.SetActive(true);
        }
            inventorySlots.transform.parent.gameObject.SetActive(true);
        // ��� �κ��丮 ������ �����ɴϴ�.
        Transform[] slots = new Transform[inventorySlots.transform.childCount];
        for (int i = 0; i < inventorySlots.transform.childCount; i++)
        {
            slots[i] = inventorySlots.transform.GetChild(i);
        }

        // ������ �����մϴ�.
        Array.Sort(slots, CompareSlotName);

        // ���ĵ� ������� ������ ��ġ�� �����մϴ�.
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSiblingIndex(i);
        }

        if(inventoryOffed==1)
            inventorySlots.transform.parent.gameObject.SetActive(false);

    }

    void SortQuickSlot()
    {
        // �̸�������� hierachy ���� 

        // ��� �� ������ �����ɴϴ�.
        Transform[] slots = new Transform[quickSlots.transform.childCount];
        for (int i = 0; i < quickSlots.transform.childCount; i++)
        {
            slots[i] = quickSlots.transform.GetChild(i);
        }

        // ������ �����մϴ�.
        Array.Sort(slots, CompareSlotName);

        // ���ĵ� ������� ������ ��ġ�� �����մϴ�.
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSiblingIndex(i);
        }
    }

    // �̸��� �������� Transform�� ���ϴ� �޼���
    int CompareSlotName(Transform a, Transform b)
    {
        string nameA = a.name;
        string nameB = b.name;
        return string.Compare(nameA, nameB);
    }

}
