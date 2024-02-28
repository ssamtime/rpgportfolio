using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;

public class PlayerEventFunction : MonoBehaviour
{
    public float punchRange;
    public int punchDamage;

    GameObject player;
    PlayerMove _PlayerMoveScript;
    Rigidbody _PlayerRigidbody;
    

    void Start()
    {
        punchRange = 0.5f;
        punchDamage = 10;

        player = GameObject.FindWithTag("Player");
        _PlayerMoveScript = player.GetComponent<PlayerMove>();
        _PlayerRigidbody = player.GetComponent<Rigidbody>();
    }

    void Update()
    {        
    }

    // �ָԳ������� ����Ǵ� �ִϸ��̼� �̺�Ʈ
    public void PunchDamageEvent()
    {
        // �� Ž��
        RaycastHit hit2;
        // �÷��̾� �ָ���ġ���� ���� �߻�

        if (Physics.Raycast(transform.position + (transform.forward * 0.2f) + new Vector3(0, 1.5f, 0),
            transform.forward, out hit2, punchRange))
        {
            EnemyFSM enemyFSM = hit2.collider.GetComponent<EnemyFSM>();
            if (enemyFSM != null)
            {
                transform.LookAt(hit2.transform);
                enemyFSM.HitEnemy(punchDamage);
            }
        }        

    }

    public void JumpEnd()
    {
        _PlayerMoveScript.isJump = false;
    }

    public void Stop()
    {
        _PlayerMoveScript.walkspeed = 0f;
        _PlayerMoveScript.runSpeed = 0f;

        // 0.5�ʵ��� �Է¸��޵���
        _PlayerMoveScript.inputAllow = false;
        Invoke("InputAllow", 0.7f);
    }

    public void InputAllow()
    {
        _PlayerMoveScript.inputAllow = true;
        _PlayerMoveScript.walkspeed = 4f;
        _PlayerMoveScript.runSpeed = 8f;
    }

}
