using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemPurchaseName : MonoBehaviour
{
    GameManager gameManager;
    public Text purchaseItemName;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }


    void LateUpdate()
    {
        purchaseItemName.text = gameManager.itemNameText + " �����Ͻðڽ��ϱ�?";
    }
}
