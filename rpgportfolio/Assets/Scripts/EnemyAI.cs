using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float chaseDistance = 5f;
    GameObject player;
    public NavMeshAgent agent; // ������̼� ������Ʈ

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if(DistanceToPlayer()< chaseDistance)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }
}
