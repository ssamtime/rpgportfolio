using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToVillage : MonoBehaviour
{


    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<PlayerMove>() != null)
        {
            SceneManager.LoadScene("StartScene");

        }
    }
}
