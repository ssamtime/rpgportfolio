using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopNPC : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Outline outlineScript;
    public Image shopImage;
    GameManager gameManager;

    void Start()
    {
        outlineScript = GetComponent<Outline>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    void Update()
    {        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outlineScript.enabled = !outlineScript.enabled;

        gameManager.blockClick = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outlineScript.enabled = !outlineScript.enabled;
        gameManager.blockClick = false;
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        shopImage.gameObject.SetActive(true);
    }
}
