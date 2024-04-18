using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class GlobalClassIsContinue
{
    public static bool isContinue = false; //��������
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
        // ���������� �ҷ����� �Ҽ��ֵ��� �������� true�� ����
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
        Application.Quit(); // ���ø����̼� ����
#endif
    }

}
