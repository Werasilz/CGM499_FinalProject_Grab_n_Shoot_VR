using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Q_Vignette_Single vignetteScreen;
    private Q_Vignette_Single vignetteDamage;

    // For active vignette damage
    [HideInInspector] public bool activeVignette;

    public float headShotCount;

    // For check current warp point number
    public int currentArea;

    public float hpPlayer;
    [SerializeField] public float timeToHealing;

    [HideInInspector] public bool isGameOver;

    public bool isLevelClear;

    public static PlayerManager Instance;

    public GameObject playerBody;

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

    public void SetupPlayerManager()
    {
        playerBody = GameObject.Find("PlayerBody").gameObject;
        vignetteScreen = GameObject.Find("Vignette Canvas").transform.Find("Q_Vignette_Single").GetComponent<Q_Vignette_Single>();
        vignetteDamage = GameObject.Find("Vignette Canvas").transform.Find("Q_Vignette_Single_Damage").GetComponent<Q_Vignette_Single>();

        hpPlayer = GameManager.Instance.maxPlayerHealth;
        headShotCount = 0;
        currentArea = 0;
        activeVignette = false;
        isLevelClear = false;
        timeToHealing = -1;
        StartCoroutine(AddingHealth());
    }

    void Update()
    {
        UpdateHealth();
        CheckHeadShotCount();
        RegenerateHealth();
        VignetteDamage();
    }

    void CheckHeadShotCount()
    {
        if (headShotCount >= GameManager.Instance.requireHeadShot)
        {
            headShotCount = 0;
            GameManager.Instance.tokenCoins += 1;
            AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].getToken, playerBody.transform.position, AudioManager.Instance.getTokenVol);
            UserInterfaceManager.Instance.ShowToken();
            UserInterfaceManager.Instance.SetHeadShotFill();
        }
    }

    public void SetCountingTime(bool isFull)
    {
        if (isFull)
        {
            timeToHealing = 1;
        }
        else
        {
            timeToHealing = 10;
        }
    }

    void RegenerateHealth()
    {
        if (!isGameOver)
        {
            if (timeToHealing > 0)
            {
                timeToHealing -= Time.deltaTime;
            }

            if (hpPlayer >= GameManager.Instance.maxPlayerHealth)
            {
                hpPlayer = GameManager.Instance.maxPlayerHealth;
            }
        }
    }

    IEnumerator AddingHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (timeToHealing < 0 && hpPlayer != GameManager.Instance.maxPlayerHealth)
            {
                hpPlayer += 1;
                AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].playerRegenerateHP, playerBody.transform.position, AudioManager.Instance.playerRegenerateHPVol);
            }
        }
    }

    void UpdateHealth()
    {
        if (hpPlayer != GameManager.Instance.maxPlayerHealth)
        {
            if (hpPlayer >= 11 && hpPlayer <= 15)
            {
                SetVignette(2);
            }
            else if (hpPlayer >= 6 && hpPlayer <= 10)
            {
                SetVignette(3);
            }
            else if (hpPlayer >= 1 && hpPlayer <= 5)
            {
                SetVignette(4);
            }
            else if (hpPlayer <= 0)
            {
                SetVignette(5);
                GameOver();
            }
        }

        else
        {
            if (vignetteScreen.mainScale > 0)
            {
                vignetteScreen.mainScale -= 0.5f * Time.deltaTime;
            }
        }
    }

    void SetVignette(int value)
    {
        if (vignetteScreen.mainScale < value)
        {
            vignetteScreen.mainScale += 1f * Time.deltaTime;
        }
        else return;
    }

    public void activeVignetteDamage()
    {
        activeVignette = true;
        vignetteDamage.mainScale = 5;
        AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].playerHit, playerBody.transform.position, AudioManager.Instance.playerHitVol);
    }

    public void VignetteDamage()
    {
        if (activeVignette)
        {
            vignetteDamage.mainScale -= 5 * Time.deltaTime;

            if (vignetteDamage.mainScale <= 0)
            {
                activeVignette = false;
            }
        }
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            hpPlayer = 0;
            isGameOver = true;
            UserInterfaceManager.Instance.SetNotificationText("GAME OVER", false);
            GameManager.Instance.clearEnemyOnField = true;
            GameManager.Instance.clearPeopleOnField = true;
        }
    }

    public void ClearLevel()
    {
        if (!isLevelClear)
        {
            isLevelClear = true;
            hpPlayer = GameManager.Instance.maxPlayerHealth;
            UserInterfaceManager.Instance.SetNotificationText("Level Clear", false);
            GameManager.Instance.clearEnemyOnField = true;

            ReferenceManager.Instance.endingLevelMenu.gameObject.SetActive(true);
            ReferenceManager.Instance.endingLevelMenu.SetBool("isEnter", true);
        }
    }

    public void ShakeScreen(float input)
    {
        GameObject.Find("Camera Offset").GetComponent<ObjectShake>().ShakeScreen(input);
    }
}
