using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EnemyController enemyController;

    private float baseSpeed;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyController = GetComponent<EnemyController>();
    }

    private void Start()
    {
        baseSpeed = navMeshAgent.speed;

        // Start AI
        enemyController.ChooseTarget(PlayerManager.Instance.playerBody);
        enemyController.SetAttackAnimation(false, null);
        SetWayPointDestination(PlayerManager.Instance.playerBody.transform.position);
    }

    bool isNear;

    private void Update()
    {
        SetSpeedOnOffMeshLink();

        if (navMeshAgent.enabled)
        {
            if (enemyController.enemyType == 4)
            {
                if (navMeshAgent.remainingDistance <= 5.0f && !isNear)
                {
                    isNear = true;
                    AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].bombFuse, PlayerManager.Instance.playerBody.transform.position, AudioManager.Instance.bombFuseVol);
                }
            }
        }
    }

    public void SetWayPointDestination(Vector3 target)
    {
        navMeshAgent.SetDestination(target);
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
