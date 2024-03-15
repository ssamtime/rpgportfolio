using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;

// �÷��̾� �̺�Ʈ�Լ��� ��Ƴ�����

public class PlayerEventFunction : MonoBehaviour
{
    public float punchRange;
    public int punchDamage;
    public float swordRange;
    public int swordDamage;

    GameObject player;
    PlayerMove _PlayerMoveScript;
    Rigidbody _PlayerRigidbody;
    

    void Start()
    {
        punchRange = 0.5f;
        punchDamage = 10;
        swordRange = 1.6f;
        swordDamage = 40;

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
    public void SwordDamageEvent()
    {
        // �� Ž��
        RaycastHit hit2;
        // �÷��̾� �ָ���ġ���� ���� �߻�

        if (Physics.Raycast(transform.position + (transform.forward * 0.2f) + new Vector3(0, 1.5f, 0),
            transform.forward, out hit2, swordRange))
        {
            EnemyFSM enemyFSM = hit2.collider.GetComponent<EnemyFSM>();
            if (enemyFSM != null)
            {
                transform.LookAt(hit2.transform);
                enemyFSM.HitEnemy(swordDamage);
            }
        }
    }

    public void JumpEnd()
    {
        _PlayerMoveScript.isJump = false;

        _PlayerMoveScript.inputAllow = true;
        _PlayerMoveScript.walkspeed = 4f;
        _PlayerMoveScript.runSpeed = 8f;
    }

    public void PunchStop()
    {
        _PlayerMoveScript.walkspeed = 0f;
        _PlayerMoveScript.runSpeed = 0f;

        _PlayerMoveScript.inputAllow = false;
    }
    public void SwordStop()
    {
        _PlayerMoveScript.walkspeed = 0f;
        _PlayerMoveScript.runSpeed = 0f;

        _PlayerMoveScript.inputAllow = false;
    }
    public void Stop()
    {
        _PlayerMoveScript.walkspeed = 0f;
        _PlayerMoveScript.runSpeed = 0f;

        _PlayerMoveScript.inputAllow = false;
    }

    // ���ݾִϸ��̼� �������� ����Ǵ� �̺�Ʈ�Լ�
    public void InputAllow()
    {
        _PlayerMoveScript.inputAllow = true;
        _PlayerMoveScript.walkspeed = 4f;
        _PlayerMoveScript.runSpeed = 8f;
    }


}
