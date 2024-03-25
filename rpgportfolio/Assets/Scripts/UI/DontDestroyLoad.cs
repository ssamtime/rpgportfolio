using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyLoad : MonoBehaviour
{
    private void Awake()
    {
        var obj = FindObjectsOfType<DontDestroyLoad>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            //destroy(this)?
        }
    }

}
