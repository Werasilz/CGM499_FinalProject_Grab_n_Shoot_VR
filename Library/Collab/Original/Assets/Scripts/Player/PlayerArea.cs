using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyRoot"))
        {
            // Trigger with player collider set enemy to attack player
            other.GetComponentInParent<EnemyController>().targetAim = gameObject;
            other.GetComponentInParent<EnemyController>().SetAttackAnimation(true, gameObject.transform);
            other.GetComponentInParent<EnemyController>().isEnterPlayerArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyRoot"))
        {
            other.GetComponentInParent<EnemyController>().isEnterPlayerArea = false;
        }
    }
}
