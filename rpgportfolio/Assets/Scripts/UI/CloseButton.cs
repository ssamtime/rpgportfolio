using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    public Image closeImage;

    public void CloseWindow()
    {
        closeImage.gameObject.SetActive(false);
    }
}
