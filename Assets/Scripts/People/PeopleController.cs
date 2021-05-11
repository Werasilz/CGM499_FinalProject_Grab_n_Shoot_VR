using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PeopleController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    public GameObject helpPopup;

    public int hpPeople;
    public bool isNearbyEnemy;
    private bool isMove;

    public int enemyNearbyCount;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        hpPeople = 5;
    }

    private void Update()
    {
        HealthPeople();
    }

    void LateUpdate()
    {
        CheckEnemyAround();
    }

    void CheckEnemyAround()
    {
        // Is enemy nearby stop running and play scare animation
        if (isMove && isNearbyEnemy)
        {
            helpPopup.transform.GetChild(0).gameObject.SetActive(true);
            SetRunning(false);

            StartCoroutine(PeopleSignalSound());
        }

        // Is enemy not nearby continue run to waypoint
        if (!isMove && !isNearbyEnemy)
        {
            helpPopup.transform.GetChild(0).gameObject.SetActive(false);
            SetRunning(true);

            StopAllCoroutines();
        }
    }

    IEnumerator PeopleSignalSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].peopleScary, transform.position, AudioManager.Instance.peopleScaryVol);
        }
    }

    public void SetRunning(bool value)
    {
        if (navMeshAgent.enabled)
        {
            isMove = value;
            navMeshAgent.isStopped = !value;
            animator.SetBool("isRun", value);
            animator.SetBool("isScare", !value);
        }
    }

    void HealthPeople()
    {
        if (hpPeople <= 0)
        {
            helpPopup.transform.GetChild(0).gameObject.SetActive(false);
            transform.GetComponent<RagdollScript>().Dead();
            StopAllCoroutines();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyArea"))
        {
            if (hpPeople > 0)
            {
                if (other.gameObject.GetComponentInParent<EnemyController>().hpEnemy > 0)
                {
                    if (other.gameObject.GetComponentInParent<EnemyController>().targetAim.transform.name == "PlayerBody")
                    {
                        enemyNearbyCount += 1;
                        isNearbyEnemy = true;
                        other.gameObject.GetComponentInParent<EnemyController>().ChooseTarget(gameObject);
                        other.gameObject.GetComponentInParent<EnemyController>().SetAttackAnimation(false, null);
                        other.gameObject.GetComponentInParent<AgentController>().SetWayPointDestination(other.gameObject.GetComponentInParent<EnemyController>().targetAim.transform.position);
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyArea"))
        {
            if (hpPeople <= 0)
            {
                if (other.GetComponentInParent<EnemyController>().targetAim == gameObject)
                {
                    other.gameObject.GetComponentInParent<EnemyController>().isEnterPeopleArea = false;
                    other.gameObject.GetComponentInParent<EnemyController>().ChooseTarget(PlayerManager.Instance.playerBody);
                    other.gameObject.GetComponentInParent<EnemyController>().SetAttackAnimation(false, null);
                    other.gameObject.GetComponentInParent<AgentController>().SetWayPointDestination(other.gameObject.GetComponentInParent<EnemyController>().targetAim.transform.position);
                }
            }

            if (other.GetComponentInParent<EnemyController>().hpEnemy <= 0)
            {
                if (enemyNearbyCount <= 0)
                {
                    isNearbyEnemy = false;
                }
            }
        }
    }
}
