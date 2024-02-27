using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    // �� ��ü�� ���� �з�
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Dead
    }
    
    [SerializeField] EnemyState m_State;          // �� ��ü�� ����     
    private GameObject player;           // �÷��̾�   
    public NavMeshAgent agent; // ������̼� ������Ʈ
    private Animator _animator;
    Transform playerTransform;   // �÷��̾� ��ǥ
    CharacterController cc;      // ĳ���� ��Ʈ�ѷ�   
    public float findDistance;   // �÷��̾� ���� ����   
    public float attackDistance; // ���� ������ ����    
    public float moveSpeed;      // �̵� �ӵ�     
    public float moveDistance;   // �̵� ������ ����
    float rotationSpeed;         // �÷��̾� �������� ȸ���ϴ� �ӵ�
    float currentTime;           // ���� �ð�    
    float attackDelay;           // ���� ������    
    public int attackPower;      // ���ݷ�    
    Vector3 originPos;           // �ʱ� ��ġ ����    

    int hp;
    int maxHp;
    bool isAttack = false;

    private void Start()
    {
        // ������ ���´� ��� ����
        m_State = EnemyState.Idle;

        // �÷��̾��� ��ġ ������ ��������
        player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;

        // �ִϸ����� ��������
        _animator = GetComponent<Animator>();

        // �ڽ��� �ʱ� ��ġ �����ϱ�
        originPos = transform.position;

        findDistance = 5.0f;
        attackDistance = 1.5f;
        moveSpeed = 3.0f;
        moveDistance = 20.0f;
        rotationSpeed = 2.0f;
        currentTime = 0;
        attackDelay = 2.0f;
        attackPower = 3;

        hp = 20;
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

    // ��� ����
    void Idle()
    {
        // ���� �÷��̾���� �Ÿ��� �׼� ���� ���� �̳���� Move ���·� ��ȯ�Ѵ�
        if (Vector3.Distance(transform.position, playerTransform.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ : Idle -> Move");
            _animator.SetBool("Idle", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", true);
        }
    }

    // �̵� ����
    void Move()
    {
        // ���� ��ġ�� �ʱ� ��ġ���� �̵� ������ ������ �Ѿ�ٸ�?
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            // ���� ���¸� ����(Return)�� ��ȯ�Ѵ�
            m_State = EnemyState.Return;
            print("���� ��ȯ : Move -> Return");
            _animator.SetBool("Idle", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", true);
        }
        // ���� �÷��̾���� �Ÿ��� ���� ���� ���̶�� �÷��̾ ���� �̵��Ѵ�
        else if (Vector3.Distance(transform.position, playerTransform.position) > attackDistance)
        {
            agent.SetDestination(player.transform.position);
            _animator.SetBool("Idle", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", true);
        }
        // ������ �������� ������ ��쿡�� ����(Attack) ���·� ��ȯ�Ѵ�
        else
        {
            m_State = EnemyState.Attack;
            print("���� ��ȯ : Move -> Attack");
            agent.velocity = Vector3.zero;

            // ���� �ð��� ���� ������ �ð���ŭ �̸� ����
            currentTime = attackDelay;
        }
    }

    // ���� ����
    void Attack()
    {
        // ���� �÷��̾ ���� ���� ���� ��ġ�ϴ� ��� �÷��̾ �����Ѵ�
        if (Vector3.Distance(transform.position, playerTransform.position) <= attackDistance)
        {
            agent.velocity = Vector3.zero;

            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // ���� ���ݸ��� �÷��̾ �����Ѵ�
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                player.GetComponent<PlayerMove>().DamageAction(attackPower);    //�̰� ������������                

                print("����");
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
        // ���ݻ�Ÿ��� ����� ���� ���¸� �̵�(Move)���� ��ȯ�Ѵ� (���߰�)
        else
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ : Attack -> Move");
            currentTime = 0;
            _animator.SetBool("Idle", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", true);
        }
    }

    // ���� ����
    void Return()
    {
        // �ʱ� ��ġ������ �Ÿ��� 0.1f �̻��̸� �ʱ� ��ġ�� �̵��Ѵ�
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            //Vector3 dir = (originPos - transform.position).normalized;
            //cc.Move(dir * moveSpeed * Time.deltaTime);

            agent.SetDestination(originPos);
            _animator.SetBool("Idle", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", true);
        }
        else
        // �׷��� �ʴٸ� �ڽ��� ��ġ�� �ʱ� ��ġ�� �����ϰ� ���� ���¸� ���� ��ȯ
        {
            transform.position = originPos;

            // HP�� ȸ���Ѵ�
            hp = maxHp;
            m_State = EnemyState.Idle;
            print("���� ��ȯ : Return -> Idle");
            _animator.SetBool("Idle", true);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", false);

        }
    }

    // �ǰ� ����
    void Damaged()
    {
        // �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(DamageProcess());
    }

    // ��� ����
    void Dead()
    {
        // ���� ���� �ǰ� ���� �ڷ�ƾ�� �����Ѵ�
        StopAllCoroutines();

        // ��� ���¸� ó���ϱ� ���� �ڷ�ƾ�� �����Ѵ�
        StartCoroutine(DeadProcess());
    }

    // ��� ���� ó���� �ڷ�ƾ �Լ�
    IEnumerator DeadProcess()
    {
        //// ĳ���� ��Ʈ�ѷ� ��Ȱ��ȭ
        //cc.enabled = false;
        _animator.SetBool("Idle", false);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Walk", false);
        _animator.SetTrigger("Dead");
        
        // 2�� �� �ڱ��ڽ�(�� ������Ʈ)�� �����Ѵ�
        yield return new WaitForSeconds(2.0f);
        print("�Ҹ�!");
        Destroy(gameObject);
    }

    // ������ ó���� �ڷ�ƾ �Լ�
    IEnumerator DamageProcess()
    {
        // 0.5�ʰ� ����ϸ� �̵� ���·� ��ȯ�Ѵ�
        yield return new WaitForSeconds(0.5f);
        m_State = EnemyState.Move;
        print("���� ��ȯ : Damaged -> Move");
        _animator.SetBool("Idle", false);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Walk", true);
    }

    // ������ ó�� ����� �Լ�
    public void HitEnemy(int hitPower)
    {
        // ���� ���°� �ǰ�/���/���� �� �ϳ��� �ش��� ��� ��� �Լ� ����
        if (m_State == EnemyState.Damaged
            || m_State == EnemyState.Dead
            || m_State == EnemyState.Return)
        {
            return;
        }

        // �÷��̾��� ���ݷ¸�ŭ ���� ü���� ���ҽ�Ų��
        hp -= hitPower;
        print(hp);

        // �� ü���� 0���� ũ�� �ǰ� ���·� ��ȯ�Ѵ�
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("���� ��ȯ : Any State -> Damaged");
            Damaged();
        }
        // �׷��� �ʴٸ� ��� ���·� ��ȯ�Ѵ�
        else
        {
            m_State = EnemyState.Dead;
            print("���� ��ȯ : Any State -> Dead");
            Dead();
        }
    }

    public void AttackFinish()
    {
        isAttack = false;
        agent.Resume();
        _animator.SetBool("Walk", false);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Idle", true);
    }

    public void GiveDamage(int damage)
    {

    }
}
