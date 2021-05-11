using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject spawnBoss;
    public GameObject spawnBossPoint;

    public GameObject[] enemyPrefab;
    public GameObject[] spawnEnemyPoint;

    public int enemyOnField;
    [SerializeField] public int waveCount;
    private int enemyPerWave;

    private bool hasWave;
    private bool isWaveEnd;

    private bool isBossWave;

    public int[] spawnTemp;

    public static WaveManager Instance;

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

    public void SetupWaveManager()
    {
        if (GameManager.Instance.level > 0)
        {
            SetupSpawn();
        }
    }

    public void SetupSpawn()
    {
        int x = GameObject.Find("EnemySpawnPoints").transform.childCount;
        spawnEnemyPoint = new GameObject[x];

        for (int i = 0; i < spawnEnemyPoint.Length; i++)
        {
            spawnEnemyPoint[i] = GameObject.Find("EnemySpawnPoints").transform.GetChild(i).gameObject;
        }

        enemyOnField = 0;
        waveCount = 0;
        enemyPerWave = 0;
        hasWave = false;
        isWaveEnd = false;
        isBossWave = false;

        if (GameManager.Instance.level != 0 && GameManager.Instance.level != 99)
        {
            spawnBossPoint = GameObject.Find("BossSpawnPoint").gameObject;
        }

        spawnTemp = new int[spawnEnemyPoint.Length];
        waveCount = 0;

        // Start First Enemy Wave
        UserInterfaceManager.Instance.ShowPrepare(true);
        Invoke("StartEnemyWave", GameManager.Instance.prepareTime);
    }

    private void Update()
    {
        StartNewWave();
        StopSpawnEnemy();
        BossWave();
        SetupEndless();

        if (PlayerManager.Instance.isGameOver || PlayerManager.Instance.isLevelClear)
        {
            CancelInvoke();
            GameManager.Instance.clearEnemyOnField = true;
            StopAllCoroutines();
        }
    }

    // Check wave is end
    void StartNewWave()
    {
        if (!PlayerManager.Instance.isGameOver && !PlayerManager.Instance.isLevelClear)
        {
            if (isWaveEnd && !hasWave)
            {
                isWaveEnd = false;
                Invoke(nameof(StartEnemyWave), GameManager.Instance.newWaveTime);
            }
        }
    }

    // Start random spawn enemy
    void StartEnemyWave()
    {
        hasWave = true;

        // Clear spawn temp
        for (int i = 0; i < spawnTemp.Length; i++)
        {
            spawnTemp[i] = 0;
        }

        // Wave start
        if (hasWave)
        {
            if (GameManager.Instance.level != 99)
            {
                if (waveCount != 10)
                {
                    waveCount += 1;
                }

                // Wave UI show only wave 1 - 10
                if (waveCount <= 10 && !isBossWave)
                {
                    UserInterfaceManager.Instance.SetNotificationText("Wave " + waveCount, true);
                    AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].waveStart, PlayerManager.Instance.playerBody.transform.position, AudioManager.Instance.waveStartVol);
                }
            }
            else
            {
                waveCount += 1;
                UserInterfaceManager.Instance.SetNotificationText("Wave " + waveCount, true);
                AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].waveStart, PlayerManager.Instance.playerBody.transform.position, AudioManager.Instance.waveStartVol);
            }


            // Wave 1 start at min 7 max 10
            if (waveCount != 1)
            {
                GameManager.Instance.minEnemyPerWave += 1;
                GameManager.Instance.maxEnemyPerWave += 1;
            }

            // Random Enemy can spawn per wave
            enemyPerWave = Random.Range(GameManager.Instance.minEnemyPerWave, GameManager.Instance.maxEnemyPerWave + 1);

            // Start spawn
            StartCoroutine(RandomSpawnEnemy());
        }
    }

    IEnumerator RandomSpawnEnemy()
    {
        while (true)
        {
            // Random time to spawn
            yield return new WaitForSeconds(Random.Range(GameManager.Instance.Enemy_minRandomRate, GameManager.Instance.Enemy_maxRandomRate + 1));


            // Must have enemy per wave to spawn
            if (enemyPerWave > 0)
            {
                // Find spawn temp to spawn enemy
                int i = Random.Range(0, spawnEnemyPoint.Length);

                // Spawn temp is empty for spawn new enemy 
                if (spawnTemp[i] == 0)
                {
                    GameObject enemyClone = Instantiate(enemyPrefab[EnemyType()], spawnEnemyPoint[i].transform.position, Quaternion.identity);
                    enemyClone.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyGroup").transform);

                    enemyPerWave -= 1;
                    enemyOnField += 1;
                    spawnTemp[i] += 1;

                    // Clear spawn temp after spawn enemy
                    StartCoroutine(ClearSpawnTemp(i));
                }
            }

        }
    }

    void StopSpawnEnemy()
    {
        // Stop spawn when not have enemy per wave to spawn
        if (enemyPerWave <= 0 && hasWave)
        {
            // Stop and reset variable
            StopAllCoroutines();
            isWaveEnd = true;
            enemyPerWave = 0;
            hasWave = false;
        }
    }

    int EnemyType()
    {
        int typeOnSpawn = 0;

        if (waveCount == 1 || waveCount == 2)
        {
            typeOnSpawn = 1;
        }
        else if (waveCount == 3 || waveCount == 4)
        {
            typeOnSpawn = 2;
        }
        else if (waveCount == 5 || waveCount == 6)
        {
            typeOnSpawn = 3;
        }
        else if (waveCount >= 7)
        {
            typeOnSpawn = 4;
        }

        return Random.Range(0, typeOnSpawn);
    }

    public IEnumerator ClearSpawnTemp(int value)
    {
        yield return new WaitForSeconds(5);
        spawnTemp[value] = 0;
        StopCoroutine("ClearSpawnTemp");
    }

    void BossWave()
    {
        if (GameManager.Instance.level != 99)
        {
            if (!isBossWave && waveCount == 10)
            {
                isBossWave = true;

                GameManager.Instance.Enemy_minRandomRate = 1;
                GameManager.Instance.Enemy_maxRandomRate = 3;

                GameManager.Instance.minEnemyPerWave = GameManager.Instance.minEnemyPerWave / 2;
                GameManager.Instance.maxEnemyPerWave = GameManager.Instance.maxEnemyPerWave / 2;

                UserInterfaceManager.Instance.ShowWarning(true);
                AudioManager.Instance.gameObject.GetComponent<AudioFadeScript>().FadeOUT();
                Invoke(nameof(PlayBossMusic), 3);
                Invoke(nameof(SpawnMeteorBoss), GameManager.Instance.bossWarningTime);
            }
        }
    }

    void PlayBossMusic()
    {
        GameObject.Find("MusicAudio").GetComponent<AudioSource>().clip = AudioManager.Instance.AudioClips[0].bossMusicSound;
        AudioManager.Instance.gameObject.GetComponent<AudioFadeScript>().FadeIN();
    }

    void GiveAutoGun()
    {
        UserInterfaceManager.Instance.SetNotificationText("Golden Gun Attached", true);
        GameManager.Instance.autoGun = true;
        UpgradeManager.Instance.UpdateGun();
    }

    void SpawnMeteorBoss()
    {
        GiveAutoGun();
        AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].meteorFly, PlayerManager.Instance.playerBody.transform.position, AudioManager.Instance.meteorFlyVol);
        GameObject bossClone = Instantiate(spawnBoss, spawnBossPoint.transform.position, Quaternion.identity);
    }

    void SetupEndless()
    {
        if (GameManager.Instance.level == 99)
        {
            GameManager.Instance.newWaveTime = 10;
            GameManager.Instance.minEnemyPerWave = 10;
            GameManager.Instance.maxEnemyPerWave = 13;

            if (waveCount < 5)
            {
                GameManager.Instance.Enemy_minRandomRate = 1;
                GameManager.Instance.Enemy_maxRandomRate = 3;
            }
            else if (waveCount > 5 && waveCount < 10)
            {
                GameManager.Instance.Enemy_minRandomRate = 1;
                GameManager.Instance.Enemy_maxRandomRate = 1;
            }
            else if (waveCount > 10 && waveCount < 15)
            {
                GameManager.Instance.Enemy_minRandomRate = 0;
                GameManager.Instance.Enemy_maxRandomRate = 1;
            }
            else if (waveCount > 15)
            {
                GameManager.Instance.Enemy_minRandomRate = 0;
                GameManager.Instance.Enemy_maxRandomRate = 0;
            }
        }
    }
}

