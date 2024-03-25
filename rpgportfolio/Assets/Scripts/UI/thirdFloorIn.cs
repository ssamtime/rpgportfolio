using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdFloorIn : MonoBehaviour
{
    GameManager gameManager;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            gameManager.thirdFloorIn = true;

            Debug.Log("3Ãþ¿È");
        }            
    }
}
