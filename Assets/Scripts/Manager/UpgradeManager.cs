using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UpgradeManager : MonoBehaviour
{
    public int cooldownTemp;
    public int bulletTemp;
    public GameObject[] upgradeButton;

    public GameObject[] GunFind;

    public static UpgradeManager Instance;

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

    public void SetupUpgradeManager()
    {
        cooldownTemp = 0;
        bulletTemp = 0;
        upgradeButton[0] = GameObject.Find("UpgradeButton").transform.GetChild(0).gameObject;
        upgradeButton[1] = GameObject.Find("UpgradeButton").transform.GetChild(1).gameObject;
        upgradeButton[2] = GameObject.Find("UpgradeButton").transform.GetChild(2).gameObject;
    }

    void Update()
    {
        if (GameManager.Instance.tokenCoins >= 1)
        {
            SetUpButton(1, true);
        }
        else if (GameManager.Instance.tokenCoins == 0)
        {
            SetUpButton(0.35f, false);
        }

        CheckMax();
    }

    /// <summary>
    /// Bullet max = 24
    /// Cooldown max = 9
    /// Health max = 30
    /// </summary>

    void CheckMax()
    {
        if (bulletTemp == GameManager.Instance.max_Bullet)
        {
            MaxButtonSetup(0);
        }

        if (cooldownTemp == GameManager.Instance.max_Cooldown)
        {
            MaxButtonSetup(1);
        }

        if (GameManager.Instance.maxPlayerHealth == GameManager.Instance.max_Health)
        {
            MaxButtonSetup(2);
        }
    }

    public void UpgradeHealth()
    {
        if (GameManager.Instance.maxPlayerHealth < GameManager.Instance.max_Health)
        {
            GameManager.Instance.maxPlayerHealth += 1;
            PlayerManager.Instance.SetCountingTime(true);
            AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].upgrade, PlayerManager.Instance.playerBody.transform.position, AudioManager.Instance.upgradeVol);
        }
    }

    public void UpgradeCooldown()
    {
        if (cooldownTemp < GameManager.Instance.max_Cooldown)
        {
            cooldownTemp += 1;
            AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].upgrade, PlayerManager.Instance.playerBody.transform.position, AudioManager.Instance.upgradeVol);
        }
    }

    public void UpgradeBullet()
    {
        if (bulletTemp < GameManager.Instance.max_Bullet)
        {
            bulletTemp += 1;
            UpdateGun();
            AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].upgrade, PlayerManager.Instance.playerBody.transform.position, AudioManager.Instance.upgradeVol);
        }
    }

    public void UpdateGun()
    {
        GunFind = GameObject.FindGameObjectsWithTag("GunRoot");

        foreach (GameObject obj in GunFind)
        {
            obj.GetComponentInChildren<GunController>().UpdateMaxBullet();
            obj.GetComponentInChildren<GunController>().UpdateTextBullet();
            obj.GetComponentInChildren<GunController>().UpdateSkin();
            obj.GetComponentInChildren<GunController>().UIGunController.SetTextColor();
        }
    }

    void SetUpButton(float num, bool value)
    {
        for (int i = 0; i < upgradeButton.Length; i++)
        {
            upgradeButton[i].GetComponent<XRSimpleInteractable>().enabled = value;
            upgradeButton[i].GetComponent<SpriteRenderer>().color = new Color(num, num, num);
            upgradeButton[i].transform.Find("Arrow").gameObject.SetActive(value);
        }
    }

    void MaxButtonSetup(int value)
    {
        upgradeButton[value].transform.GetChild(0).gameObject.SetActive(false);
        upgradeButton[value].transform.GetChild(1).gameObject.SetActive(true);
        upgradeButton[value].GetComponent<XRSimpleInteractable>().enabled = false;
        upgradeButton[value].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }
}
