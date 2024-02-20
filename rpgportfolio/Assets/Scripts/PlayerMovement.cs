using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator _animator;
    Camera _camera;
    CharacterController _controller;

    public float speed = 5f;
    public float runSpeed = 8f;
    public float finalSpeed;

    public bool toggleCameraRotation;
    public bool run;

    public float smoothness = 10f;

    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _camera = Camera.main;
        _controller = this.GetComponent<CharacterController>();
    }


    void Update()
    {
        if(Input.GetKey(KeyCode.LeftAlt)) 
        {
            toggleCameraRotation = true;    //둘러보기 활성화
        }
        else
        {
            toggleCameraRotation = false;   //둘러보기 비활성화
        }

        if(Input.GetKey(KeyCode.LeftShift)) 
        {
            run = true;
        }
        else
        {
            run = false;
        }

        InputMovement();
    }
    private void LateUpdate()
    {
        //if(toggleCameraRotation != true)
        if (!toggleCameraRotation) // toggleCameraRotation이 true가 아닐 때만 회전 코드 실행
        {
            Vector3 playerRotate = Vector3.Scale(
                _camera.transform.forward, new Vector3(1, 0, 1));   // 수평방향
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(playerRotate),
                Time.deltaTime * smoothness);
        }
    }

    void InputMovement()
    {
        finalSpeed = (run) ? runSpeed : speed;  // shift키를 누르면 run이 true일 때는 runSpeed, false일 때는 speed

        Vector3 forward = _camera.transform.forward; // 카메라의 전방 벡터를 플레이어의 forward로 사용
        forward.y = 0f; // y 방향은 회전할 필요가 없으므로 0으로 설정
        forward.Normalize(); // 벡터를 정규화하여 길이를 1로 만듦

        Vector3 right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward; // 전방 벡터를 기준으로 오른쪽 벡터 계산

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = forward * verticalInput + right * horizontalInput;

        if (toggleCameraRotation == false)  // alt 누르면 카메라 회전되게
        {
            // 입력된 방향이 0이 아닌 경우에만 회전 실행
            if (moveDirection != Vector3.zero)
            {
                // 입력된 방향의 각도를 구함
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                // 플레이어를 목표 각도로 회전
                transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            }
        }

        _controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime); // 이동 적용

        // 애니메이션 속도 설정
        float percent = ((run) ? 1 : 0.5f) * moveDirection.magnitude;
        _animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }
}
