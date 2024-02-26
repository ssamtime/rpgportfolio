using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float chaseDistance = 5f;
    GameObject player;
    public NavMeshAgent agent; // 내비게이션 에이전트
    private Animator _animator;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(DistanceToPlayer()< chaseDistance)
        {
            agent.SetDestination(player.transform.position);
            _animator.SetTrigger("Walk");
        }
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }
}
