using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private int enemyType;
    [HideInInspector] public int hpEnemy = 3;
    public bool isEnterPlayerArea;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Enemy Type
    /// 1 Normal : walk to player and attack
    /// 2 Slow : walk to player and attack
    /// 3 Fast : walk around random walk to player attack 1 hit and leave
    /// 4 Bomb : walk to player and bomb
    /// </summary>

    void Start()
    {
        if (WaveManager.Instance.waveCount == 1)                // Enemy type 1 - 2
        {
            enemyType = Random.Range(1, 3);
        }
        else if (WaveManager.Instance.waveCount == 2)           // Enemy type 1 - 3
        {
            enemyType = Random.Range(1, 4);
        }
        else if (WaveManager.Instance.waveCount == 3)           // Enemy type 1 - 4
        {
            enemyType = Random.Range(1, 5);
        }
        else
        {
            enemyType = Random.Range(1, 5);
        }

        SetRunAnimation(true);
    }

    void SetRunAnimation(bool value)
    {
        animator.SetBool("Run0" + enemyType, value);
    }

    public void SetAttackAnimation(bool value)
    {
        SetRunAnimation(false);

        agent.enabled = false;
        transform.LookAt(WaveManager.Instance.wayPoint[0].transform);

        if (enemyType == 1)
        {
            animator.SetBool("NormalAttack01", value);
        }
        else if (enemyType == 2)
        {
            animator.SetBool("HardAttack01", value);
        }
        else if (enemyType == 3)
        {
            animator.SetBool("HardAttack02", value);
        }
    }

    public bool isAttacked()
    {
        float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float normalizedTimeInCurrentLoop = normalizedTime - Mathf.Floor(normalizedTime);
        return normalizedTimeInCurrentLoop >= 0.3f && normalizedTimeInCurrentLoop <= 0.305f;
    }

    void Update()
    {
        HealthEnemy();
    }

    private void FixedUpdate()
    {
        OnEnemyAttack();
        EnemyAttack();
    }

    void HealthEnemy()
    {
        if (hpEnemy <= 0)
        {
            transform.GetComponent<RagdollScript>().Dead();
        }
    }

    void OnEnemyAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hard Attack 1") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Hard Attack 2") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Normal Attack 1"))
        {
            isAttacked();
        }
    }

    void EnemyAttack()
    {
        if (isEnterPlayerArea)
        {
            if (isAttacked())
            {
                PlayerManager.Instance.hpPlayer -= 1;
                return;
            }
        }
    }
}
