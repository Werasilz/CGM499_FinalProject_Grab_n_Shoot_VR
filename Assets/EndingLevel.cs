using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingLevel : MonoBehaviour
{
    public void NextLevel()
    {
        if (GameManager.Instance.level == 1)
        {
            SceneManager.LoadScene("CampaignLevel2");
        }

        if (GameManager.Instance.level == 2)
        {
            SceneManager.LoadScene("CampaignLevel3");
        }

        if (GameManager.Instance.level == 3)
        {
            Destroy(GameObject.Find("Manager").gameObject);
            SceneManager.LoadScene("Tutorial");
        }
    }
}
