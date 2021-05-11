using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator animator;
    public GameObject targetAim;
    [HideInInspector] public HealthBar healthBar;

    public Transform transformSpawnMeteor;
    public GameObject Meteor;
    private GameObject bossWall;
    private Vector3 bossSoundPosition;
    bool isThrowing;

    // For check animation normalizedTime start at 1
    [SerializeField] private int comboAttack = 0;

    // Check animation attack
    private bool checkAnimAttack;

    [HideInInspector] public float maxHpBoss = 500;
    public float hpBoss;

    public bool isEnterPlayerArea;

    // Melee attack for close range
    public bool isMeleeAttack;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<HealthBar>();
        bossWall = GameObject.Find("BossWall");
        bossSoundPosition = bossWall.transform.position;
        animator.SetBool("isJump", true);
    }

    void Start()
    {
        hpBoss = maxHpBoss;
        comboAttack = 0;

        // Start AI
        ChooseTarget(PlayerManager.Instance.playerBody);

        Invoke(nameof(StartState), 2);
    }

    void StartState()
    {
        animator.SetBool("isJump", false);
        SetRunAnimation(true);
        SetWayPointDestination(targetAim.transform.position);
    }

    private void Update()
    {
        if (hpBoss > 0)
        {
            CheckAttackFinish();
            CheckRoarFinish();
        }

        SetBossScale();
        UpdateComboAttack();
    }

    IEnumerator BossWalkSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.4f);
            AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].bossWalk, bossSoundPosition, AudioManager.Instance.bossWalkVol);
        }
    }

    private void FixedUpdate()
    {
        if (hpBoss > 0)
        {
            transform.LookAt(targetAim.transform);
        }

        BossAttack();
        ThrowMeteor();
    }

    #region AI 
    public void ChooseTarget(GameObject target)
    {
        targetAim = target;
    }

    public void SetWayPointDestination(Vector3 target)
    {
        navMeshAgent.SetDestination(target);
    }
    #endregion

    #region Animations
    public void SetRunAnimation(bool value)
    {
        if (navMeshAgent.enabled)
        {
            navMeshAgent.isStopped = !value;
            animator.SetBool("isWalk", value);

            if (value)
            {
                StartCoroutine(BossWalkSound());
            }
            else
            {
                Invoke(nameof(StopAllCoroutines), 2.8f);
            }
        }
    }

    public void SetAttackAnimation(bool value)
    {
        if (!isMeleeAttack)
        {
            // Far Attack
            animator.SetBool("isAttack", value);
        }
        else
        {
            // Close Attack
            animator.SetBool("isMelee", value);

            // Start check normalizedTime at 1
        }

        comboAttack = 1;
    }

    bool isRoar;

    public void SetRoarAnimation()
    {
        if (animator.GetBool("isCharge") == true)
        {
            animator.SetBool("isCharge", false);
        }

        animator.SetBool("isRoar", true);
        isIdle = false;
    }

    public void RoarSound()
    {
        if (!isRoar)
        {
            isRoar = true;
            AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].bossRoar, bossSoundPosition, AudioManager.Instance.bossRoarVol);
        }
    }

    public void SetChargeAnimation()
    {
        animator.SetBool("isCharge", true);
        AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].bossCharge, bossSoundPosition, AudioManager.Instance.bossChargeVol);
    }

    public void SetDeadAnimation()
    {
        animator.SetBool("isDead", true);
        AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].bossDead, bossSoundPosition, AudioManager.Instance.bossDeadVol);
    }
    #endregion

    #region Checking Animation
    // Loop animation Start > Roar > Attack > Loop
    // Check attack animation is finish change to roar animation
    // Check roar animation is finish change to attack animation
    public void CheckRoarFinish()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Roaring"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                animator.SetBool("isRoar", false);
                isRoar = false;
                SetAttackAnimation(true);
            }
        }
    }

    bool isIdle;

    public void CheckAttackFinish()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                animator.SetBool("isAttack", false);

                if (!isIdle)
                {
                    isIdle = true;
                    AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].bossIdle, bossSoundPosition, AudioManager.Instance.bossIdleVol);
                }

                Invoke(nameof(SetRoarAnimation), 3f);
                Invoke(nameof(RoarSound), 3f);
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Melee"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                animator.SetBool("isMelee", false);

                if (!isIdle)
                {
                    isIdle = true;
                    AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].bossIdle, bossSoundPosition, AudioManager.Instance.bossIdleVol);
                }

                Invoke(nameof(SetRoarAnimation), 5f);
                Invoke(nameof(RoarSound), 3f);
            }
        }
    }
    #endregion

    // Set new scale when health decrease
    public void SetBossScale()
    {
        if (transform.localScale.x >= 6)
        {
            for (int i = 9; i + 1 >= transform.localScale.x; i--)
            {
                if (healthBar.PercentHP <= 10 * i && transform.localScale.x == i + 1)
                {
                    transform.localScale = new Vector3(i, i, i);
                    Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
                    GameObject spawnedDecal = GameObject.Instantiate(ReferenceManager.Instance.explosionFire, spawnPosition, Quaternion.identity);
                    spawnedDecal.transform.SetParent(transform);

                    SetStopFarAttack(false);
                    SetChargeAnimation();
                    Invoke(nameof(SetRoarAnimation), 3);
                    Invoke(nameof(RoarSound), 3f);
                }
            }
        }
        else
        {
            ChangeToMeleeAttack();
        }
    }

    // Check limit scale for change attack to melee
    void ChangeToMeleeAttack()
    {
        if (!isMeleeAttack && transform.localScale.x == 5)
        {
            hpBoss -= 50;
            isMeleeAttack = true;
            SetStopFarAttack(true);
        }
    }

    void SetStopFarAttack(bool continueWalk)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || animator.GetCurrentAnimatorStateInfo(0).IsName("Roaring"))
        {
            // Stop all animation
            animator.SetBool("isRoar", false);
            animator.SetBool("isAttack", false);

            if (continueWalk)
            {
                // Set run and walk to target
                SetRunAnimation(true);
                SetWayPointDestination(targetAim.transform.position);
            }
        }
    }

    #region Attacking
    // Check animation frame to attack
    public bool isMeleeAttacked()
    {
        float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float normalizedTimeInCurrentLoop = normalizedTime - Mathf.Floor(normalizedTime);
        return normalizedTimeInCurrentLoop >= 0.3f && normalizedTimeInCurrentLoop <= 0.5f;
    }

    public bool isThrowAttacked()
    {
        float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float normalizedTimeInCurrentLoop = normalizedTime - Mathf.Floor(normalizedTime);
        return normalizedTimeInCurrentLoop >= 0.48f && normalizedTimeInCurrentLoop <= 0.6f;
    }

    // For attack once
    void UpdateComboAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Melee") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= comboAttack)
            {
                comboAttack += 1;
                checkAnimAttack = false;
            }
        }
    }

    // Check Enemy attack to player
    void BossAttack()
    {
        if (isEnterPlayerArea)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Melee"))
            {
                if (isMeleeAttacked() && !checkAnimAttack)
                {
                    AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].bossMeleeAttack, bossSoundPosition, AudioManager.Instance.bossMeleeAttackVol);
                    PlayerManager.Instance.hpPlayer -= GameManager.Instance.bossDamage_Melee;
                    PlayerManager.Instance.activeVignetteDamage();
                    PlayerManager.Instance.SetCountingTime(false);
                    checkAnimAttack = true;
                    PlayerManager.Instance.ShakeScreen(0.2f);
                    return;
                }
            }

            if (PlayerManager.Instance.hpPlayer <= 0)
            {
                SetAttackAnimation(false);
                animator.SetBool("isRoar", false);
            }
        }
    }
    #endregion

    void ThrowMeteor()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (isThrowAttacked() && !checkAnimAttack)
            {
                checkAnimAttack = true;
                Instantiate(Meteor, transformSpawnMeteor.position, Quaternion.identity);
                AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].bossThrow, bossSoundPosition, AudioManager.Instance.bossThrowVol);
            }
        }
    }
}
