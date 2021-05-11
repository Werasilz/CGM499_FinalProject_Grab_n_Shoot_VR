using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleArea : MonoBehaviour
{
    // Set enemy attack or explosion to people
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyArea"))
        {
            other.gameObject.GetComponentInParent<EnemyController>().isEnterPeopleArea = true;

            if (!transform.parent.GetComponent<RagdollScript>().isDead)
            {
                if (other.gameObject.GetComponentInParent<EnemyController>().enemyType != 4)
                {
                    other.gameObject.GetComponentInParent<EnemyController>().SetAttackAnimation(true, transform.parent.transform);
                }
                else
                {
                    other.gameObject.GetComponentInParent<EnemyController>().SetExplosion();
                }
            }
        }

        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyArea"))
        {
            other.gameObject.GetComponentInParent<EnemyController>().isEnterPeopleArea = false;
        }
    }
}
