using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float chaseDistance;
    [SerializeField] float attackDistance;
    GameObject player;
    public NavMeshAgent agent; // 내비게이션 에이전트
    private Animator _animator;

    bool isAttack = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();

        chaseDistance = 5f;
        attackDistance = 1.5f;
    }

    void Update()
    {
        if(DistanceToPlayer()< chaseDistance)
        {
            agent.SetDestination(player.transform.position);
            _animator.SetTrigger("Walk");
        }

        if(DistanceToPlayer()<= attackDistance && !isAttack)
        {
            isAttack = true;
            agent.Stop();
            _animator.SetTrigger("Attack");
        }

    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }

    public void AttackFinish()
    {
        isAttack = false;
    }
}
