using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float speed = 3;
    private float timeToDestroy = 1.2f;

    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        gameObject.transform.position += speed * gameObject.transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("CriticalPoint") || other.gameObject.CompareTag("People"))
        {
            Destroy(gameObject);
        }
    }
}