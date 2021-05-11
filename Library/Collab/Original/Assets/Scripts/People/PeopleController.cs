using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PeopleController : MonoBehaviour
{
    private GameObject wayPoint;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public int hpPeople = 10;
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

    private void Update()
    {
        HealthEnemy();
    }

    void LateUpdate()
    {
        CheckEnemyAround();
    }

    void HealthEnemy()
    {
        if (hpPeople <= 0)
        {
            transform.GetComponent<RagdollScript>().Dead();
        }
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponentInParent<EnemyController>().targetAim != gameObject)
            {
                isNearbyEnemy = true;
                other.gameObject.GetComponentInParent<SetWayPoint>().SetWayPointDestination(gameObject.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            isNearbyEnemy = false;
            other.gameObject.GetComponentInParent<EnemyController>().targetAim = null;
        }
    }
}
