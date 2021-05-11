using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player Setup")]
    public int maxPlayerHealth;
    public int requireHeadShot;
    public int scorePoints;
    public int tokenCoins;

    [Header("Wave Setup")]
    public int prepareTime;
    public int newWaveTime;
    public int bossWarningTime;
    public int minEnemyPerWave;
    public int maxEnemyPerWave;

    [Header("Default Setup")]
    public int cooldownTime_default;
    public int maxBullet_default;
    public int gunRespondTime_default;
    public float autoGunFireRate_default;
    public float BombForce;
    public int bombDamage;

    [Header("Enemy Damage")]
    public int enemyDamage_Normal;
    public int enemyDamage_Explosion_Player;
    public int enemyDamage_Explosion_People;
    public int bossDamage_Melee;

    [Header("Gun Damage")]
    public int damage_HeadShot;
    public int damage_Body;

    [Header("Score Points")]
    public int score_ThrowGun;
    public int score_HeadShot;
    public int score_BodyShot;
    public int score_BossBodyShot;
    public int score_PeopleShot;

    [Header("People Spawn Rate (Seconds)")]
    public int People_minRandomRate;
    public int People_maxRandomRate;

    [Header("Enemy Spawn Rate (Seconds)")]
    public int Enemy_minRandomRate;
    public int Enemy_maxRandomRate;

    [Header("Upgrade Maximum")]
    public int max_Bullet;
    public int max_Cooldown;
    public int max_Health;

    [Header("Show Only")]
    [SerializeField] public int level;
    [SerializeField] private float playerHealth;

    [SerializeField] private int peopleOnField;
    [SerializeField] private float peopleEscape;
    [SerializeField] private int peopleDead;

    [SerializeField] private int waveCount;
    [SerializeField] private int enemyOnField;

    [SerializeField] public static bool normalMode;
    [SerializeField] public static bool hardMode;
    [SerializeField] public static bool easyMode;
    [SerializeField] public static bool veryhardMode;

    [Header("God Mode")]
    [SerializeField] private bool infiniteBullet;
    [SerializeField] private bool infiniteHealth;
    [SerializeField] public bool autoGun;
    public bool clearEnemyOnField;
    public bool clearPeopleOnField;
    private RagdollScript[] enemyRagdoll;
    private RagdollScript[] peopleRagdoll;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetupGameManager()
    {
        maxPlayerHealth = 16;
        requireHeadShot = 5;
        scorePoints = 0;
        tokenCoins = 0;

        prepareTime = 30;
        bossWarningTime = 10;
        minEnemyPerWave = 7;
        maxEnemyPerWave = 10;

        cooldownTime_default = 10;
        maxBullet_default = 12;
        gunRespondTime_default = 5;
        autoGunFireRate_default = 0.2f;
        BombForce = 2000;
        bombDamage = 10;

        enemyDamage_Normal = 1;
        enemyDamage_Explosion_Player = 3;
        enemyDamage_Explosion_People = 5;
        bossDamage_Melee = 5;

        damage_HeadShot = 4;
        damage_Body = 1;

        score_ThrowGun = 250;
        score_HeadShot = 200;
        score_BodyShot = 50;
        score_BossBodyShot = 25;
        score_PeopleShot = 300;

        People_minRandomRate = 10;
        People_maxRandomRate = 15;

        if (easyMode)
        {
            newWaveTime = 9;
            Enemy_minRandomRate = 1;
            Enemy_maxRandomRate = 3;
        }

        if (normalMode)
        {
            newWaveTime = 10;
            Enemy_minRandomRate = 1;
            Enemy_maxRandomRate = 1;
        }

        if (hardMode)
        {
            newWaveTime = 11;
            Enemy_minRandomRate = 0;
            Enemy_maxRandomRate = 1;
        }

        if (veryhardMode)
        {
            newWaveTime = 12;
            Enemy_minRandomRate = 0;
            Enemy_maxRandomRate = 0;
        }

        max_Bullet = 12;
        max_Cooldown = 9;
        max_Health = 30;

        level = 0;

        autoGun = false;
    }

    private void Update()
    {
        GodMode();
        ClearField();
    }

    void ClearField()
    {
        if (clearEnemyOnField)
        {
            GameObject enemyGroup = GameObject.Find("EnemyGroup");
            enemyRagdoll = enemyGroup.transform.GetComponentsInChildren<RagdollScript>();

            for (int i = 0; i < enemyRagdoll.Length; i++)
            {
                enemyRagdoll[i].Dead();
            }

            clearEnemyOnField = false;
        }

        if (level != 99)
        {
            if (clearPeopleOnField)
            {
                GameObject peopleGroup = GameObject.Find("PeopleGroup");
                peopleRagdoll = peopleGroup.transform.GetComponentsInChildren<RagdollScript>();

                for (int i = 0; i < peopleRagdoll.Length; i++)
                {
                    peopleRagdoll[i].Dead();
                }

                clearPeopleOnField = false;
            }
        }
    }

    void GodMode()
    {
        if (infiniteBullet)
        {
            GameObject[] GunFind = GameObject.FindGameObjectsWithTag("GunRoot");

            foreach (GameObject obj in GunFind)
            {
                obj.GetComponentInChildren<GunController>().countBullet = 999;
                obj.GetComponentInChildren<GunController>().UpdateMaxBullet();
                obj.GetComponentInChildren<GunController>().UpdateTextBullet();
                obj.GetComponentInChildren<GunController>().UpdateSkin();
            }
        }

        if (infiniteHealth)
        {
            PlayerManager.Instance.hpPlayer = GameManager.Instance.maxPlayerHealth;
        }
    }

    void LateUpdate()
    {
        playerHealth = PlayerManager.Instance.hpPlayer;

        if (level != 99)
        {
            peopleOnField = SpawnPeopleManager.Instance.peopleOnField;
            peopleEscape = SpawnPeopleManager.Instance.peopleEscape;
            peopleDead = SpawnPeopleManager.Instance.peopleDead;
        }

        waveCount = WaveManager.Instance.waveCount;
        enemyOnField = WaveManager.Instance.enemyOnField;
    }

    public void QuestCollect(int value)
    {
        if (level == 0)
        {
            if (GameObject.Find("TutorialManager").GetComponent<TutorialScript>().questStepNumber == value)
            {
                GameObject.Find("TutorialManager").GetComponent<TutorialScript>().questValueNow += 1;
            }
        }
    }
}

