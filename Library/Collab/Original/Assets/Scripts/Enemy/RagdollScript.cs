using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollScript : MonoBehaviour
{
    private Rigidbody[] bodies;
    private NavMeshAgent agent;
    [Tooltip("True means this prefab is enemy, false means people or not enemy")]
    public bool enemy;
    [HideInInspector] public bool isDead;

    public bool ClickToDead;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetKinematic(true);
    }

    // Kinematic true = ragdoll off
    // Kinematic false = ragdoll on
    void SetKinematic(bool newValue)
    {
        bodies = GetComponentsInChildren<Rigidbody>();      // Get all rigibody in child
        foreach (Rigidbody rb in bodies)                    // set kinematic in every rigibody
        {
            rb.isKinematic = newValue;
        }
    }

    public void Dead()
    {
        if (enemy && GetComponent<EnemyController>().hpEnemy > 0)
        {
            GetComponent<EnemyController>().hpEnemy = 0;
        }

        isDead = true;
        agent.enabled = false;
        SetKinematic(false);
        GetComponent<Animator>().enabled = false;
        Destroy(gameObject, 5);
    }

    private void Update()
    {
        if (ClickToDead)
        {
            Dead();
        }

        /*if (UnityEditor.EditorApplication.isPlaying == false)
        {
            if (enemy)
            {
                WaveManager.Instance.enemyOnField -= 1;
            }
        }*/
    }

    private void OnDestroy()
    {
        if (enemy)
        {
            WaveManager.Instance.enemyOnField -= 1;
        }
    }
}
