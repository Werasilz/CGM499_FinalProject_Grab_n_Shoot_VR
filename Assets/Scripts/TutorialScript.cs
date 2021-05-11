using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialScript : MonoBehaviour
{
    public GameObject gameCanvas;
    public GameObject tutorialCanvas;
    public GameObject playerBody;

    TextMeshProUGUI tutorialText;
    TextMeshProUGUI questText;
    TextMeshProUGUI objectiveText;

    public string setObjectiveText;
    public string setQuestText;
    public int questValueNow;
    public int questValueRequire;

    public int questStepNumber;
    public bool isFinishQuest;

    private GameObject GroupQuest;


    public void SetupTutorial()
    {
        gameCanvas = GameObject.Find("User Interface").transform.GetChild(0).gameObject;
        gameCanvas.SetActive(false);

        tutorialCanvas = GameObject.Find("User Interface").transform.GetChild(1).gameObject;
        tutorialText = tutorialCanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        questText = tutorialCanvas.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        objectiveText = tutorialCanvas.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

        tutorialCanvas.transform.GetChild(0).gameObject.SetActive(false);
        tutorialCanvas.transform.GetChild(1).gameObject.SetActive(false);
        tutorialCanvas.gameObject.SetActive(false);

        playerBody = GameObject.Find("PlayerBody").gameObject;
        playerBody.SetActive(false);

        // Game Manager setup for tutorial 
        GameManager.Instance.cooldownTime_default = 1;
        GameObject.Find("EndTutorialButton").transform.GetChild(0).gameObject.SetActive(false);

        GroupQuest = GameObject.Find("GroupQuest").gameObject;
    }

    private void Update()
    {
        if (questStepNumber < 11)
        {
            objectiveText.text = setObjectiveText;
            questText.text = setQuestText + questValueNow + "/" + questValueRequire;
        }
        else
        {
            questText.text = "";
            tutorialText.gameObject.SetActive(true);
            tutorialText.text = "Finished tutorial mode";
            tutorialCanvas.transform.GetChild(0).gameObject.SetActive(false);
            tutorialCanvas.transform.GetChild(1).gameObject.SetActive(false);
            GameObject.Find("EndTutorialButton").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("EndTutorialButton").transform.GetChild(0).GetComponent<Animator>().SetBool("isEnter", true);
            SpawnPeopleManager.Instance.StopSpawnPeople();
        }

        CheckQuestFinished();
    }

    public void StartTutorial()
    {
        StartCoroutine(TutorialShow());
        tutorialCanvas.gameObject.SetActive(true);
    }

    IEnumerator TutorialShow()
    {
        yield return new WaitForSeconds(2);
        tutorialText.gameObject.SetActive(true);
        tutorialText.text = "Welcome to training room";

        yield return new WaitForSeconds(5f);
        tutorialText.text = "Let's start!";

        yield return new WaitForSeconds(5f);
        // First Quest
        tutorialText.gameObject.SetActive(false);
        tutorialCanvas.transform.GetChild(0).gameObject.SetActive(true);
        tutorialCanvas.transform.GetChild(1).gameObject.SetActive(true);
        questText.transform.parent.gameObject.SetActive(true);
        SetQuest("Gun", "Grab gun ", 0, 2);
        playerBody.SetActive(true);
        gameCanvas.SetActive(true);
        GameObject.Find("UpgradeButton").transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("UpgradeButton").transform.GetChild(1).gameObject.SetActive(false);
        GameObject.Find("UpgradeButton").transform.GetChild(2).gameObject.SetActive(false);

        StopAllCoroutines();
    }

    void SetQuest(string objective, string text, int now, int require)
    {
        isFinishQuest = false;

        setObjectiveText = objective;
        questValueNow = now;
        questValueRequire = require;
        setQuestText = text;
    }

    int spawnEnemyTutorial = 0;

    void CheckQuestFinished()
    {
        if (questValueNow == questValueRequire)
        {
            if (!isFinishQuest)
            {
                AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].waveStart, PlayerManager.Instance.playerBody.transform.position, AudioManager.Instance.waveStartVol);
                questStepNumber += 1;
                isFinishQuest = true;

                questValueNow = 0;
                questValueRequire = 0;

                if (questStepNumber > 1 && questStepNumber < 11)
                {
                    GroupQuest.transform.GetChild(questStepNumber - 2).gameObject.SetActive(true);
                }

                /*if (questStepNumber > 2)
                {
                    GroupQuest.transform.GetChild(questStepNumber - 3).gameObject.SetActive(false);
                }*/

                if (questStepNumber == 2)
                {
                    SetQuest("Kill Enemy", "Shoot enemy ", 0, 5);
                }

                if (questStepNumber == 3)
                {
                    SetQuest("Reload", "Grab in air ", 0, 2);
                }

                if (questStepNumber == 4)
                {
                    SetQuest("Boom!", "Shoot bomb ", 0, 3);
                }

                if (questStepNumber == 5)
                {
                    SetQuest("No ammo?", "Throw gun to enemy ", 0, 3);

                }

                if (questStepNumber == 6)
                {
                    SetQuest("Head Hunter", "Get Headshot ", 0, 10);
                    GameManager.Instance.max_Health = 27;
                    spawnEnemyTutorial = 0;
                    InvokeRepeating(nameof(SpawnWaveTutorial), 0.5f, 7);
                }

                if (questStepNumber == 7)
                {
                    SetQuest("Time to upgrade", "Upgrade bullet ", 0, 3);
                    GameManager.Instance.tokenCoins += 3;

                    CancelInvoke();
                    GameObject.Find("UpgradeButton").transform.GetChild(0).gameObject.SetActive(true);
                }

                if (questStepNumber == 8)
                {
                    SetQuest("Time to upgrade", "Upgrade cooldown ", 0, 3);
                    GameManager.Instance.tokenCoins += 3;

                    GameObject.Find("UpgradeButton").transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
                    GameObject.Find("UpgradeButton").transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                    GameObject.Find("UpgradeButton").transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                    GameObject.Find("UpgradeButton").transform.GetChild(1).gameObject.SetActive(true);

                }

                if (questStepNumber == 9)
                {
                    SetQuest("Time to upgrade", "Upgrade health ", 0, 3);
                    GameManager.Instance.tokenCoins += 3;
                    GameObject.Find("UpgradeButton").transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;
                    GameObject.Find("UpgradeButton").transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                    GameObject.Find("UpgradeButton").transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                    GameObject.Find("UpgradeButton").transform.GetChild(2).gameObject.SetActive(true);
                }


                if (questStepNumber == 10)
                {
                    SetQuest("Escape to portal", "Save people ", 0, 3);
                    GameManager.Instance.tokenCoins = 0;
                    GameObject.Find("UpgradeButton").transform.GetChild(2).GetComponent<BoxCollider>().enabled = false;
                    GameObject.Find("UpgradeButton").transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                    GameObject.Find("UpgradeButton").transform.GetChild(2).GetChild(1).gameObject.SetActive(true);

                    GameManager.Instance.People_minRandomRate = 1;
                    GameManager.Instance.People_maxRandomRate = 3;


                    //Spawn People
                    WaveManager.Instance.spawnTemp = new int[WaveManager.Instance.spawnEnemyPoint.Length];
                    SpawnPeopleManager.Instance.FindingArray();
                    SpawnPeopleManager.Instance.SetupSpawnPeopleManager();

                    //Spawn Enemy
                    spawnEnemyTutorial = 0;
                    InvokeRepeating(nameof(SpawnWaveTutorial), 0.5f, 7);
                }
            }
        }
    }

    void SpawnWaveTutorial()
    {
        GameObject.Find("Quest" + questStepNumber).transform.GetChild(spawnEnemyTutorial).gameObject.SetActive(true);
        spawnEnemyTutorial++;
    }
}
