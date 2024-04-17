using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ClickSceneMove : MonoBehaviour, IPointerClickHandler
{
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;

    [SerializeField] int placeNum=-1;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;

            // 더블클릭시

            gameManager.blockClick = false;

            if (gameManager == null)
                Debug.Log("게임매니저 못찾음");
            gameManager.canScreenRotate = true;

            if (placeNum==1)
                //SceneManager.LoadScene("Dungeon1");
                SceneLoader.Instance.LoadScene("Dungeon1");
            else if(placeNum==2)
                SceneManager.LoadScene("Dungeon2");

            transform.parent.parent.gameObject.SetActive(false);

        }
        else
        {
            isDoubleClicked = false;
            doubleClickedTime = Time.time;
        }
    }
}
