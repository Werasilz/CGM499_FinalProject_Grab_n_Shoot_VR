using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPopup : MonoBehaviour
{
    private Camera mainCamera;

    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        SetRotationToCamera();
    }

    void SetRotationToCamera()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, mainCamera.transform.eulerAngles.y, transform.rotation.z);
    }
}
