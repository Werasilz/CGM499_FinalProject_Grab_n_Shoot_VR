using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    public static UserInterfaceManager Instance;

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

    public void SetupUserinterfaceManager()
    {
        ReferenceManager.Instance.notificationText.text = "";
        ReferenceManager.Instance.tokenText.text = "";
    }

    public void SetHeadShotFill()
    {
        ReferenceManager.Instance.headshotFill.fillAmount = PlayerManager.Instance.headShotCount / GameManager.Instance.requireHeadShot;

        if (ReferenceManager.Instance.headshotFill.fillAmount == 1)
        {
            GameObject.Find("HeadShot_Score").transform.Find("EnergyPushEffect").GetComponent<ParticleSystem>().Play();
        }
    }

    public void SetEscapeFill()
    {
        if (SpawnPeopleManager.Instance.peopleEscape >= SpawnPeopleManager.Instance.requireEscape)
        {
            GameManager.Instance.tokenCoins += 1;
            AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].getToken, PlayerManager.Instance.playerBody.transform.position,AudioManager.Instance.getTokenVol);
            ShowToken();
            SpawnPeopleManager.Instance.peopleEscape = 0;
        }

        ReferenceManager.Instance.scoreFill.fillAmount = SpawnPeopleManager.Instance.peopleEscape / SpawnPeopleManager.Instance.requireEscape;

        if (ReferenceManager.Instance.scoreFill.fillAmount == 1)
        {
            GameObject.Find("Help_Score").transform.Find("EnergyPushEffect").GetComponent<ParticleSystem>().Play();
        }
    }

    public void SetNotificationText(string Value, bool clearText)
    {
        ReferenceManager.Instance.notificationText.text = Value;

        if (clearText)
        {
            Invoke("ClearNoticeText", 5);
        }
    }

    public void ShowToken()
    {
        ReferenceManager.Instance.tokenText.text = "+1 Token";
        Invoke("ClearTokenText", 3);
    }

    void ClearNoticeText()
    {
        ReferenceManager.Instance.notificationText.text = "";
    }

    void ClearTokenText()
    {
        ReferenceManager.Instance.tokenText.text = "";
    }

    public void ShowWarning(bool isShow)
    {
        ReferenceManager.Instance.warningBar.SetActive(isShow);
        AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].warning, PlayerManager.Instance.playerBody.transform.position,AudioManager.Instance.warningVol);
        Invoke(nameof(HideBar), 5);
    }

    public void ShowPrepare(bool isShow)
    {
        ReferenceManager.Instance.prepareBar.SetActive(isShow);
        Invoke(nameof(HideBar), 15);
    }

    void HideBar()
    {
        ReferenceManager.Instance.prepareBar.SetActive(false);
        ReferenceManager.Instance.warningBar.SetActive(false);
    }

    public void SetScorePtsText()
    {
        if (GameManager.Instance.level == 99)
        {
            ReferenceManager.Instance.scorePtsText.text = GameManager.Instance.scorePoints.ToString();
        }
    }
}
