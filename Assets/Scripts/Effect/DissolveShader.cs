using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveShader : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private RagdollScript ragdollScript;

    private bool isDissolve;

    private float speed = 1.2f;
    private float t = 0.0f;

    private void Start()
    {
        skinnedMeshRenderer = this.GetComponent<SkinnedMeshRenderer>();
        ragdollScript = GetComponentInParent<RagdollScript>();
    }

    private void Update()
    {
        if (ragdollScript.isDead)
        {
            Invoke(nameof(StartDissolve), 4f);
        }
    }

    void StartDissolve()
    {
        Material[] mats = skinnedMeshRenderer.materials;

        mats[0].SetFloat("_Dissolve", Mathf.Sin(t * speed));
        t += Time.deltaTime;

        // Unity does not allow meshRenderer.materials[0]...
        skinnedMeshRenderer.materials = mats;
    }
}
