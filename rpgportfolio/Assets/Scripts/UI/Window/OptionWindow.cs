using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionWindow : MonoBehaviour
{
    [SerializeField] Image optionWindow;

    void Start()
    {        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(optionWindow.gameObject.activeSelf == true)
                optionWindow.gameObject.SetActive(false);
            else
                optionWindow.gameObject.SetActive(true);
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
}
