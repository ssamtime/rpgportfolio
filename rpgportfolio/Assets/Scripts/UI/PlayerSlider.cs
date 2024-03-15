using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlider : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] Slider slider_HPBar;
    [SerializeField] Slider slider_MPBar;
    [SerializeField] Text text_HPText;
    [SerializeField] Text text_MPText;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //slider_HPBar.maxValue = gameManager.playerMaxHP;
        //slider_MPBar.maxValue = gameManager.playerMaxMP;
        
    }


    void Update()
    {
        slider_HPBar.value = (float)gameManager.playerHP/ gameManager.playerMaxHP;
        slider_MPBar.value = (float)gameManager.playerMP/ gameManager.playerMaxMP;

        text_HPText.text = gameManager.playerHP.ToString() + " / " + gameManager.playerMaxHP.ToString();
        text_MPText.text = gameManager.playerMP.ToString() + " / " + gameManager.playerMaxMP.ToString();
    }
}
