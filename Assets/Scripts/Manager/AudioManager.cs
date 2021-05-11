using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject audioSpawner;
    public AudioClipContain[] AudioClips;

    [Header("Gun")]
    [Range(0.0f, 1.0f)] public float shootVol;
    [Range(0.0f, 1.0f)] public float reloadVol;
    [Range(0.0f, 1.0f)] public float noAmmoVol;
    [Range(0.0f, 1.0f)] public float gunThrowVol;

    [Header("Enemy")]
    [Range(0.0f, 1.0f)] public float hitBodyVol;
    [Range(0.0f, 1.0f)] public float headShotVol;
    [Range(0.0f, 1.0f)] public float enemyDead1Vol;
    [Range(0.0f, 1.0f)] public float enemyDead2Vol;
    [Range(0.0f, 1.0f)] public float enemyDead3Vol;
    [Range(0.0f, 1.0f)] public float enemyDead4Vol;
    [Range(0.0f, 1.0f)] public float enemyBombVol;
    [Range(0.0f, 1.0f)] public float bombFuseVol;

    [Header("Environment")]
    [Range(0.0f, 1.0f)] public float hitEnvironmentVol;
    [Range(0.0f, 1.0f)] public float bombVol;
    [Range(0.0f, 1.0f)] public float gunDropVol;

    [Header("Player")]
    [Range(0.0f, 1.0f)] public float playerHitVol;
    [Range(0.0f, 1.0f)] public float playerRegenerateHPVol;

    [Header("UI")]
    [Range(0.0f, 1.0f)] public float upgradeVol;
    [Range(0.0f, 1.0f)] public float getTokenVol;
    [Range(0.0f, 1.0f)] public float warningVol;
    [Range(0.0f, 1.0f)] public float waveStartVol;
    [Range(0.0f, 1.0f)] public float handButtonVol;
    [Range(0.0f, 1.0f)] public float clickButtonVol;

    [Header("People")]
    [Range(0.0f, 1.0f)] public float peopleDeadVol;
    [Range(0.0f, 1.0f)] public float portalEnterVol;
    [Range(0.0f, 1.0f)] public float peopleScaryVol;

    [Header("Meteor")]
    [Range(0.0f, 1.0f)] public float meteorFlyVol;
    [Range(0.0f, 1.0f)] public float meteorBigExplosionVol;
    [Range(0.0f, 1.0f)] public float meteorSmallExplosionVol;
    [Range(0.0f, 1.0f)] public float meteorHitVol;

    [Header("Boss")]
    [Range(0.0f, 1.0f)] public float bossWalkVol;
    [Range(0.0f, 1.0f)] public float bossRoarVol;
    [Range(0.0f, 1.0f)] public float bossThrowVol;
    [Range(0.0f, 1.0f)] public float bossMeleeAttackVol;
    [Range(0.0f, 1.0f)] public float bossChargeVol;
    [Range(0.0f, 1.0f)] public float bossIdleVol;
    [Range(0.0f, 1.0f)] public float bossDeadVol;

    [Header("Background")]
    [Range(0.0f, 1.0f)] public float bossMusicSoundVol;

    public static AudioManager Instance;

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

    public void XRPlayAudio(AudioClip audioClip, Vector3 position, float volume)
    {
        GameObject audioSpawn = Instantiate(audioSpawner, position, Quaternion.identity);
        audioSpawn.transform.SetParent(gameObject.transform);
        audioSpawn.GetComponent<AudioSource>().clip = audioClip;
        audioSpawn.GetComponent<AudioSource>().volume = volume;
        audioSpawn.GetComponent<AudioSource>().Play();
        Destroy(audioSpawn, 5f);
    }
}

[System.Serializable]
public class AudioClipContain
{
    [Header("Gun")]
    public AudioClip shoot;
    public AudioClip reload;
    public AudioClip noAmmo;
    public AudioClip gunThrow;

    [Header("Enemy")]
    public AudioClip hitBody;
    public AudioClip headShot;
    public AudioClip enemyDead1;
    public AudioClip enemyDead2;
    public AudioClip enemyDead3;
    public AudioClip enemyDead4;
    public AudioClip enemyBomb;
    public AudioClip bombFuse;

    [Header("Environment")]
    public AudioClip hitEnvironment;
    public AudioClip bomb;
    public AudioClip gunDrop;

    [Header("Player")]
    public AudioClip playerHit;
    public AudioClip playerRegenerateHP;

    [Header("UI")]
    public AudioClip upgrade;
    public AudioClip getToken;
    public AudioClip warning;
    public AudioClip waveStart;
    public AudioClip handButton;
    public AudioClip clickButton;

    [Header("People")]
    public AudioClip peopleDead;
    public AudioClip portalEnter;
    public AudioClip peopleScary;

    [Header("Meteor")]
    public AudioClip meteorFly;
    public AudioClip meteorBigExplosion;
    public AudioClip meteorSmallExplosion;
    public AudioClip meteorHit;

    [Header("Boss")]
    public AudioClip bossWalk;
    public AudioClip bossRoar;
    public AudioClip bossThrow;
    public AudioClip bossMeleeAttack;
    public AudioClip bossCharge;
    public AudioClip bossIdle;
    public AudioClip bossDead;

    [Header("Background")]
    public AudioClip bossMusicSound;
}
