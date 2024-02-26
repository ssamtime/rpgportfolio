using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    // 적 개체의 상태 분류
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Dead
    }

    
    EnemyState m_State;          // 적 개체의 상태     
    GameObject player;           // 플레이어   
    Transform playerTransform;   // 플레이어 좌표
    CharacterController cc;      // 캐릭터 컨트롤러   
    public float findDistance;   // 플레이어 색적 범위   
    public float attackDistance; // 공격 가능한 범위    
    public float moveSpeed;      // 이동 속도     
    public float moveDistance;   // 이동 가능한 범위
    float currentTime;           // 누적 시간    
    float attackDelay;           // 공격 딜레이    
    public int attackPower;      // 공격력    
    Vector3 originPos;           // 초기 위치 저장    

    int hp;
    int maxHp;

    private void Start()
    {
        // 최초의 상태는 대기 상태
        m_State = EnemyState.Idle;

        // 플레이어의 위치 정보를 가져오기
        player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;
        // 캐릭터 컨트롤러 컴포넌트 가져오기
        cc = GetComponent<CharacterController>();

        // 자신의 초기 위치 저장하기
        originPos = transform.position;

        findDistance = 5.0f;
        attackDistance = 2.0f;
        moveSpeed = 5.0f;
        moveDistance = 20.0f;
        currentTime = 0;
        attackDelay = 2.0f;
        attackPower = 3;

        hp = 10;
        maxHp = 30;
    }

    private void Update()
    {
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
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
                Damaged();
                break;
            case EnemyState.Dead:
                Dead();
                break;
            default:
                break;
        }
    }

    // 대기 상태
    void Idle()
    {
        // 만약 플레이어와의 거리가 액션 시작 범위 이내라면 Move 상태로 전환한다
        if (Vector3.Distance(transform.position, playerTransform.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환 : Idle -> Move");
        }
    }

    // 이동 상태
    void Move()
    {
        // 현재 위치가 초기 위치에서 이동 가능한 범위를 넘어간다면?
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            // 현재 상태를 복귀(Return)로 전환한다
            m_State = EnemyState.Return;
            print("상태 전환 : Move -> Return");
        }
        // 만약 플레이어와의 거리가 공격 범위 밖이라면 플레이어를 향해 이동한다
        else if (Vector3.Distance(transform.position, playerTransform.position) > attackDistance)
        {
            // 이동 방향 설정
            Vector3 dir = (playerTransform.position - transform.position).normalized;

            // 캐릭터 컨트롤러를 이용한 이동 처리
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        // 조건을 충족하지 못했을 경우에는 공격(Attack) 상태로 전환한다
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환 : Move -> Attack");

            // 누적 시간을 공격 딜레이 시간만큼 미리 진행
            currentTime = attackDelay;
        }
    }

    // 공격 상태
    void Attack()
    {
        // 만약 플레이어가 공격 범위 내에 위치하는 경우 플레이어를 공격한다
        if (Vector3.Distance(transform.position, playerTransform.position) < attackDistance)
        {
            // 일정 간격마다 플레이어를 공격한다
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격");
                currentTime = 0;
            }
        }
        // 그렇지 않다면 현재 상태를 이동(Move)으로 전환한다 (재추격)
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환 : Attack -> Move");
            currentTime = 0;
        }
    }

    // 복귀 상태
    void Return()
    {
        // 초기 위치에서의 거리가 0.1f 이상이면 초기 위치로 이동한다
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        else
        // 그렇지 않다면 자신의 위치를 초기 위치로 조정하고 현재 상태를 대기로 전환
        {
            transform.position = originPos;

            // HP를 회복한다
            hp = maxHp;
            m_State = EnemyState.Idle;
            print("상태 전환 : Return -> Idle");
        }
    }

    // 피격 상태
    void Damaged()
    {
        // 코루틴 함수 호출
        StartCoroutine(DamageProcess());
    }

    // 사망 상태
    void Dead()
    {
        // 진행 중인 피격 관련 코루틴을 중지한다
        StopAllCoroutines();

        // 사망 상태를 처리하기 위한 코루틴을 실행한다
        StartCoroutine(DeadProcess());
    }

    // 사망 상태 처리용 코루틴 함수
    IEnumerator DeadProcess()
    {
        // 캐릭터 컨트롤러 비활성화
        cc.enabled = false;

        // 2초 후 자기자신(적 오브젝트)를 제거한다
        yield return new WaitForSeconds(2.0f);
        print("소멸!");
        Destroy(gameObject);
    }

    // 데미지 처리용 코루틴 함수
    IEnumerator DamageProcess()
    {
        // 0.5초가 경과하면 이동 상태로 전환한다
        yield return new WaitForSeconds(0.5f);
        m_State = EnemyState.Move;
        print("상태 전환 : Damaged -> Move");
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
        hp -= hitPower;

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
}
