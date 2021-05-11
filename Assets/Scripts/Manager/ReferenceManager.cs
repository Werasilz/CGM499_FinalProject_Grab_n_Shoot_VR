using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReferenceManager : MonoBehaviour
{
    public ParticleSystem muzzleFlash;
    public ParticleSystem cartridgeEjection;
    public GameObject stoneEffect;
    public GameObject bulletShotEffect;
    public GameObject headShotEffect;
    public GameObject bloodEffect;
    public GameObject explosionEffect;
    public GameObject explosionFire;
    public GameObject explosionSmallStone;
    public GameObject energyPullEffect;
    public GameObject explosionBossEffect;
    public GameObject explosionMeteorEffect;

    public TextMeshProUGUI notificationText;
    public TextMeshProUGUI tokenText;
    public TextMeshProUGUI scorePtsText;

    public Image headshotFill;
    public Image scoreFill;

    public GameObject warningBar;
    public GameObject prepareBar;

    public Material[] skinGun;

    public Material[] bossMaterial;

    public Animator endingLevelMenu;

    public static ReferenceManager Instance;

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

    public void SetupReferenceManager()
    {
        warningBar = GameObject.Find("BarInterface").transform.GetChild(0).gameObject;
        prepareBar = GameObject.Find("BarInterface").transform.GetChild(1).gameObject;
        headshotFill = GameObject.Find("HeadShot_Score").transform.Find("Circle").GetChild(0).GetComponent<Image>();
        scoreFill = GameObject.Find("Help_Score").transform.Find("Circle").GetChild(0).GetComponent<Image>();
        notificationText = GameObject.Find("Notification_Text").GetComponent<TextMeshProUGUI>();
        tokenText = GameObject.Find("Token_Text").GetComponent<TextMeshProUGUI>();
    }

    public void SetupEndingMenu()
    {
        if (GameManager.Instance.level != 0 && GameManager.Instance.level != 99)
        {
            endingLevelMenu = GameObject.Find("EndingLevel").gameObject.GetComponentInChildren<Animator>();
            endingLevelMenu.SetBool("isEnter", false);
            endingLevelMenu.gameObject.SetActive(false);
        }
    }
}
