using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopNPC : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Outline outlineScript;

    void Start()
    {
        outlineScript = GetComponent<Outline>();
    }


    void Update()
    {        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outlineScript.enabled = !outlineScript.enabled;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        outlineScript.enabled = !outlineScript.enabled;
    }

    public void OnPointerClick(PointerEventData eventData) 
    {

    }
}
