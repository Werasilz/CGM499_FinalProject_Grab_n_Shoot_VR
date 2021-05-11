using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] bool isBomb;

    public GameObject BombEffect;

    public void StartBomb()
    {
        isBomb = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isBomb)
        {
            if (other.gameObject.CompareTag("EnemyArea"))
            {
                other.gameObject.GetComponentInParent<EnemyController>().hpEnemy -= GameManager.Instance.bombDamage;
                other.gameObject.GetComponentInParent<RagdollScript>().dieFormBomb = true;
            }
            else if (other.gameObject.CompareTag("PeopleArea"))
            {
                other.gameObject.GetComponentInParent<PeopleController>().hpPeople -= GameManager.Instance.bombDamage;
                other.gameObject.GetComponentInParent<RagdollScript>().dieFormBomb = true;
            }

            BombEffect.SetActive(true);
            BombEffect.transform.SetParent(null);
            Destroy(gameObject);
        }
    }
}
