using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionWindow : MonoBehaviour
{
    [SerializeField] Image optionWindow;
    [SerializeField] Image soundOptionWindow;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(optionWindow.gameObject.activeSelf == true)
            {
                if (soundOptionWindow.gameObject.activeSelf == true)
                {
                    soundOptionWindow.gameObject.SetActive(false);
                    return;
                }
                else
                {
                    optionWindow.gameObject.SetActive(false);

                    gameManager.canScreenRotate = true;
                    gameManager.blockClick = false;
                }
            }
            else
            {
                optionWindow.gameObject.SetActive(true);
                optionWindow.transform.SetAsLastSibling();
            }        
        }
    }

    public void GameQuit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void ActiveSoundOptionWindow()
    {
        soundOptionWindow.gameObject.SetActive(true);
        soundOptionWindow.transform.SetAsLastSibling();
    }
}

