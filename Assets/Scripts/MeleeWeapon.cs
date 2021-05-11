using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("CriticalPoint"))
        {
            // Enemy Body
            GameManager.Instance.scorePoints += GameManager.Instance.score_ThrowGun;
            UserInterfaceManager.Instance.SetScorePtsText();
            Vector3 relativePos = other.transform.position - transform.position;
            GameObject spawnedDecal = GameObject.Instantiate(ReferenceManager.Instance.headShotEffect, other.transform.position, Quaternion.LookRotation(relativePos));
            spawnedDecal.transform.SetParent(other.transform);
            other.transform.GetComponentInParent<RagdollScript>().Dead();

            // Enemy Head
            if (other.gameObject.CompareTag("CriticalPoint"))
            {
                GameObject popupClone = Instantiate(GetComponentInChildren<GunController>().popupHeadShot, other.transform.position, Quaternion.identity);
                PlayerManager.Instance.headShotCount += 1;
                UserInterfaceManager.Instance.SetHeadShotFill();
            }
        }
    }
}
