using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.InputSystem.HID;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    public Animator _animator;
    private Camera _camera;
    private Rigidbody _rigidbody;

    public AnimatorOverrideController swordOverrideAnimator = null;
    public AnimatorOverrideController originalOverrideAnimator;

    public GameObject equippedShield;
    public GameObject equippedSword;
    public GameObject equippedNeck;
    public GameObject equippedShoulder;
    public GameObject equippedTasset;
    public GameObject equippedBoots;

    public float walkspeed;
    public float runSpeed;
    public float finalSpeed;
    float jumpHeight;
    float rotSpeed;
    float smoothness = 10f;
    float horizontalInput;
    float verticalInput; 

    public float punchRange;
    public int punchDamage = 10;
    public int playerHP;
    public int playerMaxHP;

    public bool run;
    private bool isGround = false;
    public bool isJump = false;
    public bool inputAllow = true;
    public bool isDead = false;

    public LayerMask layer;

    public Vector3 moveDirection;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _camera = Camera.main;

        walkspeed = 3f;
        runSpeed = 8f;
        jumpHeight = 4f;
        rotSpeed = 20f;

        punchRange = 0.5f;
        playerHP = 10;
        playerMaxHP = 30;        

    }

    void Update()
    {
        if(isDead) return;

        CheckGround();
        if (Input.GetButtonDown("Jump") && isGround && !isJump &&inputAllow) 
        {
            Jump();
            isJump=true;
            _animator.SetTrigger("Jump");
        }

        if(Input.GetMouseButtonDown(0)) 
        {
            _animator.SetTrigger("Punch");

        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else
        {
            run = false;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            _animator.runtimeAnimatorController= originalOverrideAnimator;
            equippedShield.SetActive(false);
            equippedSword.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _animator.runtimeAnimatorController = swordOverrideAnimator;
            equippedShield.SetActive(true);
            equippedSword.SetActive(true);
        }

    }

    private void FixedUpdate()
    {
        if (isDead) return;

        finalSpeed = (run) ? runSpeed : walkspeed;  // run이 true일 때는 runSpeed, false일 때는 speed

        Vector3 cameraForward = _camera.transform.forward; // 카메라의 전방 벡터를 플레이어의 forward로 사용
        cameraForward.y = 0f; // y 방향은 회전할 필요가 없으므로 0으로 설정
        cameraForward.Normalize(); // 벡터를 정규화하여 길이를 1로 만듦

        if(inputAllow)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }
        Vector3 rightVec = Quaternion.Euler(new Vector3(0, 90, 0)) * cameraForward; // 전방 벡터를 기준으로 오른쪽 벡터
        moveDirection = cameraForward * verticalInput + rightVec * horizontalInput;

        if (moveDirection != Vector3.zero)
        {
            // 입력된 방향의 각도를 구함
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            // 플레이어를 목표 각도로 회전
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }

        _rigidbody.MovePosition(_rigidbody.position + moveDirection.normalized * finalSpeed * Time.deltaTime);
                
        // 속도에 따라 애니메이션 블렌드
        float percent = ((run) ? 1 : 0.5f) * moveDirection.magnitude;
        _animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);

    }

    void CheckGround()
    {
        RaycastHit hit;

        // 발밑 0.2위에서 밑으로 0.201거리만큼 안에 Ground 레이어마스크 있는지
        if (Physics.Raycast(transform.position + (Vector3.up *0.2f),
            Vector3.down,out hit,0.201f,layer))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }

    void Jump()
    {
        Vector3 jumpPower = Vector3.up * jumpHeight;
        GetComponent<Rigidbody>().AddForce(jumpPower, ForceMode.VelocityChange);

    }
        
    public void DamageAction(int attackPower)
    {
        playerHP -= attackPower;
        print(playerHP);
        if (playerHP <= 0)
        {
            if(!isDead)
            {
                _animator.Play("Die", -1, 0f);
            }
            isDead = true;
        }
    }

    void OnDrawGizmos()
    {
        // 적 탐지
        RaycastHit hit2;
        // 플레이어 주먹위치에서 레이 발사

        if (Physics.Raycast(transform.position + (transform.forward * 0.2f) + new Vector3(0, 1.5f, 0),
             transform.forward, out hit2, punchRange))
        {
            // Hit된 지점까지 ray를 그려준다.
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + (transform.forward * 0.2f) + new Vector3(0, 1.5f, 0)
                , transform.forward * punchRange);
        }
    }
}
