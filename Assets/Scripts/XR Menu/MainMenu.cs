using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator startMenu;
    public Animator modeMenu;
    public Animator campaignMenu;
    public Animator difficultyMenu;

    public BoxCollider[] startMenuCollider;
    public BoxCollider[] modeMenuCollider;
    public BoxCollider[] campaignMenuCollider;
    public BoxCollider[] difficultyMenuCollider;

    private void Start()
    {
        startMenu.SetBool("isEnter", true);
        modeMenu.SetBool("isEnter", false);
        campaignMenu.SetBool("isEnter", false);
        difficultyMenu.SetBool("isEnter", false);

        startMenuCollider = startMenu.gameObject.GetComponentsInChildren<BoxCollider>();
        modeMenuCollider = modeMenu.gameObject.GetComponentsInChildren<BoxCollider>();
        campaignMenuCollider = campaignMenu.gameObject.GetComponentsInChildren<BoxCollider>();
        difficultyMenuCollider = difficultyMenu.gameObject.GetComponentsInChildren<BoxCollider>();
    }

    public void PlayButton()
    {
        StartCoroutine(SetAnimation(startMenu, modeMenu));
        Invoke(nameof(StopAllCoroutines), 0.2f);
    }

    public void TutorialButton()
    {
        startMenu.SetBool("isEnter", false);
        gameObject.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 100, transform.localPosition.z);
        GameObject.Find("DomeTutorial").GetComponent<NormalDissolve>().isStartDissolve = true;
        GameObject.Find("TutorialManager").GetComponent<TutorialScript>().StartTutorial();
    }

    public void CampaignButton()
    {
        StartCoroutine(SetAnimation(modeMenu, difficultyMenu));
        Invoke(nameof(StopAllCoroutines), 0.2f);
    }

    public void CampaignLevel1()
    {
        SceneManager.LoadScene("CampaignLevel1");
    }

    public void CampaignLevel2()
    {
        SceneManager.LoadScene("CampaignLevel2");
    }

    public void CampaignLevel3()
    {
        SceneManager.LoadScene("CampaignLevel3");
    }

    public void EndlessButton()
    {
        SceneManager.LoadScene("Endless");
    }

    public void SurvivalButton()
    {

    }

    public void EndTutorialButton()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void BackButton()
    {
        if (campaignMenu.GetBool("isEnter"))
        {
            StartCoroutine(SetAnimation(campaignMenu, modeMenu));
            Invoke(nameof(StopAllCoroutines), 0.2f);
        }

        if (modeMenu.GetBool("isEnter"))
        {
            StartCoroutine(SetAnimation(modeMenu, startMenu));
            Invoke(nameof(StopAllCoroutines), 0.2f);
        }
    }

    public void EasyButton()
    {
        GameManager.easyMode = true;
        GameManager.normalMode = false;
        GameManager.hardMode = false;
        GameManager.veryhardMode = false;
        StartCoroutine(SetAnimation(difficultyMenu, campaignMenu));
        Invoke(nameof(StopAllCoroutines), 0.2f);
    }

    public void NormalButton()
    {
        GameManager.easyMode = false;
        GameManager.normalMode = true;
        GameManager.hardMode = false;
        GameManager.veryhardMode = false;
        StartCoroutine(SetAnimation(difficultyMenu, campaignMenu));
        Invoke(nameof(StopAllCoroutines), 0.2f);
    }

    public void HardButton()
    {
        GameManager.easyMode = false;
        GameManager.normalMode = false;
        GameManager.hardMode = true;
        GameManager.veryhardMode = false;
        StartCoroutine(SetAnimation(difficultyMenu, campaignMenu));
        Invoke(nameof(StopAllCoroutines), 0.2f);
    }

    public void VeryHardButton()
    {
        GameManager.easyMode = false;
        GameManager.normalMode = false;
        GameManager.hardMode = false;
        GameManager.veryhardMode = true;
        StartCoroutine(SetAnimation(difficultyMenu, campaignMenu));
        Invoke(nameof(StopAllCoroutines), 0.2f);
    }


    IEnumerator SetAnimation(Animator hide, Animator show)
    {
        while (true)
        {
            if (hide.GetBool("isEnter"))
            {
                hide.SetBool("isEnter", false);
            }

            yield return new WaitForSeconds(0.15f);

            if (!show.GetBool("isEnter"))
            {
                show.SetBool("isEnter", true);
            }
        }
    }

    void DisableCollider(BoxCollider[] toDisable, bool isEnable)
    {
        for (int i = 0; i < toDisable.Length; i++)
        {
            toDisable[i].enabled = isEnable;
        }
    }
}
