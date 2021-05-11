using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    Rigidbody rigidBody;

    public GameObject Boss;
    public Transform targetDrop;
    public GameObject spawnEffect;
    int[] rotation;

    private MeshRenderer meshRenderer;

    private bool isDissolve;

    private float speed = 5f;
    private float t = 0.0f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        targetDrop = GameObject.Find("Drop Boss Position").GetComponent<Transform>();
        rotation = new int[3];

        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    void Start()
    {
        rigidBody.AddForce((targetDrop.position - transform.position) * 15);

        for (int i = 0; i < rotation.Length; i++)
        {
            rotation[i] = Random.Range(1, 3);
        }
    }

    private void Update()
    {

        //Spawn Boss on field
        if (transform.position.y <= 1)
        {
            PlayerManager.Instance.ShakeScreen(0.4f);
            spawnBossMonster();
            AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].meteorBigExplosion, PlayerManager.Instance.playerBody.transform.position, AudioManager.Instance.meteorBigExplosionVol);
            GameObject effect = Instantiate(spawnEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5);
            Destroy(gameObject);
        }

        Dissolve();
    }

    void Dissolve()
    {
        Material[] mats = meshRenderer.materials;

        mats[0].SetFloat("_Cutoff", Mathf.Sin(t * speed));
        t += Time.deltaTime;

        meshRenderer.materials = mats;
    }

    void spawnBossMonster()
    {
        //Spawn
        GameObject boss = Instantiate(Boss, transform.position, Quaternion.identity);

        //Check Scene
        if (GameManager.Instance.level == 1)
        {
            boss.transform.GetChild(3).GetComponent<SkinnedMeshRenderer>().material = ReferenceManager.Instance.bossMaterial[0];
        }
        else if (GameManager.Instance.level == 2)
        {
            boss.transform.GetChild(3).GetComponent<SkinnedMeshRenderer>().material = ReferenceManager.Instance.bossMaterial[1];
        }
        else if (GameManager.Instance.level == 3)
        {
            boss.transform.GetChild(3).GetComponent<SkinnedMeshRenderer>().material = ReferenceManager.Instance.bossMaterial[2];
        }


    }
}
