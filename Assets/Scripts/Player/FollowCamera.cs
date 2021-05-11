using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform MainCamera;

    void Awake()
    {
        MainCamera = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(MainCamera.position.x, MainCamera.position.y - 0.7f, MainCamera.position.z - 0.06f);
        transform.rotation = Quaternion.Euler(0, MainCamera.eulerAngles.y, 0);
    }
}
