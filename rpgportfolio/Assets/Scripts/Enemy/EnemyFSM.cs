using Mirror.Examples.CCU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{    
    enum EnemyState
    {
        // �� ��ü�� ���� �з�
        Idle,
        Patrol,
        Move,
        Attack,
        Return,
        Damaged,
        Dead
    }

    [SerializeField] GameObject patrolPath; // waypoint�� �θ������Ʈ
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
    public float attackRange;    // ���ݹ���    
    Vector3 originPos;           // �ʱ� ��ġ ����    

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

        // ������ ���´� ��� ����
        m_State = EnemyState.Idle;

        // �÷��̾��� ��ġ ������ ��������
        player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;

        // �ִϸ����� ��������
        _animator = GetComponent<Animator>();

        // �ڽ��� �ʱ� ��ġ �����ϱ�
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

    // ��� ����
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
        // ���� �÷��̾���� �Ÿ��� �׼� ���� ���� �̳���� Move ���·� ��ȯ�Ѵ�
        if (Vector3.Distance(transform.position, playerTransform.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ : Idle -> Move");
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
        
    // �̵� ����
    void Move()
    {
        playerHP = gameManager.playerHP;
        if (playerHP <= 0) 
        {
            m_State = EnemyState.Return;
            return;
        }
        agent.speed = 2f;

        // ���� ��ġ�� �ʱ� ��ġ���� �̵� ������ ������ �Ѿ�ٸ�
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            // ���� ���¸� ����(Return)�� ��ȯ�Ѵ�
            m_State = EnemyState.Return;
            print("���� ��ȯ : Move -> Return");
            _animator.SetBool("Idle", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", true);
        }
        // ���� �÷��̾���� �Ÿ��� ���� ���� ���̶�� �÷��̾ ���� �̵�
        else if (Vector3.Distance(
            transform.position, playerTransform.position) > attackDistance)
        {            
            agent.SetDestination(player.transform.position);
            _animator.SetBool("Idle", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", true);
            // �÷��̾ �ٶ󺸵��� ȸ��
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        // ������ �������� ������ ��쿡�� ����(Attack) ���·� ��ȯ
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
        playerHP = gameManager.playerHP;
        if (playerHP <= 0)
        {
            m_State = EnemyState.Return;
            return;
        }
        // ���� �÷��̾ ���� ���� ���� ��ġ�ϴ� ��� �÷��̾ ����
        if (Vector3.Distance(transform.position, playerTransform.position) <= attackDistance)
        {
            agent.velocity = Vector3.zero;
            // �÷��̾ �ٶ󺸵��� ȸ��
            Vector3 direction = (player.transform.position- transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // ���� ���ݸ��� �÷��̾ �����Ѵ�
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {      
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
        // ���ݻ�Ÿ��� ����� ���� ���¸� �̵�(Move)���� ��ȯ (���߰�)
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
        // �ʱ� ��ġ������ �Ÿ��� 0.1f �̻��̸� �ʱ� ��ġ�� �̵�
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            agent.SetDestination(originPos);
            _animator.SetBool("Idle", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Walk", true);
        }
        else
        {
            // �׷��� �ʴٸ� �ڽ��� ��ġ�� �ʱ� ��ġ�� �����ϰ� ���� ���¸� ���� ��ȯ
            transform.position = originPos;
            // HP ȸ��
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

    // ������ ó���� �ڷ�ƾ �Լ�
    IEnumerator DamageProcess()
    {
        audioSource.PlayOneShot(orcDamaged);
        // 0.5�ʰ� ����ϸ� �̵� ���·� ��ȯ�Ѵ�
        yield return new WaitForSeconds(0.5f);
        m_State = EnemyState.Move;
        print("���� ��ȯ : Damaged -> Move");
        _animator.SetBool("Idle", false);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Walk", true);
    }

    // ��� ����
    void Dead()
    {
        // ��� ���¸� ó���ϱ� ���� �ڷ�ƾ�� �����Ѵ�
        StartCoroutine(DeadProcess());
    }

    // ��� ���� ó���� �ڷ�ƾ �Լ�
    IEnumerator DeadProcess()
    {        
        // �ѹ������� �ǵ���
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

            // ����ġ �׵�
            gameManager.playerEXP += 100;
            if(gameManager.playerEXP >= gameManager.playerMaxEXP)
            {
                gameManager.canStatUpClick += 1;
                gameManager.playerLevel += 1;
                gameManager.playerEXP -= gameManager.playerMaxEXP;
                gameManager.playerMaxEXP = gameManager.playerMaxEXP * 1.2f;
                gameManager.playerHP = gameManager.playerMaxHP;
                gameManager.playerMP = gameManager.playerMaxMP;
                // ������ ȿ�� ����
                GameObject levelUpEffect = 
                    Instantiate<GameObject>(levelUpEffectPrefab, player.transform);
                Destroy(levelUpEffect, 4f);
                GameObject levelUpText = 
                    Instantiate<GameObject>(levelUpTextPrefab, player.transform);
                levelUpText.transform.Translate(Vector3.up);
                Destroy(levelUpText, 4f);
            }
            
            // 3�� �� �ڱ��ڽ��� �����Ѵ�
            yield return new WaitForSeconds(3.0f);
            print("�Ҹ�!");
            Destroy(gameObject);
        }
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
        if (hp-hitPower < 0)
        {
            hp = 0;
        }
        else
        {
            hp -= hitPower;
        }
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
        _animator.SetBool("Walk", false);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Idle", true);
    }

    public void GiveDamage(int damage)
    {
        RaycastHit hit2;
        // ������Ʈ �߰��뿡�� ���� �߻��ؼ� ���̿� ������
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
        // �� Ž��
        RaycastHit hit2;
        // �÷��̾� �ָ���ġ���� ���� �߻�

        if (Physics.Raycast(transform.position + (transform.forward * 0.2f) + new Vector3(0, 1.5f, 0),
            transform.forward, out hit2, attackRange))
        {
            // Hit�� �������� ray�� �׷��ش�.
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + (transform.forward * 0.2f) + new Vector3(0, 1.5f, 0)
                , transform.forward * attackRange);
        }
    }
}
