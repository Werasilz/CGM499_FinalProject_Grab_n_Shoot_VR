using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeScript : MonoBehaviour
{
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = 0.2f;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < 0.3f)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = 0.3f;
    }

    public AudioSource audioMusic;

    private void Start()
    {
        audioMusic = GameObject.Find("MusicAudio").GetComponent<AudioSource>();
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            FadeIN();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            FadeOUT();
        }
    }*/

    public void FadeIN()
    {
        StopAllCoroutines();
        StartCoroutine(AudioFadeScript.FadeIn(audioMusic, 3f));
    }

    public void FadeOUT()
    {
        StopAllCoroutines();
        StartCoroutine(AudioFadeScript.FadeOut(audioMusic, 3f));
    }
}
