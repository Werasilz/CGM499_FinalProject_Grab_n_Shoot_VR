using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPeopleManager : MonoBehaviour
{
    public GameObject[] peoplePrefab;
    public GameObject[] spawnPeoplePoint;
    public List<WayPoint> wayPoint;

    public int peopleOnField;
    public float peopleEscape;
    public int peopleDead;

    public float stackEscape;
    public int requireEscape;

    private int scoreTemp;

    public GameObject[] X_mark;
    public GameObject[] PeopleIcon;
    public int deadLimit;

    public static SpawnPeopleManager Instance;

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

    public void SetupSpawnPeopleManager()
    {
        if (GameManager.Instance.level > 0)
        {
            FindingArray();
        }

        deadLimit = X_mark.Length;
        peopleOnField = 0;
        peopleEscape = 0;
        peopleDead = 0;
        stackEscape = 0;
        requireEscape = 3;
        scoreTemp = 0;
        StartCoroutine(RandomSpawnPeople());
    }

    public void FindingArray()
    {
        for (int i = 0; i < spawnPeoplePoint.Length; i++)
        {
            spawnPeoplePoint[i] = GameObject.Find("PeopleSpawnPoints").transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < wayPoint.Count; i++)
        {
            wayPoint[i] = GameObject.Find("PeopleWayPoints").transform.GetChild(i).GetComponent<WayPoint>();
        }
        for (int i = 0; i < X_mark.Length; i++)
        {
            X_mark[i] = GameObject.Find("All_Icon_People").transform.GetChild(i).transform.GetChild(1).gameObject;
        }
    }

    public void StopSpawnPeople()
    {
        StopAllCoroutines();
    }

    public IEnumerator RandomSpawnPeople()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(GameManager.Instance.People_minRandomRate, GameManager.Instance.People_maxRandomRate + 1));
            int i = Random.Range(0, spawnPeoplePoint.Length);

            if (!PlayerManager.Instance.isGameOver && WaveManager.Instance.waveCount < 10)
            {
                if (WaveManager.Instance.spawnTemp[i] == 0)
                {
                    int peopleType = Random.Range(0, peoplePrefab.Length);
                    GameObject peopleClone = Instantiate(peoplePrefab[peopleType], spawnPeoplePoint[i].transform.position, Quaternion.identity);
                    peopleClone.GetComponent<WayPointController>().currentWayPoint = i;
                    peopleClone.transform.SetParent(GameObject.FindGameObjectWithTag("PeopleGroup").transform);
                    peopleOnField += 1;
                    WaveManager.Instance.spawnTemp[i] += 1;
                    WaveManager.Instance.StartCoroutine(WaveManager.Instance.ClearSpawnTemp(i));
                }
            }
        }
    }

    void Update()
    {
        AddDeadLimitByScore();

        if (deadLimit <= 0)
        {
            PlayerManager.Instance.GameOver();
        }

        if (deadLimit > 5)
        {
            deadLimit = 5;
        }
    }

    public void ShowXonPeopleDead()
    {
        if (deadLimit >= 0)
        {
            X_mark[deadLimit].SetActive(true);
            PeopleIcon[deadLimit].SetActive(false);
        }
    }

    public void HideX()
    {
        X_mark[deadLimit - 1].SetActive(false);
        PeopleIcon[deadLimit - 1].SetActive(true);
    }

    void AddDeadLimitByScore()
    {
        if (stackEscape >= scoreTemp + 5)
        {

            scoreTemp += 5;
            deadLimit++;
            HideX();

        }
    }
}
