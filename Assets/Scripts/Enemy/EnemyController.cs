using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    public AudioSource audioSource;

    public int enemyType;
    public int hpEnemy;
    private bool isAttackPeople;

    [HideInInspector] public bool isEnterPlayerArea;
    [HideInInspector] public bool isEnterPeopleArea;

    private bool isExplosion;

    // For check animation normalizedTime start at 1
    private int comboAttack = 0;

    // Check animation attack
    private bool checkAnimAttack;

    public GameObject targetAim;
    public GameObject ExplosionEffect;

    private float timeAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        SetupEnemy();
    }

    void Update()
    {
        HealthEnemy();
        UpdateComboAttack();
    }

    private void FixedUpdate()
    {
        EnemyAttack();
    }

    public void SetExplosion()
    {
        isExplosion = true;
        ExplosionEffect.SetActive(true);
        AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].enemyBomb, transform.position, AudioManager.Instance.enemyBombVol);
    }

    public void ChooseTarget(GameObject target)
    {
        targetAim = target;
    }

    #region Health
    void SetupEnemy()
    {
        if (enemyType == 1)         // Normal Enemy
        {
            hpEnemy = 4;
            timeAttack = 0.3f;
        }
        else if (enemyType == 2)    // Fat Enemy
        {
            hpEnemy = 6;
            timeAttack = 0.32f;
        }
        else if (enemyType == 3)   // Woman Enemy
        {
            hpEnemy = 3;
            timeAttack = 0.35f;
        }
        else if (enemyType == 4)   // Explosion Enemy
        {
            hpEnemy = 3;
        }
    }

    void HealthEnemy()
    {
        if (hpEnemy <= 0)
        {
            transform.GetComponent<RagdollScript>().Dead();
        }
    }
    #endregion

    #region Animations
    public void SetRunAnimation(bool value)
    {
        if (navMeshAgent.enabled)
        {
            navMeshAgent.isStopped = !value;
            animator.SetBool("isRun", value);
        }
    }

    public void SetAttackAnimation(bool value, Transform lookAt)
    {
        SetRunAnimation(!value);

        // Enemy Rotate to Target Direction
        if (value)
        {
            transform.LookAt(lookAt);
        }

        if (enemyType != 4)
        {
            animator.SetBool("isAttack", value);

            // Start check normalizedTime at 1
            comboAttack = 1;
        }
    }
    #endregion

    #region Attack
    void UpdateComboAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= comboAttack)
            {
                comboAttack += 1;
                checkAnimAttack = false;
            }
        }
    }

    // Check animation frame to attack
    public bool normalizeCheck()
    {
        float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float normalizedTimeInCurrentLoop = normalizedTime - Mathf.Floor(normalizedTime);
        return normalizedTimeInCurrentLoop >= timeAttack && normalizedTimeInCurrentLoop <= timeAttack + 0.2f;
    }

    // Check Enemy attack to player or people
    void EnemyAttack()
    {
        // Enemy Attack to player
        if (isEnterPlayerArea)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
            {
                if (normalizeCheck() && !checkAnimAttack)
                {
                    if (targetAim == PlayerManager.Instance.playerBody)
                    {
                        PlayerManager.Instance.hpPlayer -= GameManager.Instance.enemyDamage_Normal;
                        PlayerManager.Instance.activeVignetteDamage();
                        PlayerManager.Instance.SetCountingTime(false);
                        checkAnimAttack = true;
                        PlayerManager.Instance.ShakeScreen(0.1f);
                    }
                    return;
                }
            }

            if (isExplosion)
            {
                if (targetAim == PlayerManager.Instance.playerBody)
                {
                    isExplosion = false;
                    PlayerManager.Instance.hpPlayer -= GameManager.Instance.enemyDamage_Explosion_Player;
                    PlayerManager.Instance.activeVignetteDamage();
                    PlayerManager.Instance.SetCountingTime(false);
                    hpEnemy = 0;
                    PlayerManager.Instance.ShakeScreen(0.2f);
                }
                return;
            }
        }

        // Enemy Attack to people
        if (isEnterPeopleArea)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
            {
                if (normalizeCheck() && !checkAnimAttack)
                {
                    if (targetAim != null)
                    {
                        targetAim.GetComponent<PeopleController>().hpPeople -= GameManager.Instance.enemyDamage_Normal;
                    }

                    isAttackPeople = true;
                    checkAnimAttack = true;
                    return;
                }
            }

            if (isExplosion)
            {
                isExplosion = false;
                targetAim.GetComponent<PeopleController>().hpPeople -= GameManager.Instance.enemyDamage_Explosion_People;
                isAttackPeople = true;
                hpEnemy = 0;
                return;
            }
        }
        else
        {
            isAttackPeople = false;
        }
    }
    #endregion
}
