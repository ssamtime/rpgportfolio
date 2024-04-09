using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnterCinemachine : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cineVirtualCamera;
    [SerializeField] GameObject trackAndCart;

    PlayerMove _PlayerMoveScript;

    void Start()
    {
        _PlayerMoveScript =GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            TurnOnCamera();
        }
    }

    void TurnOnCamera()
    {
        Stop();
        trackAndCart.gameObject.SetActive(true);
        cineVirtualCamera.gameObject.SetActive(true);
        Invoke("TurnOffVirtualCamera", 4f);
    }

    void TurnOffVirtualCamera()
    {
        Camera.main.transform.localEulerAngles = new Vector3(0, 0, 0);
        Camera.main.transform.position = new Vector3(0, 0, -10);
        cineVirtualCamera.gameObject.SetActive(false);
        InputAllow();
    }

    public void Stop()
    {
        _PlayerMoveScript.inputAllow = false;
        _PlayerMoveScript.walkspeed = 0f;
        _PlayerMoveScript.runSpeed = 0f;
    }
    public void InputAllow()
    {
        _PlayerMoveScript.inputAllow = true;
        _PlayerMoveScript.walkspeed = 4f;
        _PlayerMoveScript.runSpeed = 8f;
    }
}
