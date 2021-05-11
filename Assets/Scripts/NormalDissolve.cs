using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDissolve : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private float speed = 1.2f;
    private float t = 0.0f;

    public bool isStartDissolve;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartDissolve)
        {
            Invoke(nameof(StartDissolve), 1f);
        }
    }

    void StartDissolve()
    {
        Material[] mats = meshRenderer.materials;

        mats[0].SetFloat("_Dissolve", Mathf.Sin(t * speed));
        t += Time.deltaTime;

        // Unity does not allow meshRenderer.materials[0]...
        meshRenderer.materials = mats;

        if (mats[0].GetFloat("_Dissolve") >= 0.9f)
        {
            gameObject.SetActive(false);
        }
    }
}
