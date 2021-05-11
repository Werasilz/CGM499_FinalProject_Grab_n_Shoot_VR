using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollScript : MonoBehaviour
{
    private Rigidbody[] bodies;
    private NavMeshAgent navMeshAgent;

    public bool isEnemy;
    public bool isBoss;
    public bool ForceDead;

    [HideInInspector] public bool isEnterPortal;
    [HideInInspector] public bool isDead;


    public bool dieFormBomb;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetKinematic(true);
    }

    private void Update()
    {
        ActiveForceDead();
    }

    private void ActiveForceDead()
    {
        if (ForceDead)
        {
            Dead();
        }
    }

    // Kinematic true = ragdoll off, false = ragdoll on
    void SetKinematic(bool newValue)
    {
        // Get all rigibody in child
        bodies = GetComponentsInChildren<Rigidbody>();

        // set kinematic in every rigibody   
        foreach (Rigidbody rb in bodies)
        {
            rb.isKinematic = newValue;
            if (dieFormBomb)
            {
                rb.AddForce(Vector3.up * GameManager.Instance.BombForce);
            }
        }
    }

    public void Dead()
    {
        if (!isDead)
        {
            isDead = true;
            navMeshAgent.enabled = false;

            // For Boss
            if (isBoss)
            {
                GetComponent<BossController>().hpBoss = 0;
            }
            else
            {
                DisableAnimator();
                // For Enemy
                if (isEnemy)
                {
                    SetKinematic(false);
                    GetComponent<EnemyController>().hpEnemy = 0;
                    WaveManager.Instance.enemyOnField -= 1;

                    if (GetComponent<EnemyController>().targetAim.CompareTag("PeopleDome"))
                    {
                        GetComponent<EnemyController>().targetAim.GetComponent<PeopleController>().enemyNearbyCount -= 1;
                        GetComponent<EnemyController>().targetAim = null;
                    }

                    /*if (GetComponent<EnemyController>().enemyType == 4)
                    {
                        GetComponent<EnemyController>().SetExplosion();
                    }*/

                    if (GetComponent<EnemyController>().enemyType == 1)
                    {
                        AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].enemyDead1, transform.position, AudioManager.Instance.enemyDead1Vol);
                    }
                    else if (GetComponent<EnemyController>().enemyType == 2)
                    {
                        AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].enemyDead2, transform.position, AudioManager.Instance.enemyDead2Vol);
                    }
                    else if (GetComponent<EnemyController>().enemyType == 3)
                    {
                        AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].enemyDead3, transform.position, AudioManager.Instance.enemyDead3Vol);
                    }
                    else if (GetComponent<EnemyController>().enemyType == 4)
                    {
                        AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].enemyDead4, transform.position, AudioManager.Instance.enemyDead4Vol);
                    }

                    GameManager.Instance.QuestCollect(2);
                }
                // For People
                else
                {
                    SetKinematic(false);
                    GetComponent<PeopleController>().hpPeople = 0;
                    SpawnPeopleManager.Instance.peopleOnField -= 1;
                    AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].peopleDead, transform.position, AudioManager.Instance.peopleDeadVol);

                    if (!isEnterPortal)
                    {
                        SpawnPeopleManager.Instance.peopleDead += 1;
                        SpawnPeopleManager.Instance.deadLimit--;
                        SpawnPeopleManager.Instance.ShowXonPeopleDead();
                    }
                }

                Invoke(nameof(SpawnParticle), 3);
                Destroy(gameObject, 5);
            }
        }
    }

    void SpawnParticle()
    {
        Vector3 spawnPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);

        if (isBoss)
        {
            Instantiate(ReferenceManager.Instance.explosionBossEffect, spawnPosition, Quaternion.identity);
        }
        else
        {
            GameObject effectClone = Instantiate(ReferenceManager.Instance.energyPullEffect, spawnPosition, Quaternion.identity);
            effectClone.transform.SetParent(gameObject.transform);
        }
    }

    void DisableAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
}
