using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private BossController bossController;
    private RagdollScript ragdollScript;

    [SerializeField] float calculateBar;

    public float PercentHP;

    void Awake()
    {
        ragdollScript = GetComponentInParent<RagdollScript>();
        bossController = GetComponentInParent<BossController>();
        PercentHP = 100;
    }

    void Update()
    {
        SetScaleBar();
        CheckHealthBoss();
    }

    private void CheckHealthBoss()
    {
        if (PercentHP <= 0 && !ragdollScript.isDead)
        {
            bossController.SetAttackAnimation(false);
            bossController.animator.SetBool("isRoar", false);
            bossController.SetDeadAnimation();
            ragdollScript.Dead();
            PlayerManager.Instance.ClearLevel();
        }
    }

    public void SetScaleBar()
    {
        calculateBar = bossController.hpBoss / bossController.maxHpBoss;
        transform.localScale = new Vector3(calculateBar, transform.localScale.y, transform.localScale.z);
        PercentHP = transform.localScale.x * 100;
        bossController.SetBossScale();
    }
}
