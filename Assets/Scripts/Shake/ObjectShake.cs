using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    [Tooltip("Seconds to wait before trigerring the explosion particles and the trauma effect")]
    public float Delay = 1;
    [Tooltip("Maximum stress the effect can inflict upon objects Range([0,1])")]
    public float MaximumStress = 0.6f;
    [Tooltip("Maximum distance in which objects are affected by this TraumaInducer")]
    public float Range = 45;

    public void ShakeScreen(float inputStress)
    {
        //PlayParticles();
        MaximumStress = inputStress;

        var targets = UnityEngine.Object.FindObjectsOfType<GameObject>();

        for (int i = 0; i < targets.Length; ++i)
        {
            var receiver = targets[i].GetComponent<ReceiverShake>();
            if (receiver == null) continue;
            float distance = Vector3.Distance(transform.position, targets[i].transform.position);
            if (distance > Range) continue;
            float distance01 = Mathf.Clamp01(distance / Range);
            float stress = (1 - Mathf.Pow(distance01, 2)) * MaximumStress;
            receiver.InduceStress(stress);
        }
    }

    /* Search for all the particle system in the game objects children */
    private void PlayParticles()
    {
        var children = transform.GetComponentsInChildren<ParticleSystem>();
        for (var i = 0; i < children.Length; ++i)
        {
            children[i].Play();
        }
        var current = GetComponent<ParticleSystem>();
        if (current != null) current.Play();
    }
}
