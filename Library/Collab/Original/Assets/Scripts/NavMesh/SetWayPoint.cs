using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetWayPoint : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EnemyController enemyController;

    // AI Waiting 
    [SerializeField] private bool activeWaiting;
    [SerializeField] private float totalWaitTime = 3f;
    private bool isWaiting;

    // Chance of way point, 2 of 10
    [SerializeField] private float chance = 2;

    // Checking current index of waypoint in list
    [HideInInspector] public int currentWayPoint;

    // State on agent move to target
    private bool isMove;

    // Checking for agent change way point to forward index or backward
    private bool moveForward;

    private float waitTimer;
    private float baseSpeed;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyController = GetComponent<EnemyController>();
    }

    public void Start()
    {
        if (navMeshAgent != null)
        {
            // For waypoint > 2
            /*if (WaveManager.Instance.wayPoint != null && WaveManager.Instance.wayPoint.Count >= 2 && enemyController.hpEnemy > 0)
            {
                currentWayPoint = 0;
                SetDestination();
            }
            else
            {
                Vector3 targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
                navMeshAgent.SetDestination(targetPosition);
                isMove = true;
            }*/

            // Only player waypoint
            if (WaveManager.Instance.wayPoint != null && enemyController.hpEnemy > 0)
            {
                currentWayPoint = 0;
                SetWayPointDestination(WaveManager.Instance.wayPoint[currentWayPoint].transform.position);
            }
        }

        baseSpeed = navMeshAgent.speed;
    }

    public void Update()
    {
        /*if (WaveManager.Instance.wayPoint != null)
        {
            OnNearlyArriveTarget();
        }*/

        SetSpeedOnOffMeshLink();
    }

    public void SetWayPointDestination(Vector3 target)
    {
        navMeshAgent.SetDestination(target);                                                                        // Agent move to target
        isMove = true;
    }

    void OnNearlyArriveTarget()
    {
        if (isMove && navMeshAgent.remainingDistance <= 1.0f)                                                       // Agent is near the current target
        {
            isMove = false;

            if (activeWaiting)                                                                                      // If waiting is activate then let agent wait at current target
            {
                WaitTimer();
            }
            else                                                                                                    // or not then change target to move
            {
                ChangeWayPoint();
                SetWayPointDestination(WaveManager.Instance.wayPoint[currentWayPoint].transform.position);
            }
        }
    }

    void WaitTimer()
    {
        isWaiting = true;

        if (isWaiting)                                                                                              // Waiting at target
        {
            waitTimer = 0f;
            waitTimer += Time.deltaTime;                                                                            // Counting by deltatime compare with total wait time

            if (waitTimer >= totalWaitTime)
            {
                isWaiting = false;
                ChangeWayPoint();
                SetWayPointDestination(WaveManager.Instance.wayPoint[currentWayPoint].transform.position);
            }
        }
    }

    void ChangeWayPoint()
    {
        if (Random.Range(0, 11) <= chance)                                                                          // Compare chance for move forward or backward
        {
            moveForward = !moveForward;
        }

        if (moveForward)                                                                                            // Move to next index
        {
            currentWayPoint = (currentWayPoint + 1) % WaveManager.Instance.wayPoint.Count;                          // Modula for loop to first index of list, if 3(currentWayPoint) mod 3(wayPoint.count) = 0 << first index
        }
        else                                                                                                        // Move to previous index
        {
            if (--currentWayPoint < 0)
            {
                currentWayPoint = WaveManager.Instance.wayPoint.Count - 1;
            }
        }
    }

    void SetSpeedOnOffMeshLink()
    {
        if (navMeshAgent.isOnOffMeshLink)
        {
            navMeshAgent.speed = baseSpeed * 10;
        }
        else
        {
            navMeshAgent.speed = baseSpeed;
        }
    }
}
