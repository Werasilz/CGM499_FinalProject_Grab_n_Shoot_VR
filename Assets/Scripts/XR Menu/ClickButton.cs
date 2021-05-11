using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ClickButton : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "XR Menu")
        {
            // Show XR Menu
            if (!other.transform.GetChild(0).GetComponent<Animator>().GetBool("IsHover"))
            {
                other.transform.GetChild(0).GetComponent<Animator>().SetBool("IsHover", true);
            }
            else
            {
                other.transform.GetChild(0).GetComponent<Animator>().SetBool("IsHover", false);
            }

            other.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
            other.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
            AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].handButton, gameObject.transform.position, AudioManager.Instance.handButtonVol);
        }

        if (other.transform.name == "Restart_Btn")
        {
            // Show RestartConfirm
            if (!other.transform.parent.GetChild(3).gameObject.activeInHierarchy)
            {
                other.transform.parent.GetChild(3).gameObject.SetActive(true);
                other.transform.parent.GetChild(4).gameObject.SetActive(false);
                AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].clickButton, gameObject.transform.position, AudioManager.Instance.clickButtonVol);
            }
        }

        if (other.transform.name == "Exit_Btn")
        {
            // Show ExitConfirm
            if (!other.transform.parent.GetChild(4).gameObject.activeInHierarchy)
            {
                other.transform.parent.GetChild(4).gameObject.SetActive(true);
                other.transform.parent.GetChild(3).gameObject.SetActive(false);
                AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].clickButton, gameObject.transform.position, AudioManager.Instance.clickButtonVol);
            }
        }

        if (other.transform.name == "Yes")
        {
            if (other.transform.parent.transform.name == "RestartConfirm")
            {
                AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].clickButton, gameObject.transform.position, AudioManager.Instance.clickButtonVol);
                StartCoroutine(LoadScene(null));
            }

            if (other.transform.parent.transform.name == "ExitConfirm")
            {
                AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].clickButton, gameObject.transform.position, AudioManager.Instance.clickButtonVol);
                StartCoroutine(LoadScene(0));
            }
        }

        if (other.transform.name == "No")
        {
            // Hide RestartConfirm
            if (other.transform.parent.gameObject.activeInHierarchy)
            {
                other.transform.parent.gameObject.SetActive(false);
                AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].clickButton, gameObject.transform.position, AudioManager.Instance.clickButtonVol);
            }
        }
    }

    IEnumerator LoadScene(int? level)
    {
        yield return new WaitForSeconds(1.5f);

        if (level == 0)
        {
            SceneManager.LoadScene("Tutorial");
        }

        if (level == null)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        StopAllCoroutines();
    }
}
