using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.HID;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

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

    public bool run;
    private bool isGround = false;
    public bool isJump = false;
    public bool inputAllow = true;
    public bool isDead = false;
    public bool isCursor = true;

    public LayerMask layer;

    public Vector3 moveDirection;

    GameManager gameManager;

    public Image inventoryWindow;
    public Image equipWindow;

    [SerializeField] GameObject fireBallPrefab;
    [SerializeField] GameObject iceRangePrefab;

    AudioSource audioSource;
    [SerializeField] AudioClip playerDamagedAC;
    [SerializeField] AudioClip playerJumpAC;
    [SerializeField] AudioClip fireBallAC;
    [SerializeField] AudioClip iceRangeAC;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _camera = Camera.main;

        walkspeed = 3f;
        runSpeed = 8f;
        jumpHeight = 4f;
        rotSpeed = 20f;

        punchRange = 0.8f;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        audioSource = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        var obj = FindObjectsOfType<PlayerMove>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(isDead) return;

        // 발밑에 땅이 있는지 확인
        RaycastHit hit;

        // 발밑으로 구를 던저서 안에 Ground 레이어마스크 있는지
        if (Physics.SphereCast(transform.position + (Vector3.up * 0.2f), 0.15f,
            Vector3.down, out hit, 0.1f, layer))
        {
            isGround = true;
        }
        if (Input.GetButtonDown("Jump") && isGround && !isJump &&inputAllow) 
        {
            Vector3 jumpPower = Vector3.up * jumpHeight;
            GetComponent<Rigidbody>().AddForce(jumpPower, ForceMode.VelocityChange);
            isJump =true;
            _animator.SetTrigger("Jump");
            audioSource.PlayOneShot(playerJumpAC);
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            // ui 창이 켜져있거나 npc 위에 커서있으면 공격 안됨
            if(!gameManager.blockClick)
            {
                _animator.SetTrigger("Punch");
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else
        {
            run = false;
        }

        // i누르면 인벤토리 on,off
        if(Input.GetKeyDown(KeyCode.I)) 
        {
            if (inventoryWindow.gameObject.activeSelf) 
            {
                inventoryWindow.gameObject.SetActive(false);
                gameManager.canScreenRotate = true;
                gameManager.blockClick = false;
            }
            else 
            {
                inventoryWindow.gameObject.SetActive(true);
                gameManager.canScreenRotate = false;
                gameManager.blockClick = true;
            }
        }

        // p누르면 인벤토리 on,off
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (equipWindow.gameObject.activeSelf)
            {
                equipWindow.gameObject.SetActive(false);
                gameManager.canScreenRotate = true;
                gameManager.blockClick = false;
            }
            else
            {
                equipWindow.gameObject.SetActive(true);
                gameManager.canScreenRotate = false;
                gameManager.blockClick = true;
            }
        }

        // 화면회전 토글
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            gameManager.canScreenRotate = !gameManager.canScreenRotate;
        }
        // 커서 토글
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCursor=!isCursor;
            if(isCursor)
            {
                //커서 안보이게
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

    }

    private void FixedUpdate()
    {
        if (isDead) return;

        // wasd로 입력받기
        if(inputAllow)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }
        // 메인카메라의 전방 벡터를 플레이어의 forward로 사용
        Vector3 cameraForward = _camera.transform.forward;
        cameraForward.Normalize(); // 벡터를 정규화하여 길이를 1로 만듦

        // 전방 벡터를 기준으로 오른쪽 벡터
        Vector3 rightVec = Quaternion.Euler(new Vector3(0, 90, 0)) * cameraForward; 
        moveDirection = cameraForward * verticalInput + rightVec * horizontalInput;

        if (moveDirection != Vector3.zero)
        {
            // 입력된 방향의 각도를 구함
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            // 플레이어를 목표 각도로 회전
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }

        // 캐릭터를 방향대로 이동시킴
        _rigidbody.MovePosition(_rigidbody.position +
            moveDirection.normalized * finalSpeed * Time.deltaTime);


        // run이 true일 때는 runSpeed, false일 때는 walkspeed
        finalSpeed = (run) ? runSpeed : walkspeed;

        // 속도에 따라 애니메이션 블렌드
        float percent = ((run) ? 1 : 0.5f) * moveDirection.magnitude;
        _animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }

    //void CheckGround()
    //{
    //    RaycastHit hit;

    //    // 발밑으로 구를 던저서 안에 Ground 레이어마스크 있는지
    //    if (Physics.SphereCast(transform.position + (Vector3.up * 0.2f), 0.15f,
    //        Vector3.down, out hit, 0.1f, layer))
    //    {
    //        isGround = true;
    //    }
    //}

    public void InstantiateFireBall()
    {
        // 플레이어 파이어볼쏘는 애니메이션
        _animator.SetTrigger("FireBall");

        // 플레이어 앞에서 파이어볼 생성
        Instantiate<GameObject>(fireBallPrefab, transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
        audioSource.PlayOneShot(fireBallAC);
    }
    public void InstantiateIceRange()
    {
        // 플레이어 얼음범위공격시전 애니메이션
        _animator.SetTrigger("IceRange");

        // 플레이어 앞에서 얼음범위공격 생성
        Instantiate<GameObject>(iceRangePrefab, transform);
        audioSource.PlayOneShot(iceRangeAC);
    }

    public void DamageAction(int attackPower)
    {
        if(gameManager.playerHP - attackPower <0)
        {
            gameManager.playerHP = 0;
        }
        else
        {
            gameManager.playerHP -= attackPower;
        }
        print(gameManager.playerHP);
        //audioSource.PlayOneShot(playerDamagedAC);
        if (audioSource.isPlaying)
            return;
        else
            audioSource.PlayOneShot(playerDamagedAC);

        if (gameManager.playerHP <= 0)
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
