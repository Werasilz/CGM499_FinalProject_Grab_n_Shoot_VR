using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionGunController : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        // Throw gun to enemy 
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("CriticalPoint"))
        {
            if (other.gameObject.GetComponentInParent<EnemyController>().hpEnemy > 0)
            {
                if (transform.position.y > -0.8f)
                {
                    // Enemy Body
                    GameManager.Instance.scorePoints += GameManager.Instance.score_ThrowGun;
                    UserInterfaceManager.Instance.SetScorePtsText();
                    Vector3 relativePos = other.transform.position - transform.position;
                    GameObject spawnedDecal = GameObject.Instantiate(ReferenceManager.Instance.headShotEffect, other.transform.position, Quaternion.LookRotation(relativePos));
                    spawnedDecal.transform.SetParent(other.transform);
                    Destroy(gameObject);
                    other.transform.GetComponentInParent<RagdollScript>().Dead();

                    // Enemy Head
                    if (other.gameObject.CompareTag("CriticalPoint"))
                    {
                        GameObject popupClone = Instantiate(GetComponentInChildren<GunController>().popupHeadShot, other.transform.position, Quaternion.identity);
                        PlayerManager.Instance.headShotCount += 1;
                        UserInterfaceManager.Instance.SetHeadShotFill();
                    }

                    AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].gunThrow, transform.position, AudioManager.Instance.gunThrowVol);
                    GameManager.Instance.QuestCollect(5);
                }
            }
        }
    }
}
