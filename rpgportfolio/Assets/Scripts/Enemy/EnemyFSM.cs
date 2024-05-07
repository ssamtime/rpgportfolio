using Mirror.Examples.CCU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{    
    enum EnemyState
    {
        // 적 개체의 상태 분류
        Idle,
        Patrol,
        Move,
        Attack,
        Return,
        Damaged,
        Dead
    }

    [SerializeField] GameObject patrolPath; // waypoint들 부모오브젝트
    [SerializeField] EnemyState m_State;          // 적 개체의 상태     
    private GameObject player;           // 플레이어   
    public NavMeshAgent agent; // 내비게이션 에이전트
    private Animator _animator;
    Transform playerTransform;   // 플레이어 좌표
    CharacterController cc;      // 캐릭터 컨트롤러   
    public float findDistance;   // 플레이어 색적 범위   
    public float attackDistance; // 공격 가능한 범위    
    public float moveSpeed;      // 이동 속도     
    public float moveDistance;   // 이동 가능한 범위
    float rotationSpeed;         // 플레이어 방향으로 회전하는 속도
    float currentTime;           // 누적 시간    
    float attackDelay;           // 공격 딜레이    
    public int attackPower;      // 공격력    
    public float attackRange;    // 공격범위    
    Vector3 originPos;           // 초기 위치 저장    

    public int hp;
    public int maxHp;
    bool isAttack = false;
    bool isDeadAnim = false;
    int playerHP;

    int waypointIndex;
    float stayTime;

    GameManager gameManager;

    [SerializeField] GameObject levelUpEffectPrefab;
    [SerializeField] GameObject levelUpTextPrefab;

    AudioSource audioSource;
    [SerializeField] AudioClip orcDamaged;
    [SerializeField] AudioClip orcDead;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // 최초의 상태는 대기 상태
        m_State = EnemyState.Idle;

        // 플레이어의 위치 정보를 가져오기
        player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;

        // 애니메이터 가져오기
        _animator = GetComponent<Animator>();

        // 자신의 초기 위치 저장하기
        originPos = transform.position;

        findDistance = 8.0f;
        attackDistance = 1.5f;
        moveSpeed = 3.0f;
        moveDistance = 20.0f;
        rotationSpeed = 15.0f;
        currentTime = 0;
        attackDelay = 2.0f;
        attackPower = 30;
        attackRange = 2.5f; 

        hp = 100;
        maxHp = 100;
        playerHP = gameManager.playerHP;

        waypointIndex = 0;
        stayTime = 5f;

        audioSource = GetComponent<AudioSource>();

    }
    

    private void Update()
    {        
        switch (m_State)
        {
            case EnemyState.Idle:
                Patrol();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Dead:
                Dead();
                break;
            default:
                break;
        }
    }

    // 대기 상태
    void Patrol()
    {
        playerHP = gameManager.playerHP;
        if (playerHP <= 0)
        {
            _animator.SetBool("Idle", true);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", false);
            return;
        }
        // 만약 플레이어와의 거리가 액션 시작 범위 이내라면 Move 상태로 전환한다
        if (Vector3.Distance(transform.position, playerTransform.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환 : Idle -> Move");
            _animator.SetBool("Idle", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", true);
        }
        else
        {             
            stayTime += Time.deltaTime;
            if (stayTime > 7f)
            {
                stayTime = 0f;
                agent.speed = 1f;
                _animator.SetBool("Idle", false);
                _animator.SetBool("Attack", false);
                _animator.SetBool("Walk", true);
                agent.SetDestination(patrolPath.transform.GetChild(waypointIndex).position);
            }

            if (Vector3.Distance(patrolPath.transform.GetChild(waypointIndex).position,
                transform.position) < 0.3f)
            {
                _animator.SetBool("Idle", true);
                _animator.SetBool("Attack", false);
                _animator.SetBool("Walk", false);

                waypointIndex++;
                if(waypointIndex>=patrolPath.transform.childCount)
                    waypointIndex= 0;
            }
        }        
    }
        
    // 이동 상태
    void Move()
    {
        playerHP = gameManager.playerHP;
        if (playerHP <= 0) 
        {
            m_State = EnemyState.Return;
            return;
        }
        agent.speed = 2f;

        // 현재 위치가 초기 위치에서 이동 가능한 범위를 넘어간다면
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            // 현재 상태를 복귀(Return)로 전환한다
            m_State = EnemyState.Return;
            print("상태 전환 : Move -> Return");
            _animator.SetBool("Idle", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", true);
        }
        // 만약 플레이어와의 거리가 공격 범위 밖이라면 플레이어를 향해 이동
        else if (Vector3.Distance(
            transform.position, playerTransform.position) > attackDistance)
        {            
            agent.SetDestination(player.transform.position);
            _animator.SetBool("Idle", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", true);
            // 플레이어를 바라보도록 회전
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        // 조건을 충족하지 못했을 경우에는 공격(Attack) 상태로 전환
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환 : Move -> Attack");
            agent.velocity = Vector3.zero;
            // 누적 시간을 공격 딜레이 시간만큼 미리 진행
            currentTime = attackDelay;
        }
    }

    // 공격 상태
    void Attack()
    {
        playerHP = gameManager.playerHP;
        if (playerHP <= 0)
        {
            m_State = EnemyState.Return;
            return;
        }
        // 만약 플레이어가 공격 범위 내에 위치하는 경우 플레이어를 공격
        if (Vector3.Distance(transform.position, playerTransform.position) <= attackDistance)
        {
            agent.velocity = Vector3.zero;
            // 플레이어를 바라보도록 회전
            Vector3 direction = (player.transform.position- transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // 일정 간격마다 플레이어를 공격한다
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {      
                print("공격");
                currentTime = 0;

                isAttack = true;
                _animator.SetBool("Idle", true);
                _animator.SetBool("Attack", true);
                _animator.SetBool("Walk", false);
            }
        }
        else if(isAttack)
        {
            agent.velocity = Vector3.zero;
            return;
        }
        // 공격사거리를 벗어나면 현재 상태를 이동(Move)으로 전환 (재추격)
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환 : Attack -> Move");
            currentTime = 0;
            _animator.SetBool("Idle", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", true);
        }
    }

    // 복귀 상태
    void Return()
    {
        // 초기 위치에서의 거리가 0.1f 이상이면 초기 위치로 이동
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            agent.SetDestination(originPos);
            _animator.SetBool("Idle", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", true);
        }
        else
        {
            // 그렇지 않다면 자신의 위치를 초기 위치로 조정하고 현재 상태를 대기로 전환
            transform.position = originPos;
            // HP 회복
            hp = maxHp;
            m_State = EnemyState.Idle;
            print("상태 전환 : Return -> Idle");
            _animator.SetBool("Idle", true);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", false);
        }
    }

    // 피격 상태
    void Damaged()
    {
        // 코루틴 함수 호출
        StartCoroutine(DamageProcess());
    }

    // 데미지 처리용 코루틴 함수
    IEnumerator DamageProcess()
    {
        audioSource.PlayOneShot(orcDamaged);
        // 0.5초가 경과하면 이동 상태로 전환한다
        yield return new WaitForSeconds(0.5f);
        m_State = EnemyState.Move;
        print("상태 전환 : Damaged -> Move");
        _animator.SetBool("Idle", false);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Walk", true);
    }

    // 사망 상태
    void Dead()
    {
        // 사망 상태를 처리하기 위한 코루틴을 실행한다
        StartCoroutine(DeadProcess());
    }

    // 사망 상태 처리용 코루틴 함수
    IEnumerator DeadProcess()
    {        
        // 한번만실행 되도록
        if (!isDeadAnim)
        {
            audioSource.PlayOneShot(orcDead);
            gameObject.GetComponent<Collider>().enabled = false;
            agent.speed = 0f;
            isDeadAnim = true;

            _animator.SetBool("Idle", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", false);
            _animator.Play("Die", -1, 0f);

            // 경험치 휙득
            gameManager.playerEXP += 100;
            if(gameManager.playerEXP >= gameManager.playerMaxEXP)
            {
                gameManager.canStatUpClick += 1;
                gameManager.playerLevel += 1;
                gameManager.playerEXP -= gameManager.playerMaxEXP;
                gameManager.playerMaxEXP = gameManager.playerMaxEXP * 1.2f;
                gameManager.playerHP = gameManager.playerMaxHP;
                gameManager.playerMP = gameManager.playerMaxMP;
                // 레벨업 효과 생성
                GameObject levelUpEffect = 
                    Instantiate<GameObject>(levelUpEffectPrefab, player.transform);
                Destroy(levelUpEffect, 4f);
                GameObject levelUpText = 
                    Instantiate<GameObject>(levelUpTextPrefab, player.transform);
                levelUpText.transform.Translate(Vector3.up);
                Destroy(levelUpText, 4f);
            }
            
            // 3초 후 자기자신을 제거한다
            yield return new WaitForSeconds(3.0f);
            print("소멸!");
            Destroy(gameObject);
        }
    }


    // 데미지 처리 실행용 함수
    public void HitEnemy(int hitPower)
    {
        // 현재 상태가 피격/사망/복귀 중 하나에 해당할 경우 즉시 함수 종료
        if (m_State == EnemyState.Damaged
            || m_State == EnemyState.Dead
            || m_State == EnemyState.Return)
        {
            return;
        }

        // 플레이어의 공격력만큼 적의 체력을 감소시킨다
        if (hp-hitPower < 0)
        {
            hp = 0;
        }
        else
        {
            hp -= hitPower;
        }
        print(hp);

        // 적 체력이 0보다 크면 피격 상태로 전환한다
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환 : Any State -> Damaged");
            Damaged();
        }
        // 그렇지 않다면 사망 상태로 전환한다
        else
        {
            m_State = EnemyState.Dead;
            print("상태 전환 : Any State -> Dead");
            Dead();
        }
    }

    public void AttackFinish()
    {
        isAttack = false;
        _animator.SetBool("Walk", false);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Idle", true);
    }

    public void GiveDamage(int damage)
    {
        RaycastHit hit2;
        // 오브젝트 중간쯤에서 레이 발사해서 레이에 닿으면
        if (Physics.Raycast(transform.position + (transform.forward * 0.1f) + new Vector3(0, 1.5f, 0),
            transform.forward, out hit2, attackRange))
        {
            PlayerMove playerMoveScript = hit2.collider.GetComponent<PlayerMove>();
            if (playerMoveScript != null)
            {
                transform.LookAt(hit2.transform);
                if(attackPower - gameManager.armorPower>=1)
                {
                    playerMoveScript.DamageAction(attackPower - gameManager.armorPower);
                }
                else
                {
                    playerMoveScript.DamageAction(1);
                }
            }
        }
    }


    void OnDrawGizmos()
    {
        // 적 탐지
        RaycastHit hit2;
        // 플레이어 주먹위치에서 레이 발사

        if (Physics.Raycast(transform.position + (transform.forward * 0.2f) + new Vector3(0, 1.5f, 0),
            transform.forward, out hit2, attackRange))
        {
            // Hit된 지점까지 ray를 그려준다.
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + (transform.forward * 0.2f) + new Vector3(0, 1.5f, 0)
                , transform.forward * attackRange);
        }
    }
}
