using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    public Image closeImage;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void CloseWindow()
    {
        closeImage.gameObject.SetActive(false);
        gameManager.merchantNPCturn = false;
        gameManager.blackSmithNPCturn = false;
        gameManager.sceneNPCturn = false;

        gameManager.canScreenRotate = true;
        gameManager.blockClick = false;
    }
}
