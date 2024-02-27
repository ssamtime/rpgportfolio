using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.InputSystem.HID;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using RPGCharacterAnims.Actions;

public class PlayerMove : MonoBehaviour
{
    private Animator _animator;
    private Camera _camera;
    private Rigidbody _rigidbody;

    public float walkspeed;
    public float runSpeed;
    public float finalSpeed;
    public float jumpHeight;
    public float rotSpeed;
    public float smoothness = 10f;
    float horizontalInput;
    float verticalInput; 

    public float punchRange;
    public int punchDamage = 10;
    public int playerHP;

    public bool run;
    private bool isGround = false;
    public bool isJump = false;
    public bool inputAllow = true;

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
    }

    void Update()
    {
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

    }

    private void FixedUpdate()
    {
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
    }
}
