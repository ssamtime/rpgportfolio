using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{

    GameManager gameManager;

    [SerializeField] Text attackPowerText;
    [SerializeField] Text armorPowerText;
    [SerializeField] Text playerMaxHPText;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    
    void Update()
    {
        attackPowerText.text = gameManager.attackPower.ToString();
        armorPowerText.text = gameManager.armorPower.ToString();
        playerMaxHPText.text = gameManager.playerMaxHP.ToString();
    }
}
