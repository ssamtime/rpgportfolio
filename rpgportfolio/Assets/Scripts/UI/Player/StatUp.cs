using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUp : MonoBehaviour
{
    GameManager gameManager;
    GameObject statUpButtonsObject;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        statUpButtonsObject = transform.Find("EquipWindow").Find("StatUpImages").gameObject;
        
    }
    private void Update()
    {
        if (gameManager.canStatUpClick >=1)
        {
            statUpButtonsObject.SetActive(true);// 자식찾아서 이름으로 캔버스에 스크립트달기
        }
    }

    public void AttackPowerStatUp()
    {        
        gameManager.attackPower += 5;

        gameManager.canStatUpClick -=1;
        if (gameManager.canStatUpClick <= 0)
        {
            statUpButtonsObject.SetActive(false);
        }
    }
    public void ArmorPowerStatUp()
    {
        gameManager.armorPower += 5;

        gameManager.canStatUpClick -= 1;
        if (gameManager.canStatUpClick <= 0)
        {
            statUpButtonsObject.SetActive(false);
        }
    }
    public void HPStatUp()
    {
        gameManager.playerMaxHP += 10;
        gameManager.playerHP += 10;

        gameManager.canStatUpClick -= 1;
        if (gameManager.canStatUpClick <= 0)
        {
            statUpButtonsObject.SetActive(false);
        }
    }
}
