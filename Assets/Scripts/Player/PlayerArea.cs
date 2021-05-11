using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArea : MonoBehaviour
{
    // Set enemy attack or explosion to player
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyArea"))
        {
            other.gameObject.GetComponentInParent<EnemyController>().isEnterPlayerArea = true;

            if (other.gameObject.GetComponentInParent<EnemyController>().enemyType != 4)
            {
                other.gameObject.GetComponentInParent<EnemyController>().SetAttackAnimation(true, gameObject.transform);
            }
            else
            {
                if (other.gameObject.GetComponentInParent<EnemyController>().hpEnemy > 0)
                {
                    other.gameObject.GetComponentInParent<EnemyController>().SetExplosion();
                }
            }
        }

        if (other.gameObject.CompareTag("BossArea"))
        {
            other.gameObject.GetComponentInParent<BossController>().isEnterPlayerArea = true;
            other.gameObject.GetComponentInParent<BossController>().SetRunAnimation(false);
            other.gameObject.GetComponentInParent<BossController>().SetRoarAnimation();
        }

        if (other.gameObject.CompareTag("Meteor"))
        {
            GameObject effect = Instantiate(ReferenceManager.Instance.explosionMeteorEffect, other.transform.position, Quaternion.identity);
            Destroy(effect, 2);
            Destroy(other.gameObject);
            PlayerManager.Instance.hpPlayer -= GameManager.Instance.bossDamage_Melee;
            PlayerManager.Instance.activeVignetteDamage();
            PlayerManager.Instance.SetCountingTime(false);
            PlayerManager.Instance.ShakeScreen(0.2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyArea"))
        {
            other.gameObject.GetComponentInParent<EnemyController>().isEnterPlayerArea = false;
        }
    }
}
