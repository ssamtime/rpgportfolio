using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class GlobalClassIsContinue
{
    public static bool isContinue = false; //전역변수
}

public class LobbyCanvas : MonoBehaviour
{

    [SerializeField] Image optionWindow;    

    void Start()
    {
    }

    public void StartFirst()
    {
        SceneLoader.Instance.LoadScene("StartScene");
    }

    public void ContinueGame()
    {
        // 다음씬에서 불러오기 할수있도록 전역변수 true로 변경
        GlobalClassIsContinue.isContinue = true;
        SceneLoader.Instance.LoadScene("StartScene");
    }

    public void ActiveSoundOptionWindow()
    {
        optionWindow.gameObject.SetActive(true);
        optionWindow.transform.SetAsLastSibling();
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
