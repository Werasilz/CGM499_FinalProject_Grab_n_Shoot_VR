using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PeopleController : MonoBehaviour
{
    /// <summary>
    /// TO DO
    /// - make random spawn people
    /// - make people set target to door behind player
    /// - check if have enemy on field
    /// - check distance of player target and checkpoint zone when have enemy on field
    /// - choose to go nearest target
    /// - if go checkpoint zone stop move and set idle/scare animation
    /// - wait until not have enemy on field then move to player
    /// - enemy and player hit people only 1 hit die
    /// </summary>

    private GameObject wayPoint;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public int hpPeople = 1;
    public bool isNearbyEnemy;
    private bool isMove;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        wayPoint = GameObject.Find("PeopleWayPoint");

        if (wayPoint != null && hpPeople > 0)
        {
            SetDestination();
        }
    }

    void LateUpdate()
    {
        CheckEnemyAround();
    }

    void SetDestination()
    {
        animator.SetBool("isScare", false);
        animator.SetBool("isRun", true);
        Vector3 targetPosition = wayPoint.transform.position;                                                       // Set target to the first index of list
        navMeshAgent.SetDestination(targetPosition);                                                                // Agent move to target
        isMove = true;
    }

    void CheckEnemyAround()
    {
        if (isMove && isNearbyEnemy)
        {
            StopRunning();
        }

        if (!isMove && !isNearbyEnemy)
        {
            ContinueRunning();
        }
    }

    void StopRunning()
    {
        isMove = false;
        navMeshAgent.isStopped = true;
        animator.SetBool("isRun", false);
        animator.SetBool("isScare", true);
    }

    void ContinueRunning()
    {
        navMeshAgent.isStopped = false;
        SetDestination();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            isNearbyEnemy = true;
            other.gameObject.GetComponentInParent<SetWayPoint>().SetWayPointDestination(gameObject.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            isNearbyEnemy = false;
        }
    }
}
