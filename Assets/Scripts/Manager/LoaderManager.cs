using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoaderManager : MonoBehaviour
{
    public void Start()
    {
        Scene scene = SceneManager.GetActiveScene();

        GameManager.Instance.SetupGameManager();
        ReferenceManager.Instance.SetupReferenceManager();
        UserInterfaceManager.Instance.SetupUserinterfaceManager();
        PlayerManager.Instance.SetupPlayerManager();
        UpgradeManager.Instance.SetupUpgradeManager();

        if (scene.name == "Tutorial")
        {
            GameManager.Instance.level = 0;
            GameObject.Find("TutorialManager").GetComponent<TutorialScript>().SetupTutorial();
        }

        if (scene.name == "CampaignLevel1")
        {
            GameManager.Instance.level = 1;
            WaveManager.Instance.SetupWaveManager();
            SpawnPeopleManager.Instance.SetupSpawnPeopleManager();
        }

        if (scene.name == "CampaignLevel2")
        {
            GameManager.Instance.level = 2;
            WaveManager.Instance.SetupWaveManager();
            SpawnPeopleManager.Instance.SetupSpawnPeopleManager();
        }

        if (scene.name == "CampaignLevel3")
        {
            GameManager.Instance.level = 3;
            WaveManager.Instance.SetupWaveManager();
            SpawnPeopleManager.Instance.SetupSpawnPeopleManager();
        }

        if (scene.name == "Endless")
        {
            GameManager.Instance.level = 99;
            WaveManager.Instance.SetupWaveManager();
            ReferenceManager.Instance.scorePtsText = GameObject.Find("ScorePts_text").GetComponent<TextMeshProUGUI>();
            GameManager.Instance.requireHeadShot = 10;
        }

        ReferenceManager.Instance.SetupEndingMenu();
    }
}
