using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscardConfirm : MonoBehaviour
{
    GameManager gameManager;

    public Image confirmWindow;

    //[SerializeField] AudioSource audioSource;
    //[SerializeField] AudioClip sellSoundAC;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (confirmWindow.gameObject.activeSelf)
        {
            // 엔터키 누르면
            if (Input.GetKeyDown(KeyCode.Return))
            {
                DiscardItem();
            }
        }
    }

    // 확인 버튼 누르면 실행되는 함수
    public void DiscardItem()
    {
        Destroy(gameManager.discardDraggedIcon.gameObject);
        confirmWindow.gameObject.SetActive(false);
    }

    public void ActiveFalseConfirmWindow()
    {
        confirmWindow.gameObject.SetActive(false);
    }
}
