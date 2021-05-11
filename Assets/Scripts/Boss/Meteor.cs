using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    Rigidbody rigiBody;
    Transform target;
    private float speed = 7f;
    int[] ro;
    public int HP = 7;

    void Awake()
    {
        target = PlayerManager.Instance.playerBody.transform;
        rigiBody = GetComponent<Rigidbody>();
        ro = new int[3];
    }
    void Start()
    {
        for (int i = 0; i < ro.Length; i++)
        {
            ro[i] = Random.Range(1, 3);
        }

        rigiBody.AddForce((target.position - transform.position) * speed);
    }

    void FixedUpdate()
    {
        transform.Rotate(ro[0], ro[1], ro[2]);
    }
}
