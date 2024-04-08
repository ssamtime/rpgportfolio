using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseItemNumber : MonoBehaviour
{
    [SerializeField] Text useItemAmount;
    [SerializeField] int useItemIndex;

    GameManager gameManager;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        useItemAmount.text = gameManager.useItemAmountArray[useItemIndex].ToString();

        // ���� ���� 0���ϵǸ� �����ϱ�
        if (gameManager.useItemAmountArray[useItemIndex] <= 0)
            Destroy(transform.parent.gameObject);
    }
}
