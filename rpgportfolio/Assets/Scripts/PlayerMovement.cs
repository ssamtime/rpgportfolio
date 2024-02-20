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
            toggleCameraRotation = true;    //�ѷ����� Ȱ��ȭ
        }
        else
        {
            toggleCameraRotation = false;   //�ѷ����� ��Ȱ��ȭ
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
        if (!toggleCameraRotation) // toggleCameraRotation�� true�� �ƴ� ���� ȸ�� �ڵ� ����
        {
            Vector3 playerRotate = Vector3.Scale(
                _camera.transform.forward, new Vector3(1, 0, 1));   // �������
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(playerRotate),
                Time.deltaTime * smoothness);
        }
    }

    void InputMovement()
    {
        finalSpeed = (run) ? runSpeed : speed;  // shiftŰ�� ������ run�� true�� ���� runSpeed, false�� ���� speed

        Vector3 forward = _camera.transform.forward; // ī�޶��� ���� ���͸� �÷��̾��� forward�� ���
        forward.y = 0f; // y ������ ȸ���� �ʿ䰡 �����Ƿ� 0���� ����
        forward.Normalize(); // ���͸� ����ȭ�Ͽ� ���̸� 1�� ����

        Vector3 right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward; // ���� ���͸� �������� ������ ���� ���

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = forward * verticalInput + right * horizontalInput;

        if (toggleCameraRotation == false)  // alt ������ ī�޶� ȸ���ǰ�
        {
            // �Էµ� ������ 0�� �ƴ� ��쿡�� ȸ�� ����
            if (moveDirection != Vector3.zero)
            {
                // �Էµ� ������ ������ ����
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                // �÷��̾ ��ǥ ������ ȸ��
                transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            }
        }

        _controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime); // �̵� ����

        // �ִϸ��̼� �ӵ� ����
        float percent = ((run) ? 1 : 0.5f) * moveDirection.magnitude;
        _animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }
}
