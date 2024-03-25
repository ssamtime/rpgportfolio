using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;

// 플레이어 이벤트함수들 모아놓은곳

public class PlayerEventFunction : MonoBehaviour
{
    public float punchRange;
    public int punchDamage;
    public float swordRange;
    public int swordDamage;

    GameObject player;
    PlayerMove _PlayerMoveScript;
    Rigidbody _PlayerRigidbody;
    
    [SerializeField] GameObject DamageEffectPrefab;

    GameManager gameManager;

    AudioSource audioSource;
    [SerializeField] AudioClip playerPunchAC;
    [SerializeField] AudioClip playerSwordHitAC;
    [SerializeField] AudioClip playerPunchAirAC;
    [SerializeField] AudioClip playerSwordAirAC;

    void Start()
    {
        punchRange = 0.8f;
        punchDamage = 10;
        swordRange = 1.6f;
        swordDamage = 40;

        player = GameObject.FindWithTag("Player");
        _PlayerMoveScript = player.GetComponent<PlayerMove>();
        _PlayerRigidbody = player.GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = transform.GetComponent<AudioSource>();
    }

    void Update()
    {        
    }

    // 주먹내지를때 실행되는 애니메이션 이벤트
    public void PunchDamageEvent()
    {
        // 적 탐지
        RaycastHit hit2;
        // 플레이어 주먹위치에서 구 발사
        audioSource.PlayOneShot(playerPunchAirAC, 0.4f);

        if (Physics.SphereCast(transform.position + (transform.forward * 0.2f) + new Vector3(0, 1.5f, 0),
            0.2f,transform.forward, out hit2, punchRange))
        {
            if (hit2.collider.tag == "Enemy")
            {
                transform.LookAt(hit2.transform);
                GameObject hitEffect = Instantiate<GameObject>(DamageEffectPrefab,
                    hit2.collider.transform.position + new Vector3(0, 1.1f, 0), Quaternion.identity);
                Destroy(hitEffect, 2f);
                audioSource.PlayOneShot(playerPunchAC);
            }

            EnemyFSM enemyFSM1 = hit2.collider.GetComponent<EnemyFSM>();
            RedDragonFSM enemyFSM2 = hit2.collider.GetComponent<RedDragonFSM>();
            DarkBlueFSM enemyFSM3 = hit2.collider.GetComponent<DarkBlueFSM>();
            if (enemyFSM1 != null)
                enemyFSM1.HitEnemy(gameManager.attackPower);
            else if (enemyFSM2 != null)
                enemyFSM2.HitEnemy(gameManager.attackPower);
            else if (enemyFSM3 != null)
                enemyFSM3.HitEnemy(gameManager.attackPower);
        }
    }
    public void SwordDamageEvent()
    {
        // 적 탐지
        RaycastHit[] hits;
        // 플레이어 앞위치에서 구 발사
        audioSource.PlayOneShot(playerSwordAirAC);

        hits = Physics.SphereCastAll(transform.position + (transform.forward * 0.2f) + new Vector3(0, 1.5f, 0),
            0.7f, transform.forward, swordRange);

        
        foreach (RaycastHit colliderHit in hits)
        {
            if (colliderHit.collider.tag == "Enemy")
            {
                transform.LookAt(colliderHit.collider.transform.position);
                Debug.Log("검이펙트잇는부분실행");
                GameObject hitEffect = Instantiate<GameObject>(DamageEffectPrefab,
                    colliderHit.collider.transform.position + new Vector3(0, 1.1f, 0), Quaternion.identity);
                Destroy(hitEffect, 2f);
                audioSource.PlayOneShot(playerSwordHitAC);
            }                
                        
            EnemyFSM enemyFSM1 = colliderHit.collider.GetComponent<EnemyFSM>();
            RedDragonFSM enemyFSM2 = colliderHit.collider.GetComponent<RedDragonFSM>();
            DarkBlueFSM enemyFSM3 = colliderHit.collider.GetComponent<DarkBlueFSM>();
            if (enemyFSM1 != null)
                enemyFSM1.HitEnemy(gameManager.attackPower);
            else if (enemyFSM2 != null)
                enemyFSM2.HitEnemy(gameManager.attackPower);
            else if (enemyFSM3 != null)
                enemyFSM3.HitEnemy(gameManager.attackPower);
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

    // 공격애니메이션 마지막에 실행되는 이벤트함수
    public void InputAllow()
    {
        _PlayerMoveScript.inputAllow = true;
        _PlayerMoveScript.walkspeed = 4f;
        _PlayerMoveScript.runSpeed = 8f;
    }


}
