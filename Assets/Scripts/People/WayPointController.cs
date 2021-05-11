using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WayPointController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private PeopleController PeopleController;

    // AI Waiting 
    [SerializeField] private bool activeWaiting;
    [SerializeField] private float totalWaitTime = 3f;
    private bool isWaiting;

    // Chance of way point, 2 of 10
    [SerializeField] private float chance = 2;

    // Checking current index of waypoint in list
    public int currentWayPoint;

    // State on agent move to target
    private bool isMove;

    // Checking for agent change way point to forward index or backward
    private bool moveForward;

    private float waitTimer;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        PeopleController = GetComponent<PeopleController>();
    }

    public void Start()
    {
        if (SpawnPeopleManager.Instance.wayPoint != null && SpawnPeopleManager.Instance.wayPoint.Count >= 2 && PeopleController.hpPeople > 0)
        {
            PeopleController.SetRunning(true);

            // Checking Start waypoint from left side
            if (currentWayPoint == 0 || currentWayPoint == 2)
            {
                // Go to waypoint left
                currentWayPoint = 0;
                SetWayPointDestination(SpawnPeopleManager.Instance.wayPoint[currentWayPoint].transform.position);
            }

            // Checking Start waypoint from right side
            if (currentWayPoint == 1 || currentWayPoint == 3)
            {
                // Go to waypoint right
                currentWayPoint = 1;
                SetWayPointDestination(SpawnPeopleManager.Instance.wayPoint[currentWayPoint].transform.position);
            }
        }
    }

    public void Update()
    {
        OnNearlyArriveTarget();
    }

    public void SetWayPointDestination(Vector3 target)
    {
        navMeshAgent.SetDestination(target);
        isMove = true;
    }

    void OnNearlyArriveTarget()
    {
        // Agent is near the current target
        if (isMove && navMeshAgent.enabled)
        {
            if (navMeshAgent.remainingDistance <= 1.0f)
            {
                isMove = false;

                // If waiting is activate then let agent wait at current target
                if (activeWaiting)
                {
                    WaitTimer();
                }
                // or not then change target to move
                else
                {
                    //ChangeWayPointByChance();
                    ChangeToNextWayPoint();
                }
            }
        }
    }

    void WaitTimer()
    {
        isWaiting = true;

        // Waiting at target
        if (isWaiting)
        {
            // Counting by deltatime compare with total wait time
            waitTimer = 0f;
            waitTimer += Time.deltaTime;

            if (waitTimer >= totalWaitTime)
            {
                isWaiting = false;
                //ChangeWayPointByChance();
                ChangeToNextWayPoint();
            }
        }
    }

    void ChangeToNextWayPoint()
    {
        if (currentWayPoint == 0 || currentWayPoint == 1)
        {
            currentWayPoint = 2;
        }

        SetWayPointDestination(SpawnPeopleManager.Instance.wayPoint[currentWayPoint].transform.position);
    }

    // This function only set current way point need to call SetWayPointDestination() to set navmesh
    void ChangeWayPointByChance()
    {
        // Compare chance for move forward or backward waypoint 
        if (Random.Range(0, 11) <= chance)
        {
            moveForward = !moveForward;
        }

        // Move to next index
        if (moveForward)
        {
            // Modula for loop to first index of list, if 3(currentWayPoint) mod 3(wayPoint.count) = 0 << first index
            currentWayPoint = (currentWayPoint + 1) % SpawnPeopleManager.Instance.wayPoint.Count;
        }
        // Move to previous index
        else
        {
            if (--currentWayPoint < 0)
            {
                currentWayPoint = SpawnPeopleManager.Instance.wayPoint.Count - 1;
            }
        }
    }
}
