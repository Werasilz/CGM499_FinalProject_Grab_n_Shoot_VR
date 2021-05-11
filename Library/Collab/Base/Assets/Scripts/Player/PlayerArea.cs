using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyRoot"))
        {
            other.GetComponentInParent<EnemyController>().SetAttackAnimation(true);
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
